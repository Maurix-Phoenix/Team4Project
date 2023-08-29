//FloatingnAbility.cs
//by ANTHONY FEDELI

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// FloatingAbility.cs give to the gameobject the ability of move up and down between lane.
/// </summary>

public class FloatingAbility : MonoBehaviour
{
    [SerializeField] private bool _IsOnMenuScene = false;

    [Header("Floating Values")]
    [SerializeField] private float _Amplitude = 0.1f;
    [SerializeField] private float _Frequency = 5f;

    private float _UpAndDownTimer;
    private Rigidbody _RigidBody;

    private void Start()
    {
        _UpAndDownTimer = 0;
        _RigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_IsOnMenuScene)
        {
            if (!gameObject.GetComponent<LevelEntity>().IsStopped)
            {
                _UpAndDownTimer += Time.deltaTime;
            }
        }
        else
        {
            _UpAndDownTimer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        FloatBetweenLane();
    }

    private void FloatBetweenLane()
    {
        if (!_IsOnMenuScene)
        {
            if (!gameObject.GetComponent<LevelEntity>().IsStopped)
            {
                if (!gameObject.GetComponent<LevelEntity>().MoveToPlayer)
                {
                    if (_RigidBody.isKinematic)
                    {
                        float floatingSpeed = _Amplitude * _Frequency * (Mathf.Cos(_Frequency * _UpAndDownTimer));
                        Vector3 floatingMovement = Vector3.up * floatingSpeed;
                        Vector3 directionalMovement = Vector3.left * gameObject.GetComponent<LevelEntity>().MoveSpeed;
                        _RigidBody.MovePosition(_RigidBody.position + (floatingMovement + directionalMovement) * Time.fixedDeltaTime);
                    }
                    else
                    {
                        float floatingSpeed = _Amplitude * _Frequency * (Mathf.Cos(_Frequency * _UpAndDownTimer));
                        _RigidBody.MovePosition(_RigidBody.position + Vector3.up * floatingSpeed * Time.fixedDeltaTime);
                    }
                }
            }
        }
        else
        {
            float floatingSpeed = _Amplitude * _Frequency * (Mathf.Cos(_Frequency * _UpAndDownTimer));
            _RigidBody.MovePosition(_RigidBody.position + Vector3.up * floatingSpeed * Time.fixedDeltaTime);
        }
    }
}
