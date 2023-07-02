using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyTemplate _Template;


    int _health;
    // Start is called before the first frame update
    void Start()
    {
        //spawner
        _health = _Template.Health;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
