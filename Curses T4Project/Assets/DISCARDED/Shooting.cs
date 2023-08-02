using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _InputShootReference;
    public GameObject _CannonballPrefab;
    [SerializeField] private Transform _CannonLocation;

    [Header("Shoot Variables")]
    [SerializeField] private float _ShootCD = 1f;
    [SerializeField] private bool _isShooting = false;
    [SerializeField] private bool _canShoot = true;
    private float _ShootingRecharge = 0f;


    private void FixedUpdate()
    {
        if (_isShooting)
        {
            _isShooting = false;
            GameObject _LaunchedCannondBall = Instantiate(_CannonballPrefab, _CannonLocation.transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (!_canShoot && !_isShooting)
        {
            _ShootingRecharge += Time.deltaTime;
            if (_ShootingRecharge > _ShootCD)
            {
                _ShootingRecharge = 0f;
                _canShoot = true;
            }
        }
    }

    private void OnShootInput(InputValue _value)
    {
        if (_value.isPressed && _canShoot)
        {
            _canShoot = !_value.isPressed;
            _isShooting = _value.isPressed;
        }
    }


}
