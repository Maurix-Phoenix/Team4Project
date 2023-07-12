//CannonBall.cs
//by ANTHONY FEDELI

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

/// <summary>
/// Cannonball.cs manages the behaviour of the cannonball
/// </summary>
public class Cannonball : MonoBehaviour
{
    private int _CannonballDamage = 1;
    private float _MaxDistance = 0f;
    private float _TrajectoryX = 0f;
    private float _TrajectoryY = 0f;
    private Vector3 _Trajectory = Vector3.zero;
    private Vector3 _StartingShootLocation= Vector3.zero;

    private Rigidbody _Rb;

    private void Awake()
    {
        //initialize the rigidbody on the cannonball
        _Rb = GetComponent<Rigidbody>();
        _Rb.freezeRotation = true;
        _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _Rb.interpolation = RigidbodyInterpolation.Interpolate;
        _Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        _StartingShootLocation = gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        //destroy cannonball after reaching the max distance
        if (_Rb.velocity.x > 0)
        {
            if (gameObject.transform.position.x > _StartingShootLocation.x + _MaxDistance)
            {
                Destroy(gameObject);
            }
        }
        else if (_Rb.velocity.x < 0)
        {
            if (gameObject.transform.position.x < _StartingShootLocation.x - _MaxDistance)
            {
                Destroy(gameObject);
            }
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
        //set the variable needed for the cannonbal behaviour
        _MaxDistance = MaxDistance;
        _CannonballDamage = CannonballDamage;
        _TrajectoryX = Mathf.Cos(TrajectoryAngle * Mathf.Deg2Rad);
        _TrajectoryY = Mathf.Sin(TrajectoryAngle * Mathf.Deg2Rad);
        _Trajectory = (new Vector3 (_TrajectoryX, _TrajectoryY, 0f)) * CannonballSpeed;

        //shoot the cannonball
        _Rb.AddForce(_Trajectory, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDamageable>().TakeDamage(_CannonballDamage, gameObject);
        Destroy(gameObject);
    }
}
