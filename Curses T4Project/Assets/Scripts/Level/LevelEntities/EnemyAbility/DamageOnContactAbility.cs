//DamageOnContactAbility.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// DamageOnContactAbility.cs give to the gameobject the ability of do damage when enter in contact with the player.
/// </summary>

public class DamageOnContactAbility : MonoBehaviour
{
    [SerializeField] private int _CollisionDamage = 1;

    [Header("Shark Pack - Ability")]
    [SerializeField] private bool _CanAttack = true;
    [SerializeField] private bool _CanInstantKill = false;

    private void OnTriggerEnter(Collider other)
    {
        //do damage to the player and deactivate the collider
        if (other.gameObject.GetComponent<Player>() != null)
        {
            if (gameObject.GetComponent<SeaMonster>() != null)
            {
                if (gameObject.GetComponent<SeaMonster>().IsSharkPack)
                {
                    if (_CanAttack)
                    {
                        _CanAttack = false;
                        return;
                    }

                    //end level or do damage
                    if (_CanInstantKill)
                    {
                        GameManager.Instance.LevelManager.CurrentLevel.EndLevel(Level.EndLevelType.GameOver);
                    }
                    else
                    {
                        other.gameObject.GetComponent<Player>().TakeDamage(_CollisionDamage, gameObject);
                    }
                    return;
                }
                if (gameObject.GetComponent<SeaMonster>().IsOctopus)
                {
                    return;
                }
            }

            other.gameObject.GetComponent<Player>().TakeDamage(_CollisionDamage, gameObject);
        }
    }
}
