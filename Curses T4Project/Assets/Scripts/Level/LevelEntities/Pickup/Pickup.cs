//Pickup.cs
//by MAURIZIO FISCHETTI
using UnityEngine;
using static T4P;

public class Pickup : LevelEntity
{
    [Header("References")]
    [SerializeField] public T4Project.PickupsType PickupType;
    [SerializeField] private AudioClip _CoinSFX;
    [SerializeField] private AudioClip _CannonballSFX;
    [SerializeField] private AudioClip _FlagSFX;

    [Header("Variables")]
    [SerializeField] private int _Value = 1;
    public float AttractionDistance = 3;
    [SerializeField] private float _AttractionSpeed = 10.0f;


    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!MovePickUpToPlayer && Vector3.Distance(GameManager.Instance.LevelManager.Player.transform.position, transform.position) < AttractionDistance)
        {
            MovePickUpToPlayer = true;
            MoveSpeed = _AttractionSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayPickUPSFX();
            GameManager.Instance.LevelManager.Player.AddResource(PickupType, _Value);

            gameObject.SetActive(false);
            T4Debug.Log($"{gameObject.name} Collected.");
        }
    }

    private void PlayPickUPSFX()
    {
        switch (PickupType)
        {
            case T4Project.PickupsType.Cannonball:
                GameManager.Instance.AudioManager.PlaySFX(_CannonballSFX);
                break;
            case T4Project.PickupsType.Doubloon:
                GameManager.Instance.AudioManager.PlaySFX(_CoinSFX);
                break;
            case T4Project.PickupsType.Flag:
                GameManager.Instance.AudioManager.PlaySFX(_FlagSFX);
                break;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, AttractionDistance);
    }
}
