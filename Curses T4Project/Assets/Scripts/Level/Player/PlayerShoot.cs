//PlayerShoot.cs
//by ANTHONY FEDELI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [Tooltip("Base Value 10")][SerializeField] private float _CannonballSpeed = 10f;
    [Tooltip("Above Water the modifier is equal to 1")][SerializeField] private float _UnderWaterSpeedModifier = 0.2f;
    private float _StartingUnderWaterSpeedModifier;

    [Header("Shoot Variables")]
    [Tooltip("Base Value 10")][SerializeField] private float _MaxDistance = 10f;
    [Tooltip("Base Value 20")][Range(0, 90)][SerializeField] private float _TrajectoryAngle = 20f;
    [Tooltip("Above Water the modifier is equal to 1")][SerializeField] private float _UnderWaterTrajectoryAngleModifier = 0.2f;
    private float _StartingUnderWaterTrajectoryAngleModifier;
    [SerializeField] private float _ShootCD = 1f;
    [SerializeField] private float _ShootAtBossCD = 0.2f;
    private float _ShootingRecharge = 0f;

    private PlayerMovement _PlayerMovement;

    private void Awake()
    {
        _PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _StartingUnderWaterSpeedModifier = _UnderWaterSpeedModifier;
        _StartingUnderWaterTrajectoryAngleModifier = _UnderWaterTrajectoryAngleModifier;

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.Player.IsShooting)
        {
            GameManager.Instance.Player.IsShooting = false;
            GameManager.Instance.Player.CanShoot = false;
            _ShootingRecharge = 0f;
            GameManager.Instance.Player.NOfCannonball--;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab, _CannonLocation.transform.position, Quaternion.identity);

            //MAU - i've corrected the underwater modifiers using a sum  (a - modifier) cause using this (a * modifier) was too exagerated because the values are near 0.
            _LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(_TrajectoryAngle - _UnderWaterTrajectoryAngleModifier, _CannonballSpeed - _UnderWaterSpeedModifier, _MaxDistance, _CannonballDamage);
            //_LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(_TrajectoryAngle * _UnderWaterTrajectoryAngleModifier, _CannonballSpeed * _UnderWaterSpeedModifier, _MaxDistance, _CannonballDamage);

        }

        if (gameObject.transform.position.x >= GameManager.Instance.Level.XIntermediatePosition && GameManager.Instance.Player.NOfCannonball > 0 && GameManager.Instance.Player.CanShoot)
        {
            GameManager.Instance.Player.CanShoot = false;
            _ShootingRecharge = 0f;
            GameManager.Instance.Player.NOfCannonball--;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab, _CannonLocation.transform.position, Quaternion.identity);
            _LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(0f, _CannonballSpeed, 0f , _CannonballDamage);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.Level.ActualLayer < 0)
        {
            _UnderWaterSpeedModifier = _StartingUnderWaterSpeedModifier;
            _UnderWaterTrajectoryAngleModifier = _StartingUnderWaterTrajectoryAngleModifier;
        }
        else
        {
            _UnderWaterSpeedModifier = 1f;
            _UnderWaterTrajectoryAngleModifier = 1f;
        }
        if (GameManager.Instance.Level.IsInBossBattle)
        {
            _ShootCD = _ShootAtBossCD;
        }

        if (!GameManager.Instance.Player.CanShoot && !GameManager.Instance.Player.IsInStartAnimation)
        {
            _ShootingRecharge += Time.deltaTime;
            if (_ShootingRecharge > _ShootCD)
            {
                GameManager.Instance.Player.CanShoot = true;
            }
        }
    }

    private void OnShootInput()
    {
        if (!GameManager.Instance.Player.IsChangingLayer && !GameManager.Instance.Player.IsInStartAnimation && GameManager.Instance.Player.NOfCannonball > 0 && GameManager.Instance.Player.CanShoot && !GameManager.Instance.Level.IsInBossBattle)
        {
            GameManager.Instance.Player.IsShooting = true;
        }
    }
}
