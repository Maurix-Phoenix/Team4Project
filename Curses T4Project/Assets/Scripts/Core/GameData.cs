//GameData.cs
//by MAURIZIO FISCHETTI

//WRITE HERE THE THINGS TO SAVE
//ACCESS WHEN NEEDED THROUGH DataManager.GameData
//                           GameManager.Instance.DataManager.GameData
using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameData
{
    //Settings Datas
    [Header("Settings")]
    public  float MusicVolume = 1.0f;
    public  float SFXVolume = 1.0f;
}
