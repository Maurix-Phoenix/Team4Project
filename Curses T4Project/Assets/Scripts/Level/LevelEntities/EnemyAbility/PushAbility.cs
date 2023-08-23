//PushAbility.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// PushAbility.cs give to the gameobject the ability of push the player to another lane when enter in collision with it.
/// </summary>

public class PushAbility : MonoBehaviour
{
    [SerializeField] private int _Direction = 0;
    [SerializeField] private bool _HasPushed = false;

    private void OnTriggerEnter(Collider other)
    {
        _Direction = 0;
        if (other.gameObject.GetComponentInParent<Player>() != null && !_HasPushed)
        {
            _HasPushed = true;
            if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer >= -1)
            {
                _Direction = 2 * Random.Range(0, 2) - 1;
                Debug.Log(_Direction);
            }
            else
            {
                _Direction = 1;
            }
            other.gameObject.GetComponentInParent<PlayerMovement>().PushedAway(_Direction);
        }
    }
}
