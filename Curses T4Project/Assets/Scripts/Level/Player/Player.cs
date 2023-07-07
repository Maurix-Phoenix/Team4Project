//Player.cs
//by ANTHONY FEDELI

using System.Collections;
using UnityEngine;
using static T4P;
public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance;
    private Level CurrentLevel;

    [Header("Player Variables")]
    [SerializeField] private int _InitialHealth = 3;
    public int Cannonballs;

   

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CurrentLevel = Level.ThisLevel;
        Cannonballs = CurrentLevel.StartingCannonBalls;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void TakeDamage(int dmg, GameObject damager)
    {
        _InitialHealth -= dmg;

        //Damage Effect here

        T4Debug.Log($"Player damaged by {damager.name}");

        if (_InitialHealth <= 0)
        {
            //GameOver here
        }
    }
}
