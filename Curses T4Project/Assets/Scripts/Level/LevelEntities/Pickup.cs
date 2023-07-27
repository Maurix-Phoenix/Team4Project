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
    [SerializeField] private float _AttractionSpeed = 10.0f;


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
            _PickupType = PT.Type;
        }
    }

    protected override void Update()
    {

        base.Update();
        if (!MoveToPlayer && Vector3.Distance(GameManager.Instance.LevelManager.Player.transform.position, transform.position) < _AttractionDistance)
        {
            MoveToPlayer = true;
            MoveSpeed = _AttractionSpeed;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = GameManager.Instance.LevelManager.Player;
            GameManager.Instance.LevelManager.Player.AddResource(_PickupType, _Value);

            gameObject.SetActive(false);
            T4Debug.Log($"{gameObject.name} Collected.");
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _AttractionDistance);
    }
}
