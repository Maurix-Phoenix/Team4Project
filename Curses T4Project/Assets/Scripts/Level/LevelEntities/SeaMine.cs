//SeaMine.cs
//by MAURIZIO FISCHETTI

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static T4P;


public class SeaMine : LevelEntity, IDamageable
{
    [Header("Sea Mine")]
    [SerializeField] private int _Health = 1;

    [Header("Explosion Effect")]
    public GameObject ExplosionPrefabVFX;
    [SerializeField] private bool _PlayerOnlyTrigger = false;
    [SerializeField] private int _ExplosionDamage = 1;
    [SerializeField] private float _ExplosionRange = 2.0f;
    [SerializeField] private float _WaitToExplode = 0.2f;
    [SerializeField] private bool _ExplodeAtStart = false;

    private bool _IsExploding = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (_ExplodeAtStart)
        {
            StartCoroutine(Explode(_WaitToExplode));
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {

        if(_PlayerOnlyTrigger)
        {
            if (other.gameObject.GetComponent<Player>() != null)
            {
                TakeDamage(1, other.gameObject);
            }
            else return;
        }
        else
        {
            TakeDamage(1, other.gameObject );
        }

    }

    /// <summary>
    /// Explode the mine creating an explosion.
    /// </summary>
    /// <param name="t">delaytime of the explosion</param>
    /// <returns></returns>
    public IEnumerator Explode(float t)
    {
        if (!_IsExploding)
        {

            _IsExploding = true;
            yield return new WaitForSeconds(t);

            

            //TODO: need to set the explosion particle system with the range of the explosion.
            Instantiate(ExplosionPrefabVFX, transform.position, Quaternion.identity, Level.ThisLevel.Content.transform);

            Collider[] others = Physics.OverlapSphere(transform.position, _ExplosionRange);

            foreach (Collider coll in others)
            {
                IDamageable damageable = null;
                if (coll.TryGetComponent<IDamageable>(out damageable))
                {
                    damageable.TakeDamage(_ExplosionDamage, gameObject);
                }
                else if (coll.transform.parent != null && coll.transform.parent.TryGetComponent<IDamageable>(out damageable))
                {
                    damageable.TakeDamage(_ExplosionDamage, gameObject );
                }               
            }
            gameObject.SetActive(false);

            yield return new WaitForEndOfFrame();
        }
        else
        {
           yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _ExplosionRange);
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;
        float timeToExplode = _WaitToExplode;

        if(_Health <= 0)
        {
            StartCoroutine(Explode(timeToExplode));
        }
    }
}
