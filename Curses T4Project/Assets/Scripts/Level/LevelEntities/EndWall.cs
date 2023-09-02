//EndWall.cs
//by ANTHONY FEDELI


using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// EndWall.cs manages the behaviour of the wall at the end of the level.
/// </summary>

public class EndWall : LevelEntity, IDamageable
{
    [Header("References")]
    [SerializeField] private GameObject _CannonballPrefab;
    [SerializeField] private GameObject[] _CannonPrefab = new GameObject[3]; 
    [SerializeField] private Transform _FirePos; 
    [SerializeField] private BoxCollider _TriggerToMovePlayer;
    [SerializeField] private GameObject _EndWallUI;
    [SerializeField] private TMP_Text _HealthUI;

    [Header("EndWall Variables")]
    [SerializeField] private Vector3 _TriggerPosition; 
    [SerializeField] private Vector3 _TriggerDimension; 
    [SerializeField] private int _Health = 10;
    private int _NOfCannonball = 1;

    [Header("Cannons Variables")]
    [SerializeField] private bool _CanShoot = false;
    [SerializeField] private bool _EndWallIsPositionated = false;
    [SerializeField] private GameObject _CannonActiveToShoot;

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

    public GameObject CannonActiveToShoot { get { return _CannonActiveToShoot; } }
    public int Health { get { return _Health; } }
    public Transform FirePos { get { return _FirePos; } }

    private void Awake()
    {
        InitializeRB();
        InitializeTrigger();
    }

    protected override void Update()
    {
        base.Update();
        if (GameManager.Instance.LevelManager.Player.gameObject.transform.position.x >= GameManager.Instance.LevelManager.CurrentLevel.XIntermediatePosition &&
            GameManager.Instance.LevelManager.Player.NOfCannonball <= 0 &&
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
        if (GameManager.Instance.LevelManager.Player.gameObject.transform.position.x >= GameManager.Instance.LevelManager.CurrentLevel.XIntermediatePosition && GameManager.Instance.LevelManager.Player.NOfCannonball <= 0)
        {
            ShootCannonball();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_EndWallIsPositionated)
        {
            _EndWallIsPositionated = true;
            _NOfCannonball = other.gameObject.GetComponent<Player>().Health;
            _TriggerToMovePlayer.gameObject.SetActive(false);
            _CannonActiveToShoot = _CannonPrefab[Mathf.Abs(GameManager.Instance.LevelManager.CurrentLevel.ActualLayer)];
            _FirePos = _CannonActiveToShoot.transform.Find("FirePos").gameObject.transform;
            GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle = true;
            IsStopped = true;
            //_Rb.isKinematic = true;

            //active endwall
            _EndWallUI.SetActive(true);
            _HealthUI.text = Health.ToString();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCannonball") && _EndWallIsPositionated)
        {
            TakeDamage(other.gameObject.GetComponent<Cannonball>().CannonballDamage, other.gameObject);
        }

    }

    private void OnValidate()
    {
        InitializeTrigger();
    }

    private void InitializeRB()
    {
        //Initialize the Rigidbody component
        _Rb = GetComponent<Rigidbody>();
        _Rb.useGravity = false;
        _Rb.interpolation = RigidbodyInterpolation.Interpolate;
        _Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void InitializeTrigger()
    {
        _TriggerToMovePlayer = gameObject.transform.Find("Trigger").GetComponent<BoxCollider>();
        _TriggerToMovePlayer.size = _TriggerDimension;
        _TriggerToMovePlayer.center = new Vector3(- _TriggerDimension.x / 2, _TriggerPosition.y, _TriggerPosition.z);
    }

    private void ShootCannonball()
    {
        if (_CanShoot)
        {
            _CanShoot = false;
            _NOfCannonball--;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab, _FirePos.position, Quaternion.identity);
            _LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(180f - _TrajectoryAngle, _ShootSpeed, _MaxDistance, _CannonballDamage);
        }
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;

        if(_Health>=0)
        {
            _HealthUI.text = _Health.ToString();
        }

        if (_Health <= 0)
        {
            DropLoot();
            StartCoroutine(DeadAnimation());
        }
    }

    private IEnumerator DeadAnimation()
    {
        yield return new WaitForSeconds(_DespawnTimer);

        GameManager.Instance.LevelManager.CurrentLevel.IsLevelEnded = true;

        gameObject.SetActive(false);
    }

}
