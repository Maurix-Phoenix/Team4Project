//SeaMine.cs
//by MAURIZIO FISCHETTI

using System.Collections;
using UnityEngine;
using static T4P;


public class SeaMine : LevelEntity
{
    //Custom object vars
    [SerializeField] private float _FloatingSpeed = 1f;
    [SerializeField] private float _Amplitude = 0.5f;

    [Header("Explosion Effect")]
    public GameObject ExplosionPrefabVFX;
    [SerializeField] private float _ExplosionRange = 2.0f;
    [SerializeField] private float _WaitToExplode = 0.2f;
    [SerializeField] private bool _ExplodeAtStart = false;

    private Vector3 _StartingPos;

    Rigidbody RB;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _StartingPos = transform.position;
        if(_ExplodeAtStart )
        {
            StartCoroutine(Explode(_WaitToExplode));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RB.position.x < T4Project.XVisualLimit.x)
        {
            gameObject.SetActive(false);
        }
        
    }

    private void FixedUpdate()
    {
        MoveWithLevel(Level.ThisLevel.LevelSpeed);
    }

    public void MoveWithLevel(float speed)
    {
        //floating movement
        float newY = _StartingPos.y + _Amplitude * Mathf.Sin(_FloatingSpeed * Time.time);

        RB.MovePosition(RB.position + new Vector3(-1, newY, 0) * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Player>() != null)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(this.gameObject);
        }

       StartCoroutine(Explode(_WaitToExplode));
    }

    /// <summary>
    /// Explode the mine creating an explosion.
    /// </summary>
    /// <param name="t">delaytime of the explosion</param>
    /// <returns></returns>
    public IEnumerator Explode(float t)
    {
        yield return new WaitForSeconds(t);

        gameObject.SetActive(false);

        //TODO: need to set the explosion particle system with the range of the explosion.
        Instantiate(ExplosionPrefabVFX, transform.position, Quaternion.identity);

        Collider[] others = Physics.OverlapSphere(transform.position, _ExplosionRange);

        foreach(Collider coll in others)
        {

            if(coll.gameObject.GetComponent<SeaMine>() != null)
            {
                SeaMine sm = coll.gameObject.GetComponent<SeaMine>();
                sm.StartCoroutine(sm.Explode(sm._WaitToExplode));
            }

            if(coll.gameObject.GetComponent<Player>())
            {
                Player player = coll.gameObject.GetComponent<Player>();
                player.TakeDamage(this.gameObject);
            }
        }
        yield return new WaitForEndOfFrame();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _ExplosionRange);
    }
}
