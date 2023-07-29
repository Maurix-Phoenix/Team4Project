//LevelData.cs
//by MAURIZIO FISCHETTI

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    [Header("Level Info")]
    public int LevelID = 0;
    public string LevelName = "Level";
    public string LevelDesigner = "TheDrowned";

    [Header("Level Completition")]
    public bool Unlocked = false;
    public int StarsObtained = 0;
    public bool StarCompleted = false;
    public bool StarDoubloons = false;
    public bool StarAce = false;
    public int TotalFlags = 3;
    public bool FlagObtained;
    public int TotalDoubloons = 0;
}

