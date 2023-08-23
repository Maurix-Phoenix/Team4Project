using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static T4P;

public class SlowAbility : MonoBehaviour
{
    [SerializeField][Range(0f, 99f)] private float _MovementReductionPercentage;
    [SerializeField] private float _DebuffDuration;
    [SerializeField] private bool _HasSlowed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Player>() != null && !_HasSlowed)
        {
            _HasSlowed = true;
            float speedMultiplier = 1f - (_MovementReductionPercentage / 100);
            other.gameObject.GetComponentInParent<PlayerMovement>().CoralHitted(_DebuffDuration, speedMultiplier, _HasSlowed);
            T4Debug.Log($"{gameObject.name} has reduced the changing layer speed {other.gameObject.GetComponentInParent<Player>().name} by {_MovementReductionPercentage}%.");
        }
    }
}
