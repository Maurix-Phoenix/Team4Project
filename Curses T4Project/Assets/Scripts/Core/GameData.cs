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
    public int DeathCount = 0;
    public int EnemiesKilled = 0;
    public int CoinsCollected = 0;
    //Settings Datas
    [Header("Settings")]
    public float MusicVolume = 1.0f;
    public float SFXVolume = 1.0f;
}
