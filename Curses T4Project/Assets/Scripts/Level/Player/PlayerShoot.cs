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

    [Header("Shoot Variables")]
    [Tooltip("Base Value 10")][SerializeField] private float _MaxDistance = 10f;
    [Tooltip("Base Value 20")][Range(0, 90)][SerializeField] private float _TrajectoryAngle = 20f;
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
    }

    private void FixedUpdate()
    {
        if (Player.ThisPlayer.IsShooting)
        {
            Player.ThisPlayer.IsShooting = false;
            Player.ThisPlayer.CanShoot = false;
            _ShootingRecharge = 0f;
            Player.ThisPlayer.NOfCannonball--;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab, gameObject.transform.position + _CannonLocation.localPosition, Quaternion.identity);
            _LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(_TrajectoryAngle, _CannonballSpeed, _MaxDistance, _CannonballDamage);
        }

        if (gameObject.transform.position.x >= Level.ThisLevel.XIntermediatePosition && Player.ThisPlayer.NOfCannonball > 0 && Player.ThisPlayer.CanShoot)
        {
            Player.ThisPlayer.CanShoot = false;
            _ShootingRecharge = 0f;
            Player.ThisPlayer.NOfCannonball--;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab, gameObject.transform.position + _CannonLocation.localPosition, Quaternion.identity);
            _LaunchedCannondBall.GetComponent<Cannonball>().ShootCannonball(_TrajectoryAngle, _CannonballSpeed, _MaxDistance, _CannonballDamage);
        }
    }

    private void Update()
    {
        if (Level.ThisLevel.IsInBossBattle)
        {
            _ShootCD = _ShootAtBossCD;
        }

        if (!Player.ThisPlayer.CanShoot && !Player.ThisPlayer.IsInStartAnimation)
        {
            _ShootingRecharge += Time.deltaTime;
            if (_ShootingRecharge > _ShootCD)
            {
                Player.ThisPlayer.CanShoot = true;
            }
        }
    }

    private void OnShootInput()
    {
        if (!Player.ThisPlayer.IsChangingLayer && !Player.ThisPlayer.IsInStartAnimation && Player.ThisPlayer.NOfCannonball > 0 && Player.ThisPlayer.CanShoot && !Level.ThisLevel.IsInBossBattle)
        {
            Player.ThisPlayer.IsShooting = true;
        }
    }
}
