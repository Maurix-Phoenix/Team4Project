using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static T4P;

public class Treasure : LevelEntity, IDamageable
{
    [Header("Treasure Stats")]
    [SerializeField] private bool _IsOpened;
    [SerializeField] private AudioClip _OpenSFX;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        if(!_IsOpened)
        {
            _IsOpened = true;
            StartCoroutine(OpenChest());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_IsOpened && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _IsOpened = true;
            StartCoroutine(OpenChest());
        }
    }

    private IEnumerator OpenChest()
    {
        animator.SetTrigger("OpenChest");
        GameManager.Instance.AudioManager.PlaySFX(_OpenSFX);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        DropLoot(1.5f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
