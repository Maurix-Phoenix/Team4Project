using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCannonball;

public class PlayerCannonball : MonoBehaviour
{
    public enum CannonballType
    {
        Normal,
        Physics
    }

    public CannonballType cannoballType;
    [Header("General Cannonball Stats")]
    [SerializeField] private int _cannonballDamage = 1;
    [SerializeField] private float _lifeTime = 3f;

    [Header("Normal Cannonball Stats")]
    [SerializeField] private float _normalCannonballSpeed = 15f;

    [Header("Physics Cannonball Stats")]
    public float _physicsCannonballSpeed = 17.5f;
    public float _physicsGravityScale = 1.0f;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        SetDestroyTime();

        //set velocity based on cannonball type
        InitializeCannonballStats();
    }

    private void FixedUpdate()
    {
        if (cannoballType == CannonballType.Normal)
        {
            _rb.AddForce(Vector3.zero, ForceMode.Acceleration);
        }
        else if (cannoballType == CannonballType.Physics)
        {
            Vector3 gravity = -9.81f * _physicsGravityScale * Vector3.up;
            _rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    private void InitializeCannonballStats()
    {
        if (cannoballType == CannonballType.Normal)
        {
            SetStraightVelocity();
        }
        else if (cannoballType == CannonballType.Physics)
        {
            SetPhysicsVelocity();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            IDamageable damageable;
            if (collision.gameObject.TryGetComponent<IDamageable>(out damageable))
            {
                damageable.TakeDamage(_cannonballDamage, gameObject);
            }
        }
        Destroy(gameObject);
    }


    private void SetDestroyTime()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void SetStraightVelocity()
    {
        _rb.velocity = transform.right * _normalCannonballSpeed;
    }

    private void SetPhysicsVelocity()
    {
        _rb.velocity = transform.right * _physicsCannonballSpeed;
    }

}
