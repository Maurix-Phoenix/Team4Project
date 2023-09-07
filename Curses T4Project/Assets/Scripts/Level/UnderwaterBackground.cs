using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterBackground : MonoBehaviour
{
    [SerializeField]

    private bool _CanMove = false;
    private float _ScrollingSpeed = 0.5f;

    private Vector3 _RightEdge;
    private Vector3 _LeftEdge;
    private Vector3 _EdgesDistance;

    private SpriteRenderer _SR; 

    private GameManager _GM;

    private void Awake()
    {
        print("awake is called");

        _SR = GetComponent<SpriteRenderer>();
        _GM = GameManager.Instance;
    }

    private void OnEnable()
    {
        if(_GM != null)
        {
            EventManager EM = _GM.EventManager;
            EM.LevelLoaded += OnLevelLoad;
            EM.LevelStart += OnLevelStart;
            EM.LevelStop += OnLevelStop;
        }

    }

    private void OnDisable()
    {
        if(_GM != null)
        {
            EventManager EM = _GM.EventManager;
            EM.LevelLoaded -= OnLevelLoad;
            EM.LevelStart -= OnLevelStart;
            EM.LevelStop -= OnLevelStop;
        }

    }

    private void Start()
    {
        print("start is called");
        _LeftEdge = new(transform.position.x - (_SR.bounds.extents.x / 3f), transform.position.y, transform.position.z);
        _RightEdge = new(transform.position.x + (_SR.bounds.extents.x / 3f), transform.position.y, transform.position.z);
        _EdgesDistance = _RightEdge - _LeftEdge;
    }

    private void OnLevelLoad()
    {
        print("level loaded is called");
        _LeftEdge = new(transform.position.x - (_SR.bounds.extents.x / 3f), transform.position.y, transform.position.z);
        _RightEdge = new(transform.position.x + (_SR.bounds.extents.x / 3f), transform.position.y, transform.position.z);
        _EdgesDistance = _RightEdge - _LeftEdge;

        _CanMove = false;
    }

    private void OnLevelStart()
    {
        print("levelstart is called");
        _CanMove = true;
    }
    private void OnLevelStop()
    {
        print("levelstop is called");
        _CanMove = false;
    }



    // Update is called once per frame
    void Update()
    {
        ScrollBackgroundRight();
    }

    private void ScrollBackgroundRight()
    {
        if (_CanMove)
        {
            float currentSpeed = _ScrollingSpeed;
            if( _GM != null)
            {
                if(!_GM.LevelManager.CurrentLevel.IsInBossBattle && !_GM.LevelManager.CurrentLevel.IsFinalArrivalBeach)
                {
                    currentSpeed = _ScrollingSpeed + (_GM.LevelManager.CurrentLevel.LevelSpeed / 3);
                }
                else
                {
                    currentSpeed = 0;
                }
            }

            transform.position += currentSpeed * Vector3.left * Time.deltaTime;
            if (transform.position.x < _LeftEdge.x)
            {
                ResetSpritePosition();
            }
        }
    }

    private void ResetSpritePosition()
    {
        transform.position += _EdgesDistance;
    }
}
