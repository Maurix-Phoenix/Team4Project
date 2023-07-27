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
    protected override void Update()
    {
        base.Update();
        transform.position += new Vector3(-1 * Time.deltaTime * GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed, 0, 0);

    }


}
