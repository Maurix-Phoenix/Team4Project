//TMP -MF

using UnityEngine;
using static T4P;
public class Player : MonoBehaviour, IDamageable
{
    private Level CurrentLevel;

    private int _Health = 3;


    void Start()
    {
        CurrentLevel = Level.ThisLevel;
    }

    // Update is called once per frame
    void Update()
    {

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
