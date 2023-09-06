//ShootAbility.cs
//by ANTHONY FEDELI

using System.Collections;
using UnityEngine;

/// <summary>
/// ShootAbility.cs give to the gameobject the ability of shoot a projectile.
/// </summary>

public class ShootAbility : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private bool _AlwaysShoot = false;
    [SerializeField] private float _ShootRange = 3.0f;
    [SerializeField] private bool _ShowShootRange = true;
    [SerializeField] private float _TimeBetweenShoot = 2.0f;
    [SerializeField] private int _CannonballDamage = 1;
    [SerializeField] private float _CannonballSpeed = 3f;
    [SerializeField] private float _MaxDistance = 3.0f;
    [SerializeField][Range(0, 90)] private float _TrajectoryAngle = 15f;

    private bool _IsPlayerInRange = false;
    private bool _IsShooting = false;
    private bool _CanShoot = false;
    private float _TimeToShoot = 0;

    private void Start()
    {
        _TimeToShoot = _TimeBetweenShoot;
    }

    private void Update()
    {
        if (!gameObject.GetComponent<EnemyShip>().IsStopped && !gameObject.GetComponent<EnemyShip>().IsDead && (gameObject.transform.position.x < 15 && gameObject.transform.position.x > 0))
        {
            _IsPlayerInRange = false;
            if (Physics.Raycast(new Ray(transform.position, Vector3.left), out RaycastHit hit, _ShootRange, LayerMask.GetMask("Player")))
            {

                _IsPlayerInRange = true;
            }

            if (!_CanShoot)
            {
                _TimeToShoot -= Time.deltaTime;
                if (_TimeToShoot <= 0)
                {
                    _CanShoot = true;
                }
            }
            else
            {
                if (!_AlwaysShoot)
                {
                    if (_IsPlayerInRange && !_IsShooting)
                    {
                        _IsShooting = true;
                        StartCoroutine(Shoot());
                    }
                }
                else
                {
                    if (!_IsShooting)
                    {
                        _IsShooting = true;
                        StartCoroutine(Shoot());
                    }
                }
            }
        }
    }

    private IEnumerator Shoot()
    {

        GameObject projectile = Instantiate(ProjectilePrefab, gameObject.transform.Find("Cannon").transform.position, Quaternion.identity);
        projectile.GetComponent<Cannonball>().ShootCannonball(-_TrajectoryAngle, -_CannonballSpeed, _MaxDistance, _CannonballDamage);

        _TimeToShoot = _TimeBetweenShoot;
        _CanShoot = false;
        _IsShooting = false;

        yield return null;
    }

    private void OnDrawGizmos()
    {
        if (_ShowShootRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - _ShootRange, transform.position.y, transform.position.z));
        }
    }
}
