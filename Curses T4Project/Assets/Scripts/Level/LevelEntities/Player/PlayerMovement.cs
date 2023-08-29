//PlayerMovement.cs
//by ANTHONY FEDELI

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerMovement.cs manages the Movement between Layers of play and the idle animation of the Ship
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _InputMovementReference;
    [SerializeField] private GameObject _Ship;

    [Header("Movement Variables")]
    [SerializeField] private bool _BackToSurfaceInBossBattle = false;
    [SerializeField] private float _ChangingLayerSpeed = 2f;
    [SerializeField] private float _ImmersionRotationSpeed = 30f;
    [SerializeField] private Vector3 _DefaultShipRotation = new Vector3(0f, 0f, 90f);
    [SerializeField] private float _MovementCD = 1f;
    private float _MovementRecharge = 0f;
    private float _TimeForChangingLayer;
    private Vector3 _ActualPosition;
    private Vector3 _NextPosition;
    private float _TravelDistance;
    private float _BaseChangingLayerSpeed;

    [Header("Debuff")]
    [SerializeField] private bool _IsSlowed = false;
    [SerializeField] private ParticleSystem _SlowEffect;
    [SerializeField] private float _ChangingLayerSpeedMultiplier = 1;
    [SerializeField] private float _SlowChagingLayerSpeedDuration;
    [SerializeField] private bool _CanRotateWithDebuff = false;

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
        _BaseChangingLayerSpeed = _ChangingLayerSpeed;
        _ChangingLayerSpeedMultiplier = 1f;
    }
    private void Start()
    {
        SetStartingPosition();
    }
    private void Update()
    {
        if (!GameManager.Instance.LevelManager.Player.IsChangingLayer && !GameManager.Instance.LevelManager.Player.IsInStartAnimation && _MovementRecharge < _MovementCD)
        {
            _MovementRecharge += Time.deltaTime;
            if (_MovementRecharge >= _MovementCD)
            {
                GameManager.Instance.LevelManager.Player.CanMove = true;
            }
        }

        if (_IsSlowed)
        {
            _SlowChagingLayerSpeedDuration -= Time.deltaTime;
            if (_SlowChagingLayerSpeedDuration <= 0)
            {
                if (!GameManager.Instance.LevelManager.Player.IsChangingLayer)
                {
                    _SlowEffect.Stop();
                    _SlowChagingLayerSpeedDuration = 0f;
                    _ChangingLayerSpeedMultiplier = 1f;
                    _ChangingLayerSpeed = _BaseChangingLayerSpeed;
                    _IsSlowed = false;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
        {
            if (GameManager.Instance.LevelManager.Player.IsInStartAnimation)
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
            if (GameManager.Instance.LevelManager.Player.IsChangingLayer)
            {
                PlayingMovement();
            }
            else
            {
                EndMovement();
            }
        }
    }

    #region Movement
    private void SetStartingPosition()
    {
        //Set Starting Position
        _ActualPosition = new Vector3(GameManager.Instance.LevelManager.CurrentLevel.XStartingPosition,
                                      GameManager.Instance.LevelManager.CurrentLevel.ActualLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer,
                                      0f);
    }
    private void OnMovementInput()
    {
        // To use the input the ship need to be in idle animation and not changing layer
        if (!GameManager.Instance.LevelManager.Player.IsChangingLayer &&
            !GameManager.Instance.LevelManager.Player.IsInStartAnimation &&
            GameManager.Instance.LevelManager.Player.CanMove &&
            GameManager.Instance.LevelManager.CurrentLevel.NOfLayersUnderWater > 0 &&
            !GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)

        {
            //set the direction of the movement
            _Direction.y = _InputMovementReference.action.ReadValue<float>();

            // based on the direction value and the layer where the ship is, the ship will change its layer
            // Go Down
            if (_Direction.y < 0 &&
                GameManager.Instance.LevelManager.CurrentLevel.ActualLayer > -GameManager.Instance.LevelManager.CurrentLevel.NOfLayersUnderWater)
            {
                //start the movement animation and the CD before move again
                GameManager.Instance.LevelManager.Player.IsChangingLayer = true;
                GameManager.Instance.LevelManager.Player.CanMove = false;

                //reset the timers need for the movement animation
                _TimeForChangingLayer = 0f;
                _FloatingTime = 0f;

                //set the next layer position
                _NextPosition = _ActualPosition + new Vector3(0f, _Direction.y * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer, 0f);

                //set a travel distance based on the idle animation positition and the next position
                PickDistance(_NextPosition.y);

                //start the immersion animation
                StartCoroutine(RotateAnimation(_Direction.y));
            }
            //Go Up
            else if (_Direction.y > 0 &&
                     GameManager.Instance.LevelManager.CurrentLevel.ActualLayer < 0)
            {
                //start the movement animation and the CD before move again
                GameManager.Instance.LevelManager.Player.IsChangingLayer = true;
                GameManager.Instance.LevelManager.Player.CanMove = false;


                //reset the timers need for the movement animation
                _TimeForChangingLayer = 0f;
                _FloatingTime = 0f;

                //set the next layer position
                _NextPosition = _ActualPosition + new Vector3(0f, _Direction.y * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer, 0f);

                //set a travel distance based on the idle animation positition and the next position
                PickDistance(_NextPosition.y);

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
            if (!GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
            {
                //Ship is above the actual position
                if (gameObject.transform.position.y > _ActualPosition.y)
                {
                    _TravelDistance = GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer + (gameObject.transform.position.y - _ActualPosition.y);
                }

                //Ship is under the actual position
                else
                {
                    _TravelDistance = GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer - (_ActualPosition.y - gameObject.transform.position.y);
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
            if (!GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
            {
                //Ship is above the actual position
                if (gameObject.transform.position.y > _ActualPosition.y)
                {
                    _TravelDistance = GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer - (gameObject.transform.position.y - _ActualPosition.y);
                }

                //Ship is under the actual position
                else
                {
                    _TravelDistance = GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer + (_ActualPosition.y - gameObject.transform.position.y);
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
        if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer == 0)
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
            GameManager.Instance.LevelManager.Player.IsInStartAnimation = false;

            //MAU - Start the level
            GameManager.Instance.LevelManager.CurrentLevel.StartLevel();
        }
        //move the ship to the play position
        _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime + Vector3.right * _AnimationSpeed * Time.fixedDeltaTime);
    }
    private void PlayingMovement()
    {
        //Check if the ship is changing layer or is in idel animation
        //if the ship is in idle animation
        if (!GameManager.Instance.LevelManager.Player.IsChangingLayer)
        {
            //based on the actual Layer set the stagger effect of the ship on above the water or under of it
            _FloatingTime += Time.deltaTime;
            if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer == 0)
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
                    GameManager.Instance.LevelManager.Player.IsChangingLayer = false;

                    //set the new position
                    gameObject.transform.position = _NextPosition;

                    //Start the timer for be able to move again
                    _MovementRecharge = 0f;

                    //reset float timer
                    _FloatingTime = 0f;

                    //Update dhe actual layer
                    GameManager.Instance.LevelManager.CurrentLevel.SetLayer((int)_Direction.y);

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
                    GameManager.Instance.LevelManager.Player.IsChangingLayer = false;

                    //set the new position
                    gameObject.transform.position = _NextPosition;

                    //Start the timer for be able to move again
                    _MovementRecharge = 0f;

                    //reset float timer
                    _FloatingTime = 0f;

                    //Update dhe actual layer
                    GameManager.Instance.LevelManager.CurrentLevel.SetLayer((int)_Direction.y);

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
    public void BackToSurface()
    {
        _BackToSurfaceInBossBattle = true;
    }
    private void EndMovement()
    {
        //Reset rigidbody constraint
        GameManager.Instance.LevelManager.Player.CanMove = false;
        _Rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        if (!GameManager.Instance.LevelManager.Player.LastDistancePicked && _BackToSurfaceInBossBattle)
        {
            GameManager.Instance.LevelManager.Player.LastDistancePicked = true;
            _TimeForChangingLayer = 0f;
            PickDistance(GameManager.Instance.LevelManager.CurrentLevel.FinalLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer);
            StartCoroutine(RotateAnimation(1f));
        }

        //Move the ship to the Final layer
        if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer != GameManager.Instance.LevelManager.CurrentLevel.FinalLayer && _BackToSurfaceInBossBattle)
        {
            _ActualPosition = gameObject.transform.position;
            _TimeForChangingLayer += Time.deltaTime;

            //mathematical functions to obtain a sinusoidal speed value
            float sinusoidalSpeed = ((Mathf.Sin(Mathf.PI * (((_TimeForChangingLayer * _ChangingLayerSpeed) / _TravelDistance) - 0.5f)) + 1f)) / 2f;
            float speed = _ChangingLayerSpeed * sinusoidalSpeed;

            //Final position under the actual position
            if (GameManager.Instance.LevelManager.CurrentLevel.FinalLayer < GameManager.Instance.LevelManager.CurrentLevel.ActualLayer)
            {
                //Speed based on the final position
                speed *= -1f;

                if (_ActualPosition.y < GameManager.Instance.LevelManager.CurrentLevel.FinalLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer)
                {
                    //Update the actual layer
                    GameManager.Instance.LevelManager.CurrentLevel.ActualLayer = GameManager.Instance.LevelManager.CurrentLevel.FinalLayer;

                    gameObject.transform.position = new Vector3(0f, GameManager.Instance.LevelManager.CurrentLevel.FinalLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer, 0f);
                }
            }
            //Final position above the actual position
            else if (GameManager.Instance.LevelManager.CurrentLevel.FinalLayer > GameManager.Instance.LevelManager.CurrentLevel.ActualLayer)
            {
                if (_ActualPosition.y > GameManager.Instance.LevelManager.CurrentLevel.FinalLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer)
                {
                    //Update the actual layer
                    GameManager.Instance.LevelManager.CurrentLevel.ActualLayer = GameManager.Instance.LevelManager.CurrentLevel.FinalLayer;

                    gameObject.transform.position = new Vector3(0f, GameManager.Instance.LevelManager.CurrentLevel.FinalLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer, 0f);
                }
            }
            _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime);
        }
        //Move the ship to the Intermediate and End X Position
        else
        {
            //Intermediate X Position
            if (!GameManager.Instance.LevelManager.CurrentLevel.IsLevelEnded)
            {
                //Reset rigidbody constraint
                _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

                //based on the actual Layer set the stagger effect of the ship on above the water or under of it
                _FloatingTime += Time.deltaTime;
                if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer == 0)
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

                if (gameObject.transform.position.x < GameManager.Instance.LevelManager.CurrentLevel.XIntermediatePosition)
                {
                    //move the ship to the Intermediate position
                    _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime + Vector3.right * _AnimationSpeed * _AnimationSpeedModifier * Time.fixedDeltaTime);
                }
                else
                {
                    //floating ship
                    gameObject.transform.position = new Vector3(GameManager.Instance.LevelManager.CurrentLevel.XIntermediatePosition, _Rb.position.y, 0f);
                    _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime);
                }
            }
            //End X Position
            else
            {
                _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

                //based on the actual Layer set the stagger effect of the ship on above the water or under of it
                _FloatingTime += Time.deltaTime;
                if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer == 0)
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

                if (gameObject.transform.position.x < GameManager.Instance.LevelManager.CurrentLevel.XEndingPosition)
                {
                    //move the ship to the play position
                    if (GameManager.Instance.LevelManager.CurrentLevel.PlayerHasReachBeach)
                    {
                        _AnimationSpeed = 0;
                    }
                    else
                    {
                        //increase animation speed
                        _AnimationSpeed += Time.fixedDeltaTime;
                    }
                    _Rb.MovePosition(_Rb.position + Vector3.up * speed * Time.fixedDeltaTime + Vector3.right * _AnimationSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    //LEVEL FINISHED
                    //MAU - call END LEVEL
                    GameManager.Instance.LevelManager.CurrentLevel.EndLevel(Level.EndLevelType.Victory);
                }
            }
        }
    }
    private IEnumerator RotateAnimation(float inputDirection)
    {
        while (GameManager.Instance.LevelManager.Player.IsChangingLayer)
        {
            //mathematical functions to obtain a sinusoidal rotation value
            float sinusoidalSpeed = (-Mathf.Cos(Mathf.PI * ((((2f * _TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance))) + 1f) / 2f;
            float direction = inputDirection * Mathf.Sign(Mathf.Sin(Mathf.PI * (((_TimeForChangingLayer) * _ChangingLayerSpeed) / _TravelDistance)));
            float rotationalSpeed = direction * _ImmersionRotationSpeed * sinusoidalSpeed;

            //rotate the ship mesh
            _Ship.transform.Rotate(Vector3.forward, rotationalSpeed * _ChangingLayerSpeedMultiplier * Time.deltaTime);

            yield return null;
        }

        while (GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle && gameObject.transform.position.y < 0f)
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
    #endregion

    #region Debuff
    public void PushAway(float direction)
    {
        ChangeLane(direction);
    }

    private void ChangeLane(float direction)
    {
        // To use the input the ship need to be in idle animation and not changing layer
        if (!GameManager.Instance.LevelManager.Player.IsChangingLayer &&
            !GameManager.Instance.LevelManager.Player.IsInStartAnimation &&
            GameManager.Instance.LevelManager.Player.CanMove &&
            GameManager.Instance.LevelManager.CurrentLevel.NOfLayersUnderWater > 0 &&
            !GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)

        {
            //set the direction of the movement
            _Direction.y = direction;

            // based on the direction value and the layer where the ship is, the ship will change its layer
            // Go Down
            if (_Direction.y < 0 &&
                GameManager.Instance.LevelManager.CurrentLevel.ActualLayer > -GameManager.Instance.LevelManager.CurrentLevel.NOfLayersUnderWater)
            {
                //start the movement animation and the CD before move again
                GameManager.Instance.LevelManager.Player.IsChangingLayer = true;
                GameManager.Instance.LevelManager.Player.CanMove = false;

                //reset the timers need for the movement animation
                _TimeForChangingLayer = 0f;
                _FloatingTime = 0f;

                //set the next layer position
                _NextPosition = _ActualPosition + new Vector3(0f, _Direction.y * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer, 0f);

                //set a travel distance based on the idle animation positition and the next position
                PickDistance(_NextPosition.y);

                //start the immersion animation
                if (_CanRotateWithDebuff)
                {
                    StartCoroutine(RotateAnimation(_Direction.y));
                }
            }
            //Go Up
            else if (_Direction.y > 0 &&
                     GameManager.Instance.LevelManager.CurrentLevel.ActualLayer < 0)
            {
                //start the movement animation and the CD before move again
                GameManager.Instance.LevelManager.Player.IsChangingLayer = true;
                GameManager.Instance.LevelManager.Player.CanMove = false;


                //reset the timers need for the movement animation
                _TimeForChangingLayer = 0f;
                _FloatingTime = 0f;

                //set the next layer position
                _NextPosition = _ActualPosition + new Vector3(0f, _Direction.y * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer, 0f);

                //set a travel distance based on the idle animation positition and the next position
                PickDistance(_NextPosition.y);

                //start the immersion animation
                if (_CanRotateWithDebuff)
                {
                    StartCoroutine(RotateAnimation(_Direction.y));
                }
            }
        }
    }

    public void CoralHitted(float SlowMovementDuration, float ChangingLayerSpeedMultiplier, bool IsSlowed)
    {
        SlowMovement(SlowMovementDuration, ChangingLayerSpeedMultiplier, IsSlowed);
    }

    private void SlowMovement(float SlowMovementDuration, float ChangingLayerSpeedMultiplier, bool IsSlowed)
    {
        _SlowEffect.Play();
        _SlowChagingLayerSpeedDuration = SlowMovementDuration;
        _ChangingLayerSpeedMultiplier = ChangingLayerSpeedMultiplier;
        _ChangingLayerSpeed *= _ChangingLayerSpeedMultiplier;
        _IsSlowed = IsSlowed;
    }
    #endregion
}