//DOTAbility.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// DOTAbility.cs give to the gameobject the ability of do damage over time when enter in contact with the player.
/// </summary>
public class DOTAbility : MonoBehaviour
{
    [Header("Reposition")]
    [SerializeField] private bool _TargetLocked = false;
    [SerializeField] private Transform _TargetPosition;
    [SerializeField] private float _RepositionSpeed = 1f;

    [Header("Attack parameters")]
    [SerializeField] private bool _InPositionToAttack = false;
    [SerializeField] private bool _DoDamage = false;
    [SerializeField] private float _BaseAttackCD;
    [SerializeField] private float _TimeRemainBetweenAttack = 2f;
    [SerializeField] private int _DotDamage = 1;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _BaseAttackCD = _TimeRemainBetweenAttack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            _TargetPosition = other.gameObject.GetComponent<PlayerShoot>().CannonLocation;
            gameObject.GetComponent<SeaMonster>().MoveSpeed = 0;
            _TargetLocked = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            _TargetPosition = other.gameObject.GetComponent<PlayerShoot>().CannonLocation;
            if (_DoDamage)
            {
                other.gameObject.GetComponent<Player>().TakeDamage(_DotDamage, gameObject);
                _DoDamage = false;
            }
        }
    }

    private void Update()
    {
        CheckPosition();
        StartAttack();
    }

    private void CheckPosition()
    {
        if (_TargetLocked && !_InPositionToAttack)
        {
            if (gameObject.transform.position != _TargetPosition.position)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, _TargetPosition.position, Time.deltaTime * _RepositionSpeed);
            }
            else
            {
                animator.SetBool("IsInPosition", true);
                _InPositionToAttack = true;
            }
        }

        if (_InPositionToAttack)
        {
            gameObject.transform.position = _TargetPosition.position;
        }
    }

    private void StartAttack()
    {
        if (_InPositionToAttack)
        {
            _TimeRemainBetweenAttack -= Time.deltaTime;
            if (_TimeRemainBetweenAttack < 0)
            {
                _TimeRemainBetweenAttack = _BaseAttackCD;
                _DoDamage = true;
                animator.SetTrigger("Attack");
            }
        }
    }
}
