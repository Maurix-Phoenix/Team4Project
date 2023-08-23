//EnemyShip.cs
//by MAURIZIO FISCHETTI & ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// EnemyShip.cs manages the behaviour of the Ship, the variables and conditions.
/// </summary>

public class EnemyShip : LevelEntity, IDamageable
{
    [Header("Enemy Ship Stats")]
    [SerializeField] private int _Health = 1;

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;
        if(_Health <= 0)
        {
            DropLoot();
            //tmp
            gameObject.SetActive(false);
        }
    }
}
