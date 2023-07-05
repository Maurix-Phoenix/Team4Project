//TMP -MF

using UnityEngine;
using static T4P;
public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance;
    private Level CurrentLevel;

    private int _Health = 3;
    private Vector3 _Direction = Vector3.zero;
    private float _MoveSpeed = 3.0f;
    private Rigidbody RB;

    private void Awake()
    {
        Instance = this;
        RB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        CurrentLevel = Level.ThisLevel;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
       
    }

    private void FixedUpdate()
    {
        RB.MovePosition(transform.position + _Direction * _MoveSpeed * Time.fixedDeltaTime);
        _Direction = Vector3.zero;
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _Direction.y = 1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            _Direction.y = -1;
        }

        //TMP
        if(Input.GetKey(KeyCode.D))
        {
            _Direction.x = 1;
        }
        if(Input.GetKey(KeyCode.A)) 
        {
            _Direction.x = -1;
        }
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        _Health -= dmg;

        //Damage Effect here

        T4Debug.Log($"Player damaged by {damager.name}");

        if (_Health <= 0)
        {
            //GameOver here
        }
    }
}
