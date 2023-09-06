//EnemyShip.cs
//by MAURIZIO FISCHETTI & ANTHONY FEDELI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemyShip.cs manages the behaviour of the Ship, the variables and conditions.
/// </summary>

public class EnemyShip : LevelEntity, IDamageable
{
    [Header("Enemy Ship Stats")]
    [SerializeField] private int _Health = 1;
    [SerializeField] private bool _IsDead = false;

    public bool IsDead { get { return _IsDead; } }

    [Header("SFX & VFX")]
    [SerializeField] private AudioClip _Destruction;
    [SerializeField] private ParticleSystem _DeathAnimationVFX;
    [SerializeField] private List<GameObject> _ObjectsToHideOnDeath;

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;

        if(_Health <= 0)
        {
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        _IsDead = true;

        GetComponent<BoxCollider>().enabled = false;

        if (_IsDead)
        {
            _DeathAnimationVFX.Play();

            while (_DeathAnimationVFX.isPlaying)
            {
                for (int i = 0; i < _ObjectsToHideOnDeath.Count; i++)
                {
                    _ObjectsToHideOnDeath[i].SetActive(false);
                }

                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
