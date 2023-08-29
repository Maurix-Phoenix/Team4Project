//PushAbility.cs
//by ANTHONY FEDELI

using UnityEngine;
using static T4P;

/// <summary>
/// PushAbility.cs give to the gameobject the ability of push the player to another lane when enter in collision with it.
/// </summary>

public class PushAbility : MonoBehaviour
{
    public enum PushDirection
    {
        None,
        UpperLane,
        BottomLane
    }

    [SerializeField] private PushDirection PushTo;
    [SerializeField] private bool _HasPushed = false;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Player>() != null && !_HasPushed)
        {
            _HasPushed = true;

            switch (PushTo)
            {
                case PushDirection.None:
                    T4Debug.Log($"Lane to move the player to isn't selected in {this} of {gameObject.name}.");
                    break;
                case PushDirection.UpperLane:
                    if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer >= 0)
                    {
                        T4Debug.Log($"{gameObject.name} cant' push the player above the water.");
                    }
                    else
                    {
                        other.gameObject.GetComponentInParent<PlayerMovement>().PushAway(1f);
                    }
                    break;
                case PushDirection.BottomLane:
                    if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer <= - GameManager.Instance.LevelManager.CurrentLevel.NOfLayersUnderWater)
                    {
                        T4Debug.Log($"{gameObject.name} cant' push the player under the seabed.");
                    }
                    else
                    {
                        other.gameObject.GetComponentInParent<PlayerMovement>().PushAway(-1f);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
