//Particles.cs
//by MAURIZIO FISCHETTI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : LevelEntityTemporary
{
    private ParticleSystem PS; 

    private void Awake()
    {
        PS = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(-1 * Time.deltaTime, 0, 0);

    }


}
