//LevelEntity.cs
//by MAURIZIO FISCHETTI

using System.Collections;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEntity : MonoBehaviour
{
    [SerializeField] public Vector3 Position;
    [SerializeField] public Quaternion Rotation;
    [SerializeField] public Vector3 Scale;

    [SerializeField] public Rigidbody RB;

    [SerializeField] public bool Floating = false;
    [SerializeField] public float FloatingAmplitude = 0.3f;
    [SerializeField] public float FloatingFrequency = 1.0f;
    [SerializeField] public float MoveSpeed;
    [SerializeField] public bool MoveToPlayer = false;
    private Vector3 Direction;

    public bool IsStopped = false;

    private void OnEnable()
    {
        EventManager EM = GameManager.Instance.EventManager;
        EM.LevelStart += OnLevelStart;
        EM.LevelStop += OnLevelStop;
    }

    private void OnDisable()
    {
        EventManager EM = GameManager.Instance.EventManager;
        EM.LevelStart -= OnLevelStart;
        EM.LevelStop -= OnLevelStop;
    }

    #region Events Methods

    private void OnLevelStart()
    {
        IsStopped = false;
    }

    private void OnLevelStop()
    {
        IsStopped = true;
    }

    #endregion

    protected virtual void Start()
    {
        Position = transform.localPosition;
        Rotation = transform.localRotation;
        Scale = transform.localScale;

        MoveSpeed = GameManager.Instance.Level.LevelSpeed;
        GameManager.Instance.Level.LevelObjects.Add(this);

        RB = gameObject.GetComponent<Rigidbody>();
        Direction = Vector3.zero;
    }

    protected virtual void Update()
    {
        if (RB == null)
        {
            Move();
        }

        if (transform.position.x <= T4P.T4Project.XVisualLimit.x)
        {
            gameObject.SetActive(false);
        }

        if(transform.position.y <= T4P.T4Project.YVisualLimit.x || transform.position.y >= T4P.T4Project.YVisualLimit.y)
        {
            gameObject.SetActive(false);
        }
        
    }

    protected virtual void FixedUpdate()
    {
        if (RB != null)
        {
            Move();
        }
    }

    private void Move()
    {
        if (!IsStopped)
        {
            Direction.x = -1;
            if (!MoveToPlayer)
            {
                if (Floating)
                {
                    Direction.y = FloatingAmplitude * Mathf.Sin(FloatingFrequency * Time.time);
                }
            }
            else
            {
                Direction.x = GameManager.Instance.Player.transform.position.x - transform.position.x;
                Direction.y = GameManager.Instance.Player.transform.position.y - transform.position.y;
            }

            //move with physics
            if (RB != null)
            {
                RB.MovePosition(RB.position + Direction.normalized * MoveSpeed * Time.fixedDeltaTime);
            }

            //move without physics
            if (RB == null)
            {
                transform.position += Direction.normalized * MoveSpeed * Time.deltaTime;
            }
        }
    }
}
