//PlayerMovement.cs
//by ANTHONY FEDELI

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// PlayerMovement.cs manages the Movement between Layers of play and the idle animation of the Ship
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _InputMovementReference;
    [SerializeField] private GameObject _Ship;
    
    [Header("Movement Variables")]
    [SerializeField] private bool _IsChangingLayer = false;
    [SerializeField] private float _ChangingLayerSpeed = 2f;
    [SerializeField] private float _ImmersionRotationSpeed = 30f;
    [SerializeField] private Vector3 _DefaultShipRotation = new Vector3(0f, 0f, 90f);
    [SerializeField] private bool _CanMove = false;
    [SerializeField] private float _MovementCD = 1f;
    private float _MovementRecharge = 0f;
    private float _TimeForChangingLayer;
    private Vector3 _ActualPosition;
    private Vector3 _NextPosition;
    private float _TravelDistance;

    //[Header("Ship Y Positions")]
    //[Tooltip("The Layer counting start from 0.")]
    //[SerializeField] private int _NOfLayersUnderWater = 0;
    //[SerializeField] private int _UnitSpaceBetweenLayer = 3;
    //[Tooltip("0 is the layer above the water.\n N Of Layers is the layer on the sea bed.")]
    //[SerializeField] private int _ActualLayer = 0;
    //private float _TravelDistance;

    [Header("Floating")]
    [SerializeField] private float _AboveWaterFrequency = 3f;
    [SerializeField] private float _AboveWaterAmplitude = 0.05f;
    [SerializeField] private float _UnderWaterFrequency = 1f;
    [SerializeField] private float _UnderWaterAmplitude = 0.1f;
    private float _FloatSpeed;
    private float _ActualAmplitude;
    private float _FloatingTime;

    [Header("Start Level Animation")]
    [SerializeField] private float _AnimationSpeed = 1f;
    [SerializeField] private bool _IsInAnimation = true;

    private Rigidbody _Rb;
    private Vector3 _Direction;

    //Property
    public bool IsInAnimation { get { return _IsInAnimation; } }
    public bool IsChangingLayer { get { return _IsChangingLayer; } }


    private void Awake()
    {
        //Initialize the Rigidbody component
        _Rb = GetComponent<Rigidbody>();
        _Rb.useGravity = false;
        _Rb.isKinematic = true;
        _Rb.freezeRotation = true;
        _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _Rb.interpolation = RigidbodyInterpolation.Interpolate;
        _Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    private void Start()
    {
        InitializeShip();
    }

    private void Update()
    {
        if(!_IsChangingLayer && !_IsInAnimation && _MovementRecharge < _MovementCD)
        {
            _MovementRecharge += Time.deltaTime;
            if (_MovementRecharge >= _MovementCD)
            {
                Debug.Log("CanMove");
               _CanMove = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void InitializeShip()
    {
        //Set Starting Position
        _ActualPosition = new Vector3(Level.ThisLevel.StartingXPosition,
                                      Level.ThisLevel.ActualLayer * Level.ThisLevel.UnitSpaceBetweenLayer,
                                      0f);
        gameObject.transform.position = _ActualPosition;
    }
    private void OnMovementInput()
    {
        // To use the input the ship need to be in idle animation and not changing layer
        if (!_IsChangingLayer && !_IsInAnimation && _CanMove && Level.ThisLevel.NOfLayersUnderWater > 0)
        {
            //start the movement animation and the CD before move again
            _IsChangingLayer = true;
            _CanMove = false;

            // based on the input value and the layer where the ship is, the ship will change its layer
            if (_InputMovementReference.action.ReadValue<float>() < 0 &&
                Level.ThisLevel.ActualLayer > -Level.ThisLevel.NOfLayersUnderWater)
            {
                //set the direction of the movement
                _Direction.y = -1;

                //reset the timers need for the movement animation
                _TimeForChangingLayer = 0f;
                _FloatingTime = 0f;

                //set the next layer position
                _NextPosition = _ActualPosition + new Vector3(0f, _Direction.y * Level.ThisLevel.UnitSpaceBetweenLayer, 0f);

                //set a travel distance based on the idle animation positition and the next position
                PickDistance();

                //start the immersion animation
                StartCoroutine(RotateAnimation(_Direction.y));
            }
            else if (_InputMovementReference.action.ReadValue<float>() > 0 &&
                     Level.ThisLevel.ActualLayer < 0)
            {
                //set the direction of the movement
                _Direction.y = 1;

                //reset the timers need for the movement animation
                _TimeForChangingLayer = 0f;
                _FloatingTime = 0f;

                //set the next layer position
                _NextPosition = _ActualPosition + new Vector3(0f, _Direction.y * Level.ThisLevel.UnitSpaceBetweenLayer, 0f);

                //set a travel distance based on the idle animation positition and the next position
                PickDistance();

                //start the immersion animation
                StartCoroutine(RotateAnimation(_Direction.y));
            }
        }
    }
    private void PickDistance()
    {
        //Ship is above the next position
        if (_ActualPosition.y > _NextPosition.y)
        {
            //Ship is above the actual position
            if (gameObject.transform.position.y > _ActualPosition.y)
            {
                _TravelDistance = Level.ThisLevel.UnitSpaceBetweenLayer + (gameObject.transform.position.y - _ActualPosition.y);
            }

            //Ship is under the actual position
            else
            {
                _TravelDistance = Level.ThisLevel.UnitSpaceBetweenLayer - (_ActualPosition.y - gameObject.transform.position.y);
            }
        }

        //Ship is under the next position
        else
        {
            //Ship is above the actual position

            if (gameObject.transform.position.y > _ActualPosition.y)
            {
                _TravelDistance = Level.ThisLevel.UnitSpaceBetweenLayer - (gameObject.transform.position.y - _ActualPosition.y);
            }

            //Ship is under the actual position
            else
            {
                _TravelDistance = Level.ThisLevel.UnitSpaceBetweenLayer + (_ActualPosition.y - gameObject.transform.position.y);
            }
        }
    }
    private void Movement()
    {
        //Check if the ship is changing layer or is in idel animation
        //if the ship is in idle animation
        if (!_IsChangingLayer)
        {

            //based on the actual Layer set the stagger effect of the ship on above the water or under of it
            _FloatingTime += Time.deltaTime;
            if (Level.ThisLevel.ActualLayer == 0)
            {
                _FloatSpeed = _AboveWaterFrequency;
                _ActualAmplitude = _AboveWaterAmplitude;
            }
            else
            {
                _FloatSpeed = _UnderWaterFrequency;
                _ActualAmplitude = _UnderWaterAmplitude;
            }

            //mathematical functions to obtain a sinusoidal speed value
            float floatingSpeed = _ActualAmplitude * (Mathf.Cos(Mathf.PI * ((_FloatingTime * _FloatSpeed))) + 1f);
            float direction = - Mathf.Sign(Mathf.Cos(Mathf.PI * (((_FloatingTime * _FloatSpeed) / 2f) + 1f)));
            float speed = _FloatSpeed * floatingSpeed * direction;

            //check if the ship is in the animation of start/end level
            if (_IsInAnimation)
            {
                //move the ship to the play position
                _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime + Vector3.right * _AnimationSpeed * Time.fixedDeltaTime);

                //after reaching the play position set position and the rigidbody constraint
                if (gameObject.transform.position.x > 0)
                {
                    _Rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                    _ActualPosition = new Vector3(0f, gameObject.transform.position.y, 0f);
                    gameObject.transform.position = _ActualPosition;

                    //interrupt the start/end animation
                    _IsInAnimation = false;
                }
            }
            else
            {
                //stagger effect above water or under of it
                _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime);
            }
        }

        //if the ship is changing layer
        else
        {
            _ActualPosition = gameObject.transform.position;
            _TimeForChangingLayer += Time.deltaTime;

            //mathematical functions to obtain a sinusoidal speed value
            float sinusoidalSpeed = ((Mathf.Sin(Mathf.PI * (((_TimeForChangingLayer * _ChangingLayerSpeed) / _TravelDistance) - 0.5f)) + 1f)) / 2f;
            float speed = _ChangingLayerSpeed * sinusoidalSpeed;

            //based on the input the ship will go up or down
            //down
            if (_Direction.y < 0)
            {
                //Check some position for setting the ship
                if (_ActualPosition.y < _NextPosition.y)
                {
                    //Interrupt the changing layer animatin
                    _IsChangingLayer = false;

                    //set the new position
                    gameObject.transform.position = _NextPosition;

                    //Start the timer for be able to move again
                    _MovementRecharge = 0f;

                    //Update dhe actual layer
                    Level.ThisLevel.SetLayer((int)_Direction.y);

                    //reset the direction
                    _Direction.y = 0;
                }
            }

            //up
            else if (_Direction.y > 0)
            {
                //Check some position for setting the ship
                if (_ActualPosition.y > _NextPosition.y)
                {
                    //Interrupt the changing layer animatin
                    _IsChangingLayer = false;

                    //set the new position
                    gameObject.transform.position = _NextPosition;

                    //Start the timer for be able to move again
                    _MovementRecharge = 0f;

                    //Update dhe actual layer
                    Level.ThisLevel.SetLayer((int)_Direction.y);

                    //reset the direction
                    _Direction.y = 0;
                }
            }

            //Changing layer animation
            if (_Direction.y != 0)
            {
                _Rb.MovePosition(gameObject.transform.position + _Direction * speed * Time.fixedDeltaTime);
            }
        }
    }
    private IEnumerator RotateAnimation(float inputDirection)
    {
        while (_IsChangingLayer)
        {
            //mathematical functions to obtain a sinusoidal rotation value
            float sinusoidalSpeed = (- Mathf.Cos(Mathf.PI * ((((2f * _TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance))) + 1f) / 2f;
            float direction = inputDirection * Mathf.Sign(Mathf.Sin(Mathf.PI * (((_TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance)));
            float rotationalSpeed = direction * _ImmersionRotationSpeed * sinusoidalSpeed;

            //rotate the ship mesh
            _Ship.transform.Rotate(Vector3.forward, rotationalSpeed * Time.deltaTime);

            yield return null;
        }

        //reset the rotation of the mesh to the default value
        _Ship.transform.eulerAngles = _DefaultShipRotation;
    }
}
