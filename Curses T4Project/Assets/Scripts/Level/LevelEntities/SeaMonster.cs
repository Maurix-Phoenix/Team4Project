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
        //if near x = 0 (playerpos) gets the aggro

        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(transform.position, Vector3.left);
        if (Physics.Raycast(ray, out hit, _AggroRange))
        {
            if (hit.collider != null && hit.collider.GetComponent<Player>() != null)
            {
                _Aggroed = true;
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
        if(_Health <= 0)
        {
            //death animation (routine) here, after deactivate the object
        }
    }
}
