//GameData.cs
//by MAURIZIO FISCHETTI

//WRITE HERE THE THINGS TO SAVE
//ACCESS WHEN NEEDED THROUGH DataManager.GameData
//                           GameManager.Instance.DataManager.GameData
using System;
using UnityEngine;

[Serializable]
public class GameData
{
    //Game Datas
    [Header("GameDatas")]
    public int TotalDeathCount = 0;
    public int TotalEnemiesKilled = 0;
    public int TotalCoinsCollected = 0;
    public int TotalFlagsObtained = 0;
    public int TotalStarsObtained = 0;


    //Settings Datas
    [Header("Settings")]
    public bool Muted = false;
    public float MusicVolume = 1.0f;
    public float SFXVolume = 1.0f;
}
