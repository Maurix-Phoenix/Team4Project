//Pickup.cs
//by MAURIZIO FISCHETTI
using UnityEngine;
using static T4P;

public class Pickup : LevelEntity
{

    public PickupTemplate PT;
    [SerializeField] private T4Project.PickupsType _PickupType; //TODO
    [SerializeField] private int _Value = 1;
    [SerializeField] private float _AttractionDistance = 3;
    [SerializeField] private float _AttractionSpeed = 5.0f;

    [Header("Floating")]
    [SerializeField] private float _FloatingSpeed = 1f;
    [SerializeField] private float _Amplitude = 0.2f;

    private Rigidbody RB;


    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        if(PT != null )
        {
            //if pt is not null it will pass its values on the private current instance ones.
            _Value = PT.Value;
            _AttractionDistance = PT.AttractionDistance;
        }
    }

    private void FixedUpdate()
    {
        Move(Level.ThisLevel.LevelSpeed);
    }

    public void Move(float speed)
    {
        //floating movement
        float newY = transform.position.y + _Amplitude * Mathf.Sin(_FloatingSpeed * Time.time);

        //total movement
        RB.MovePosition(RB.position + new Vector3(-1, newY, 0) * speed * Time.fixedDeltaTime);

        //TODO: ignores the level movement if the player is inside the area of attraction
        MoveToPlayer();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Player>() != null)
        {
            //add resources (to player or level?)
            gameObject.SetActive(false);
        }
    }

    public void MoveToPlayer()
    {
        //TODO
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _AttractionDistance);
    }
}
