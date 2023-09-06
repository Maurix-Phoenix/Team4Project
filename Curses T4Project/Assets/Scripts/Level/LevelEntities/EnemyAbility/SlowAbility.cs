//SlowAbility.cs
//by ANTHONY FEDELI

using UnityEngine;
using static T4P;

/// <summary>
/// SlowAbility.cs give to the gameobject the ability of reduce the changing layer speed of the playe when enter in contact with it.
/// </summary>

public class SlowAbility : MonoBehaviour
{
    [Header("Debuff")]
    [SerializeField][Range(0f, 99f)] private float _MovementReductionPercentage;
    [SerializeField] private float _DebuffDuration;
    [SerializeField] private bool _HasSlowed;

    [Header("SFX")]
    [SerializeField] private AudioClip _SlowEffectSound;

    private void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Player>() != null && !other.gameObject.GetComponentInParent<Player>().IsChangingLayer && !_HasSlowed)
        {
            GameManager.Instance.AudioManager.PlaySFX(_SlowEffectSound);
            _HasSlowed = true;
            float speedMultiplier = 1f - (_MovementReductionPercentage / 100);
            other.gameObject.GetComponentInParent<PlayerMovement>().CoralHitted(_DebuffDuration, speedMultiplier, _HasSlowed);
            T4Debug.Log($"{gameObject.name} has reduced the changing layer speed {other.gameObject.GetComponentInParent<Player>().name} by {_MovementReductionPercentage}%.");
        }
    }
}
