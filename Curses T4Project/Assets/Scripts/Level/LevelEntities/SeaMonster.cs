//SeaMonster.cs
//by MAURIZIO FISCHETTI

using UnityEngine;
using static T4P;

public class SeaMonster : LevelEntity, IDamageable
{
    [Header("Sea Monster")]
    [SerializeField] private int _Health = 1;
    [SerializeField] private int _Damage = 1;


    [Header("Aggro")]
    [SerializeField] private bool _AggroAtStart = false;
    [SerializeField] private float _AggroSpeedMultiplier = 2.0f;
    [SerializeField] private float _AggroRange = 3.0f;

    private bool _Aggroed = false;

    protected override void Start()
    {
        base.Start();

        if(_AggroAtStart)
        {
            _Aggroed = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        //if the ray hit the player then aggro
        if (Physics.Raycast(new Ray(transform.position, Vector3.left), out RaycastHit hit, _AggroRange))
        {
            if (hit.collider != null)
            {
                if (hit.collider.attachedRigidbody.gameObject.name == "Player")
                {
                    _Aggroed = true;
                }
            }

        }

        //if in aggro multiply the speed
        if (_Aggroed)
        {
            MoveSpeed = Level.ThisLevel.LevelSpeed * _AggroSpeedMultiplier;
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //do damage to the player and deactivate the collider
        if (other.gameObject.GetComponent<Player>() != null)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(_Damage,this.gameObject);
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x -_AggroRange, transform.position.y, transform.position.z));
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;

        if(_Health <= 0)
        {
            //TMP
            gameObject.SetActive(false);
            //death animation (routine) here, after deactivate the object
        }
    }
}
