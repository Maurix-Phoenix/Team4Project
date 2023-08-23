//UpAndDownAbility.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// UpAndDownAbility.cs give to the gameobject the ability of move up and down between lane.
/// </summary>

public class UpAndDownAbility : MonoBehaviour
{
    [SerializeField] private float _Amplitude;
    [SerializeField] private float _Frequency;

    private Rigidbody _Rigidbody;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float floatingSpeed = _Amplitude * Mathf.Cos(_Frequency * Time.time);
        _Rigidbody.MovePosition(_Rigidbody.position + Vector3.up * floatingSpeed * Time.fixedDeltaTime);
    }
}
