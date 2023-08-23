//SeaMonster.cs
//by MAURIZIO FISCHETTI and ANTHONY FEDELI

using UnityEngine;

public class SeaMonster : LevelEntity, IDamageable
{
    [Header("Sea Monster")]
    [SerializeField] private int _Health = 1;
    [SerializeField] private bool _IsSharkPack = false;
    [SerializeField] private bool _IsOctopus = false;

    public bool IsSharkPack { get { return _IsSharkPack; } }
    public bool IsOctopus { get { return _IsOctopus; } }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;

        if(_Health <= 0)
        {
            DropLoot();
            //TMP
            gameObject.SetActive(false);
            //death animation (routine) here, after deactivate the object
        }
    }
}
