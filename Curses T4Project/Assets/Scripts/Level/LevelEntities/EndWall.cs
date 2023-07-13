//EndWall.cs
//by ANTHONY FEDELI


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// EndWall.cs manages the behaviour of the wall at the end of the level.
/// </summary>

public class EndWall : LevelEntity, IDamageable
{
    [Header("References")]
    [SerializeField] private GameObject _CannonballPrefab;
    [SerializeField] private GameObject _CannonPrefab;
    [SerializeField] private GameObject _TriggerMovement;

    [Header("EndWall Variables")]
    [SerializeField] private int _Health = 10;
    [SerializeField] private int _NOfCannonball = 3;

    [Header("Cannons Variables")]
    [SerializeField] private bool _CanShoot = false;
    [SerializeField] private float _CannonHeight = 1.5f;
    [SerializeField] private List<GameObject> _Cannons = new List<GameObject>();
    private GameObject _CannonActiveToShoot;
    public GameObject CannonActiveToShoot { get { return _CannonActiveToShoot; } }
    private Vector3 _CannonSpot;

    [Header("Cannonball Variables")]
    [SerializeField] private int _CannonballDamage = 1;

    [Header("Shoot Variables")]
    [Tooltip("Base Value 10")][SerializeField] private float _MaxDistance = 10f;
    [Tooltip("Base Value 10")][SerializeField] private float _ShootSpeed = 10f;
    [Tooltip("Base Value 17")][Range(0, 90)][SerializeField] private float _TrajectoryAngle = 20f;
    [Tooltip("Base Value 0.2")][SerializeField] private float _ShootCD = 0.2f;
    [Tooltip("Base Value 2")][SerializeField] private float _TimerBeforeFirstShoot = 2f;
    private float _ShootRecharge = 0f;
    private float _FirstShootCharge = 0f;

    [Header("Animation Variables")]
    [Tooltip("Base Value 1")][SerializeField] private float _DespawnTimer = 1f;

    private Rigidbody _Rb;

    protected override void Start()
    {
        base.Start();
        SetCannons();
    }

    private void Awake()
    {
        //Initialize the Rigidbody component
        _Rb = GetComponent<Rigidbody>();
        _Rb.useGravity = false;
        _Rb.isKinematic = true;
        _Rb.interpolation = RigidbodyInterpolation.Interpolate;
        _Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        //Initialize position
        //gameObject.transform.position = _StartPosition;
    }

    protected override void Update()
    {
        base.Update();
        if (Player.ThisPlayer.gameObject.transform.position.x >= Level.ThisLevel.XIntermediatePosition &&
            Player.ThisPlayer.NOfCannonball <= 0 &&
            !_CanShoot &&
            _NOfCannonball > 0 &&
            _Health > 0f)
        {
            if (_FirstShootCharge < _TimerBeforeFirstShoot)
            {
                _FirstShootCharge += Time.deltaTime;
            }
            else
            {
                _ShootRecharge += Time.deltaTime;
                if (_ShootRecharge > _ShootCD)
                {
                    _CanShoot = true;
                    _ShootRecharge = 0f;
                }

            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Player.ThisPlayer.gameObject.transform.position.x >= Level.ThisLevel.XIntermediatePosition && Player.ThisPlayer.NOfCannonball <= 0)
        {
            ShootCannonball();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject.GetComponent<Player>())
        {
            _CannonActiveToShoot = _Cannons[-Level.ThisLevel.ActualLayer];
            Level.ThisLevel.IsInBossBattle = true;
            IsStopped = true;
        }
    }

    private void SetCannons()
    {
        for (int i = 0; i < Level.ThisLevel.NOfLayersUnderWater; i++)
        {
            _CannonSpot = new Vector3(gameObject.transform.position.x, (Level.ThisLevel.UnitSpaceBetweenLayer * (-i)) + _CannonHeight, 0f);
            GameObject Cannon = Instantiate(_CannonPrefab, _CannonSpot, Quaternion.Euler(0f, 0f, 100f), gameObject.transform);
            _Cannons.Add(Cannon);
        }
    }
    private void ShootCannonball()
    {
        if (_CanShoot)
        {
            _CanShoot = false;
            _NOfCannonball--;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab, _CannonActiveToShoot.transform.position, Quaternion.identity);
            _LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(180f - _TrajectoryAngle, _ShootSpeed, _MaxDistance, _CannonballDamage);
        }
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;

        if (_Health <= 0)
        {
            StartCoroutine(DeadAnimation());
        }
    }

    private IEnumerator DeadAnimation()
    {
        yield return new WaitForSeconds(_DespawnTimer);

        Level.ThisLevel.IsLevelEnded = true;

        gameObject.SetActive(false);
    }

}
