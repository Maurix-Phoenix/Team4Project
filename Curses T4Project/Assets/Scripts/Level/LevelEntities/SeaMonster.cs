//SeaMonster.cs
//by MAURIZIO FISCHETTI and ANTHONY FEDELI

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMonster : LevelEntity, IDamageable
{
    [Header("Sea Monster Stat")]
    [SerializeField] private int _Health = 1;
    [SerializeField] private bool _IsDead = false;

    [Header("Sea Monster Type")]
    [SerializeField] private bool _IsSharkPack = false;
    [SerializeField] private bool _IsOctopus = false;
    [SerializeField] private bool _CanBite = false;
    private bool _SharkPackCanAttack = false;

    [Header("Reference")]
    [SerializeField] private AudioClip _BiteSFX;
    [SerializeField] private AudioClip _HitSFX;
    [SerializeField] private ParticleSystem _DeathAnimationVFX;
    [SerializeField] private List<GameObject> _ObjectsToHideOnDeath;

    public bool IsSharkPack { get { return _IsSharkPack; } }
    public bool IsOctopus { get { return _IsOctopus; } }
    public bool IsDead { get { return _IsDead; } }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;

        if (_Health <= 0)
        {
            DropLoot();
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        _IsDead = true;
        IsStopped = true;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (_CanBite)
            {
                if (_IsSharkPack && !_SharkPackCanAttack)
                {
                    _SharkPackCanAttack = true;
                    return;
                }
                GameManager.Instance.AudioManager.PlaySFX(_BiteSFX);
            }
            else
            {
                if (IsOctopus)
                {
                    return;
                }
                GameManager.Instance.AudioManager.PlaySFX(_HitSFX);
            }

        }
    }
}
