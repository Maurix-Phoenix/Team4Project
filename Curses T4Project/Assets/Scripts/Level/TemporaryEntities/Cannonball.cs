//CannonBall.cs
//by ANTHONY FEDELI

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

/// <summary>
/// Cannonball.cs manages the behaviour of the cannonball
/// </summary>
public class Cannonball : LevelEntityTemporary
{
    [Header("SFX e VFX")]
    [SerializeField] private AudioClip _AboveWaterSFX;
    [SerializeField] private AudioClip _UnderWaterSFX;
    [SerializeField] private AudioClip _ExplosionUpSFX;
    [SerializeField] private AudioClip _ExplosionDownSFX;
    [SerializeField] private ParticleSystem _CannonBallExplosion;

    private int _CannonballDamage = 1;
    private float _CannonballSpeed = 1f;
    private float _MaxDistance = 0f;
    private float _TrajectoryX = 0f;
    private float _TrajectoryY = 0f;
    private Vector3 _Trajectory = Vector3.zero;
    private Vector3 _StartLocation= Vector3.zero;

    private Vector3 _TargetLocation;

    private Rigidbody _Rb;
    private Player _Player; //MAU
    private EndWall _EndWall; //MAU
    public int CannonballDamage { get { return _CannonballDamage; } }

    private void Awake()
    {
        //initialize the rigidbody on the cannonball
        _Rb = GetComponent<Rigidbody>();
        _Rb.freezeRotation = true;
        _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _Rb.interpolation = RigidbodyInterpolation.Interpolate;
        _Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        _Player = GameManager.Instance.LevelManager.Player; //MAU

        if (FindObjectOfType<EndWall>() != null)
        {
            _EndWall = FindObjectOfType<EndWall>(); //MAU
        }

        _StartLocation = gameObject.transform.position;
    }

    protected override void Start()
    {
        base.Start();
        if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer >= 0)
        {
            GameManager.Instance.AudioManager.PlaySFX(_AboveWaterSFX);
        }
        else
        {
            GameManager.Instance.AudioManager.PlaySFX(_UnderWaterSFX);
        }

        if (gameObject.layer == LayerMask.NameToLayer("EnemyCannonball"))
        {
            _TargetLocation = (_Player.transform.position - _StartLocation).normalized;
            T4P.T4Debug.Log("EnemyCannonball " + _TargetLocation);

        }
        else if (gameObject.layer == LayerMask.NameToLayer("PlayerCannonball"))
        {
            if(!GameManager.Instance.LevelManager.CurrentLevel.IsFinalArrivalBeach)
            {
                _TargetLocation = (_EndWall.transform.position - _StartLocation).normalized;
            }

            if (!GameManager.Instance.LevelManager.CurrentLevel.PlayerHasReachBeach)
            {
                if (GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle) //MAU - getting sure to get the active cannon position only in bossbattle.
                {
                    _Rb.constraints = RigidbodyConstraints.FreezeRotation;
                    _TargetLocation = (_EndWall.CannonActiveToShoot.transform.Find("FirePos").transform.position - _StartLocation).normalized;
                }
            }

            T4P.T4Debug.Log("PlayerCannonball " + _TargetLocation);
        }
    }

    protected override void Update()
    {
        base.Update();

        //If level is not in boss battle, destroy the projectile after reaching the max distance.
        if(!GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
        {
            _Rb.useGravity = true;
            if (gameObject.layer == LayerMask.NameToLayer("EnemyCannonball") ||
                    transform.position.y < _StartLocation.y - 1)
            {
                if (Vector3.Distance(transform.position, _StartLocation) > _MaxDistance)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, _StartLocation) > _MaxDistance ||
                    transform.position.y < _StartLocation.y - GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer / 2 ||
                    transform.position.y > _StartLocation.y + GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer / 2)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            _Rb.useGravity = false;
        }
    }

    private void FixedUpdate()
    {
        //moving the cannonball
        if (GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
        {
            if (gameObject.layer == LayerMask.NameToLayer("EnemyCannonball"))
            {
                _TargetLocation = (_Player.transform.position - _StartLocation).normalized;
            }
            
            // _Rb.useGravity = false;
            _Rb.MovePosition(transform.position + _TargetLocation * _CannonballSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Set the value needed before the shooting
    /// </summary>
    /// <param name="TrajectoryAngle">the angle of the trajectory</param>
    /// <param name="CannonballSpeed">the speed of the shooting</param>
    /// <param name="MaxDistance">max distance before the cannonball explode</param>
    /// <param name="CannonballDamage">max damage that the cannonbal can do</param>
    /// <returns></returns>

    public void ShootCannonball(float TrajectoryAngle, float CannonballSpeed, float MaxDistance, int CannonballDamage)
    {
        if (GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
        {
            _CannonballSpeed = CannonballSpeed;
            _CannonballDamage = CannonballDamage;
        }
        else
        {
            //set the variable needed for the cannonbal behaviour
            _MaxDistance = MaxDistance;
            _CannonballDamage = CannonballDamage;
            _TrajectoryX = Mathf.Cos(TrajectoryAngle * Mathf.Deg2Rad);
            _TrajectoryY = Mathf.Sin(TrajectoryAngle * Mathf.Deg2Rad);
            _Trajectory = (new Vector3 (_TrajectoryX, _TrajectoryY, 0f)) * CannonballSpeed;

            //shoot the cannonball
            _Rb.AddForce(_Trajectory, ForceMode.VelocityChange);
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        //MAU
        //check if the collision are on different layers
        if (collision.gameObject.layer != gameObject.layer)
        {
            IDamageable damageable;
            Instantiate(_CannonBallExplosion, transform.position, Quaternion.identity);
            ExplosionSound();

            if (collision.gameObject.TryGetComponent<IDamageable>(out damageable))
            {
                damageable.TakeDamage(_CannonballDamage, gameObject);
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Mine"))
        {
            //check if the collision are on different layers
            if (collision.gameObject.layer != gameObject.layer)
            {
                IDamageable damageable;
                if (collision.gameObject.TryGetComponent<IDamageable>(out damageable))
                {
                    ExplosionSound();
                    damageable.TakeDamage(_CannonballDamage, gameObject);
                }
            }
            Destroy(gameObject);
        }
    }

    private void ExplosionSound()
    {
        if (gameObject.transform.position.y < 0)
        {
            GameManager.Instance.AudioManager.PlaySFX(_ExplosionDownSFX);
        }
        else
        {
            GameManager.Instance.AudioManager.PlaySFX(_ExplosionUpSFX);
        }
    }
}
