//EnemyShip.cs
//by MAURIZIO FISCHETTI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static T4P;

public class EnemyShip : LevelEntity, IDamageable
{

    [Header("Enemy Ship")]
    [SerializeField] private int _Health = 1;
    [SerializeField] private int _CollisionDamage = 1;

    [Header("Shoots")]
    public GameObject ProjectilePrefab;
    [SerializeField] private bool _AlwaysShoot = false;
    [SerializeField] private float _ShootRange = 3.0f;
    [SerializeField] private float _ShootTime = 2.0f;
    [SerializeField] private float _ShootDelay = 0.3f;
    [SerializeField] private int _CannonballDamage = 1;
    [SerializeField] private float _CannonballSpeed = 3f;
    [SerializeField] private float _MaxDistance =3.0f;
    [SerializeField][Range(0, 90)] private float _TrajectoryAngle = 15f;

    private bool _IsPlayerInRange = false;
    private bool _IsShooting = false;
    private bool _CanShoot = false;
    private float _TimeToShoot = 0;

    protected override void Start()
    {
        base.Start();
        _TimeToShoot = _ShootTime;
    }
    protected override void Update()
    {
        base.Update();

        _IsPlayerInRange = false;
        if (Physics.Raycast(new Ray(transform.position, Vector3.left), out RaycastHit hit, _ShootRange))
        {
            if (hit.collider != null)
            {
                if (hit.collider.attachedRigidbody.gameObject.name == "Player")
                {
                    _IsPlayerInRange = true;
                }
            }
      
        }
        

        if(!_CanShoot)
        {
            _TimeToShoot -= Time.deltaTime;
            if(_TimeToShoot <= 0)
            {
                _CanShoot=true;
            }
        }
        else
        {
            if (!_AlwaysShoot)
            {
                if (_IsPlayerInRange && !_IsShooting)
                {
                    _IsShooting = true;
                    StartCoroutine(Shoot(_ShootDelay));
                }
            }
            else
            {
                if(!_IsShooting)
                {
                    _IsShooting = true;
                    StartCoroutine(Shoot(_ShootDelay));
                }
            }
        }
    }


    private IEnumerator Shoot(float delay)
    {
        yield return new WaitForSeconds(delay);

        //shoot func here
        GameObject projetile = Instantiate(ProjectilePrefab, gameObject.transform.Find("Cannon").transform.position, Quaternion.identity);
        projetile.GetComponent<Cannonball>().ShootCannonball(-_TrajectoryAngle, -_CannonballSpeed, _MaxDistance, _CannonballDamage);

        _TimeToShoot = _ShootTime;
        _CanShoot = false;
        _IsShooting = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        //do damage to the player and deactivate the collider
        if (other.gameObject.GetComponent<Player>() != null)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(_CollisionDamage, gameObject);
            gameObject.GetComponent<Collider>().enabled = false;
            RB.isKinematic = true;
        }
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;
        if(_Health <= 0)
        {
            //tmp
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - _ShootRange, transform.position.y, transform.position.z));
    }
}
