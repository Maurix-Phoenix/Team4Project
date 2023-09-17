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
    [SerializeField] private ParticleSystem _TrailVFX;

    [Header("Labels")]
    private UILabel _HealthLabel;
    public UILabel.LabelIconStyles LabelStyle;

    public bool IsSharkPack { get { return _IsSharkPack; } }
    public bool IsOctopus { get { return _IsOctopus; } }
    public bool IsDead { get { return _IsDead; } }

    protected override void Start()
    {
        base.Start();

        //Health Label
        if(_Health > 0 && !_IsSharkPack)
        {
            _HealthLabel = GameManager.Instance.UIManager.CreateUILabel();
            _HealthLabel.ShowLabel(LabelStyle, _Health.ToString(), new Vector3(transform.position.x,transform.position.y -0.8f,transform.position.z + 0), transform);
        }
    }

    protected override void Update()
    {
        base.Update();

        if(_HealthLabel != null)
        {
            _HealthLabel.SetPosition(new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z));
        }

    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;
        _HealthLabel.SetText($"{_Health}");

        if (_Health <= 0)
        {
            Destroy(_HealthLabel.gameObject);
            DropLoot();
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        _IsDead = true;
        IsStopped = true;
        _TrailVFX.Stop();

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
