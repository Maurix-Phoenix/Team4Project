using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private bool _IsShaking = false;
    [SerializeField] private float _MaxRangeShake = 0.2f;
    [SerializeField] private float _ShakeDuration = 0.5f;
    [SerializeField] private float _ShakeSpeed = 1f;

    private float _StartRangeShake;
    private float _StartShakeDuration;
    private Vector3 _StartPosition;

    private void Awake()
    {
        _IsShaking = false;
    }
    private void Start()
    {
        _StartPosition = transform.position;
        _StartRangeShake = _MaxRangeShake;
        _StartShakeDuration = _ShakeDuration;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (_IsShaking)
            {
                ShakeCamera();
            }
            else
            {
                gameObject.transform.position = _StartPosition;
                _MaxRangeShake = _StartRangeShake;
                _ShakeDuration = _StartShakeDuration;
            }
        }
    }

    public void StartShaking()
    {
        _IsShaking = true;
    }

    private void ShakeCamera()
    {
        if (_ShakeDuration > 0)
        {
            float angle = Random.Range(0f, 360f);
            float x = Mathf.Cos(angle) * _MaxRangeShake;
            float y = Mathf.Sin(angle) * _MaxRangeShake;
            Vector3 NextPosition = new Vector3(_StartPosition.x + x, _StartPosition.y + y, _StartPosition.z);
            Debug.Log("NextPosition");

            gameObject.transform.position = NextPosition;

            if (_MaxRangeShake >= 0f)
            {
                _MaxRangeShake -= _ShakeSpeed * Time.deltaTime;
            }

            _ShakeDuration -= Time.deltaTime;
        }
        else
        {
            _IsShaking = false;
        }
    }
}
