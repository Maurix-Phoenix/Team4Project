using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static T4P;
public class Player : MonoBehaviour
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

    public void TakeDamage(GameObject damager)
    {
        _Health -= 1;

        //Damage Effect here

        T4Debug.Log($"Player damaged by {damager.name}");

        if( _Health <= 0 )
        {
            //GameOver here
        }
    }
}
