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
    [SerializeField] private AudioClip _SoundOnTouch;
    [SerializeField] private ParticleSystem _DeathAnimationVFX;
    [SerializeField] private ParticleSystem _TrailVFX;
    [SerializeField] private List<GameObject> _ObjectsToHideOnDeath;

    [Header("Labels")]
    private UILabel _HealthLabel;
    public UILabel.LabelIconStyles LabelStyle;

    protected override void Start()
    {
        base.Start();
        _HealthLabel = GameManager.Instance.UIManager.CreateUILabel();
        _HealthLabel.ShowLabel(LabelStyle, _Health.ToString(), new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z), transform);
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
        _HealthLabel.SetText(_Health.ToString());


        if (_Health <= 0)
        {
            Destroy(_HealthLabel.gameObject);
            DropLoot();
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(DeathAnimation());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (gameObject.GetComponent<StealAbility>() == null)
            {
                GameManager.Instance.AudioManager.PlaySFX(_SoundOnTouch);
            }
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
}
