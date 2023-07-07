//PlayerShoot.cs
//by ANTHONY FEDELI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]

/// <summary>
/// PlayerShoot.cs manages the ability of the ship to shoot a cannonball
/// </summary>

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _InputShootReference;
    [SerializeField] private GameObject _CannonballPrefab;
    [SerializeField] private Transform _CannonLocation;

    [Header("Cannonball Variables")]
    [SerializeField] private int _CannonballDamage = 1;
    [SerializeField] private float _CannonballSpeed = 10f;
    [SerializeField] private float _MaxDistance = 10f;
    [SerializeField][Range(0, 90)] private float _TrajectoryAngle = 20f;

    [Header("Shoot Variables")]
    [SerializeField] private int _NOfCannonball;
    [SerializeField] private bool _CanShoot = true;
    [SerializeField] private float _ShootCD = 1f;
    private float _ShootingRecharge = 0f;

    private PlayerMovement _PlayerMovement;
    private bool _IsShooting;

    private void Awake()
    {
        _PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _NOfCannonball = Level.ThisLevel.StartingCannonBalls;
    }

    private void FixedUpdate()
    {
        if (_IsShooting)
        {
            _IsShooting = false;
            _CanShoot = false;
            _ShootingRecharge = 0f;
            _NOfCannonball--;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab);
            _LaunchedCannondBall.transform.position = gameObject.transform.position + _CannonLocation.localPosition;
            _LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(_TrajectoryAngle, _CannonballSpeed, _MaxDistance, _CannonballDamage);
        }
    }

    private void Update()
    {
        if (!_CanShoot)
        {
            _ShootingRecharge += Time.deltaTime;
            if (_ShootingRecharge > _ShootCD)
            {
                _CanShoot = true;
            }
        }
    }

    private void OnShootInput()
    {
        if (!_PlayerMovement.IsChangingLayer && !_PlayerMovement.IsInAnimation && _NOfCannonball > 0 && _CanShoot)
        {
            _IsShooting = true;
        }
    }
}
