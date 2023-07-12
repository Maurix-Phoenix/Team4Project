//PlayerMovement.cs
//by ANTHONY FEDELI

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

/// <summary>
/// PlayerMovement.cs manages the Movement between Layers of play and the idle animation of the Ship
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _InputMovementReference;
    [SerializeField] private GameObject _Ship;

    [Header("Movement Variables")]
    [SerializeField] private float _ChangingLayerSpeed = 2f;
    [SerializeField] private float _ImmersionRotationSpeed = 30f;
    [SerializeField] private Vector3 _DefaultShipRotation = new Vector3(0f, 0f, 90f);
    [SerializeField] private float _MovementCD = 1f;
    private float _MovementRecharge = 0f;
    private float _TimeForChangingLayer;
    private Vector3 _ActualPosition;
    private Vector3 _NextPosition;
    private float _TravelDistance;

    [Header("Floating")]
    [Tooltip("Base Value 3")]
    [SerializeField] private float _AboveWaterFrequency = 3f;
    [Tooltip("Base Value 0.05")]
    [SerializeField] private float _AboveWaterAmplitude = 0.05f;
    [Tooltip("Base Value 1")]
    [SerializeField] private float _UnderWaterFrequency = 1f;
    [Tooltip("Base Value 0.1")]
    [SerializeField] private float _UnderWaterAmplitude = 0.1f;
    private float _FloatSpeed;
    private float _ActualAmplitude;
    private float _FloatingTime;

    [Header("Animation")]
    [SerializeField] private float _AnimationSpeed = 1f;
    [SerializeField] private float _AnimationSpeedModifier = 1f;

    private Rigidbody _Rb;
    private Vector3 _Direction;

    private void Awake()
    {
        _Rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        InitializeShip();
    }

    private void Update()
    {
        if (!Player.ThisPlayer.IsChangingLayer && !Player.ThisPlayer.IsInStartAnimation && _MovementRecharge < _MovementCD)
        {
            _MovementRecharge += Time.deltaTime;
            if (_MovementRecharge >= _MovementCD)
            {
                Player.ThisPlayer.CanMove = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!Level.ThisLevel.IsInBossBattle)
        {
            if (Player.ThisPlayer.IsInStartAnimation)
            {
                StartMovement();
            }
            else
            {
                PlayingMovement();
            }
        }
        else
        {
            if (Player.ThisPlayer.IsChangingLayer)
            {
                PlayingMovement();
            }
            else
            {
                EndMovement();
            }
        }
    }

    private void InitializeShip()
    {
        //Set Starting Position
        _ActualPosition = new Vector3(Level.ThisLevel.XStartingPosition,
                                      Level.ThisLevel.ActualLayer * Level.ThisLevel.UnitSpaceBetweenLayer,
                                      0f);
        gameObject.transform.position = _ActualPosition;
    }
    private void OnMovementInput()
    {
        // To use the input the ship need to be in idle animation and not changing layer
        if (!Player.ThisPlayer.IsChangingLayer && !Player.ThisPlayer.IsInStartAnimation && Player.ThisPlayer.CanMove && Level.ThisLevel.NOfLayersUnderWater > 0 && !Level.ThisLevel.IsInBossBattle)
        {
            if (_InputMovementReference.action.ReadValue<float>() == 0)
            {
                return;
            }

            //start the movement animation and the CD before move again
            Player.ThisPlayer.IsChangingLayer = true;
            Player.ThisPlayer.CanMove = false;

            //set the direction of the movement
            _Direction.y = _InputMovementReference.action.ReadValue<float>();

            //reset the timers need for the movement animation
            _TimeForChangingLayer = 0f;
            _FloatingTime = 0f;

            //set the next layer position
            _NextPosition = _ActualPosition + new Vector3(0f, _Direction.y * Level.ThisLevel.UnitSpaceBetweenLayer, 0f);

            //set a travel distance based on the idle animation positition and the next position
            PickDistance(_NextPosition.y);

            // based on the input value and the layer where the ship is, the ship will change its layer -
            // Go Down
            if (_InputMovementReference.action.ReadValue<float>() < 0 &&
                Level.ThisLevel.ActualLayer > -Level.ThisLevel.NOfLayersUnderWater)
            {
                //start the immersion animation
                StartCoroutine(RotateAnimation(_Direction.y));
            }

            //Go Up
            else if (_InputMovementReference.action.ReadValue<float>() > 0 &&
                     Level.ThisLevel.ActualLayer < 0)
            {
                //start the immersion animation
                StartCoroutine(RotateAnimation(_Direction.y));
            }
        }
    }
    private void PickDistance(float NextYPosition)
    {
        //Ship is above the next position
        if (_ActualPosition.y > NextYPosition)
        {
            if (!Level.ThisLevel.IsInBossBattle)
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
            else
            {
                //Travel distance from actual position and final position
                _TravelDistance = Mathf.Abs(gameObject.transform.position.y) - Mathf.Abs(NextYPosition);
            }
        }

        //Ship is under the next position
        else
        {
            if (!Level.ThisLevel.IsInBossBattle)
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
            else
            {
                //Travel distance from actual position and final position
                _TravelDistance = Mathf.Abs(gameObject.transform.position.y) - Mathf.Abs(NextYPosition);
            }
        }
    }
    private void StartMovement()
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
        float direction = -Mathf.Sign(Mathf.Cos(Mathf.PI * (((_FloatingTime * _FloatSpeed) / 2f) + 1f)));
        float speed = _FloatSpeed * floatingSpeed * direction;

        if (gameObject.transform.position.x >= 0)
        {
            //interrupt the start/end animation
            _Rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            _ActualPosition = new Vector3(0f, gameObject.transform.position.y, 0f);
            gameObject.transform.position = _ActualPosition;
            _FloatingTime = 0f;
            Player.ThisPlayer.IsInStartAnimation = false;

            //MAU - Start the level
            Level.ThisLevel.StartLevel();
        }
        //move the ship to the play position
        _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime + Vector3.right * _AnimationSpeed * Time.fixedDeltaTime);
    }

    private void PlayingMovement()
    {
        //Check if the ship is changing layer or is in idel animation
        //if the ship is in idle animation
        if (!Player.ThisPlayer.IsChangingLayer)
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
            float direction = -Mathf.Sign(Mathf.Cos(Mathf.PI * (((_FloatingTime * _FloatSpeed) / 2f) + 1f)));
            float speed = _FloatSpeed * floatingSpeed * direction;

            //stagger effect above water or under of it
            _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime);
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
                    Player.ThisPlayer.IsChangingLayer = false;

                    //set the new position
                    gameObject.transform.position = _NextPosition;

                    //Start the timer for be able to move again
                    _MovementRecharge = 0f;

                    //reset float timer
                    _FloatingTime = 0f;

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
                    Player.ThisPlayer.IsChangingLayer = false;

                    //set the new position
                    gameObject.transform.position = _NextPosition;

                    //Start the timer for be able to move again
                    _MovementRecharge = 0f;

                    //reset float timer
                    _FloatingTime = 0f;

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

    private void EndMovement()
    {
        //Reset rigidbody constraint
        Player.ThisPlayer.CanMove = false;
        _Rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        if (!Player.ThisPlayer.LastDistancePicked)
        {
            Player.ThisPlayer.LastDistancePicked = true;
            _TimeForChangingLayer = 0f;
            PickDistance(Level.ThisLevel.FinalLayer * Level.ThisLevel.UnitSpaceBetweenLayer);
            StartCoroutine(RotateAnimation(1f));
        }

        //Move the ship to the Final layer
        if (Level.ThisLevel.ActualLayer != Level.ThisLevel.FinalLayer)
        {
            _ActualPosition = gameObject.transform.position;
            _TimeForChangingLayer += Time.deltaTime;

            //mathematical functions to obtain a sinusoidal speed value
            float sinusoidalSpeed = ((Mathf.Sin(Mathf.PI * (((_TimeForChangingLayer * _ChangingLayerSpeed) / _TravelDistance) - 0.5f)) + 1f)) / 2f;
            float speed = _ChangingLayerSpeed * sinusoidalSpeed;

            //Final position under the actual position
            if (Level.ThisLevel.FinalLayer < Level.ThisLevel.ActualLayer)
            {
                //Speed based on the final position
                speed *= -1f;

                if (_ActualPosition.y < Level.ThisLevel.FinalLayer * Level.ThisLevel.UnitSpaceBetweenLayer)
                {
                    //Update the actual layer
                    Level.ThisLevel.ActualLayer = Level.ThisLevel.FinalLayer;

                    gameObject.transform.position = new Vector3(0f, Level.ThisLevel.FinalLayer * Level.ThisLevel.UnitSpaceBetweenLayer, 0f);
                }
            }
            //Final position above the actual position
            else if (Level.ThisLevel.FinalLayer > Level.ThisLevel.ActualLayer)
            {
                if (_ActualPosition.y > Level.ThisLevel.FinalLayer * Level.ThisLevel.UnitSpaceBetweenLayer)
                {
                    //Update the actual layer
                    Level.ThisLevel.ActualLayer = Level.ThisLevel.FinalLayer;

                    gameObject.transform.position = new Vector3(0f, Level.ThisLevel.FinalLayer * Level.ThisLevel.UnitSpaceBetweenLayer, 0f);
                }
            }
            _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime);
        }

        //Move the ship to the Intermediate and End X Position
        else
        {
            //Intermediate X Position
            if (!Level.ThisLevel.IsLevelEnded)
            {
                //Reset rigidbody constraint
                _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

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
                float direction = -Mathf.Sign(Mathf.Cos(Mathf.PI * (((_FloatingTime * _FloatSpeed) / 2f) + 1f)));
                float speed = _FloatSpeed * floatingSpeed * direction;

                if (gameObject.transform.position.x < Level.ThisLevel.XIntermediatePosition)
                {
                    //move the ship to the Intermediate position
                    _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime + Vector3.right * _AnimationSpeed * _AnimationSpeedModifier * Time.fixedDeltaTime);
                }
                else
                {
                    //floating ship
                    gameObject.transform.position = new Vector3(Level.ThisLevel.XIntermediatePosition, _Rb.position.y, 0f);
                    _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime);
                }
            }
            //End X Position
            else
            {
                _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

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
                float direction = -Mathf.Sign(Mathf.Cos(Mathf.PI * (((_FloatingTime * _FloatSpeed) / 2f) + 1f)));
                float speed = _FloatSpeed * floatingSpeed * direction;

                if (gameObject.transform.position.x < Level.ThisLevel.XEndingPosition)
                {
                    //increase animation speed
                    _AnimationSpeed += Time.fixedDeltaTime;

                    //move the ship to the play position
                    _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime + Vector3.right * _AnimationSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    //LEVEL FINISHED
                    //MAU - call endlevel
                    Level.ThisLevel.EndLevel(Level.EndLevelType.Victory);
                }
            }
        }
    }

    private IEnumerator RotateAnimation(float inputDirection)
    {
        while (Player.ThisPlayer.IsChangingLayer)
        {
            //mathematical functions to obtain a sinusoidal rotation value
            float sinusoidalSpeed = (-Mathf.Cos(Mathf.PI * ((((2f * _TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance))) + 1f) / 2f;
            float direction = inputDirection * Mathf.Sign(Mathf.Sin(Mathf.PI * (((_TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance)));
            float rotationalSpeed = direction * _ImmersionRotationSpeed * sinusoidalSpeed;

            //rotate the ship mesh
            _Ship.transform.Rotate(Vector3.forward, rotationalSpeed * Time.deltaTime);

            yield return null;
        }

        while (Level.ThisLevel.IsInBossBattle && gameObject.transform.position.y < 0f)
        {
            //mathematical functions to obtain a sinusoidal rotation value
            float sinusoidalSpeed = (-Mathf.Cos(Mathf.PI * ((((2f * _TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance))) + 1f) / 2f;
            float direction = 0.5f * inputDirection * Mathf.Sign(Mathf.Sin(Mathf.PI * (((_TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance)));
            float rotationalSpeed = direction * _ImmersionRotationSpeed * sinusoidalSpeed;

            //rotate the ship mesh
            _Ship.transform.Rotate(Vector3.forward, rotationalSpeed * Time.deltaTime);

            yield return null;
        }



        //reset the rotation of the mesh to the default value
        _Ship.transform.eulerAngles = _DefaultShipRotation;
    }
}
