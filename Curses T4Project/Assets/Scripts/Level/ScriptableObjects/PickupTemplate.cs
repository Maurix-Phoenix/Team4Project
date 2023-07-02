using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static T4P;

[CreateAssetMenu(fileName = "Pickup", menuName = "T4P/Pickups/Create Pickup")]
public class PickupTemplate : ScriptableObject
{
    public string Name = "";
    public T4Project.PickupsType Type = T4Project.PickupsType.None;
    public int Value;
    public float AttractionDistance = 1.8f;

    [Header("Prefab")]
    public GameObject Prefab;

    [Header("Audios")]
    public AudioClip DropSFX;
    public AudioClip PickupSFX;

   

}
