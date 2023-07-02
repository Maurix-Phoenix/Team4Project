using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickup : LevelEntity
{

    public PickupTemplate PT;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Player>() != null)
        {
            //add resources to player or level
        }
    }

    public void MoveToPlayer()
    {
        
    }
}
