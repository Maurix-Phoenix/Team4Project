//Level.cs
//by MAURIZIO FISCHETTI

using System;
using System.Collections.Generic;
using UnityEngine;
using static T4P;

public class Level : MonoBehaviour
{
    public static Level ThisLevel;
    private GameManager GM;
    [Header("References")]
    public GameObject Content;

    [Header("Level Entities")]
    public List<LevelEntity> LevelObjects;
    public List<LevelEntityTemporary> TemporaryObjects;

    [Header("Level Conditions")]
    public float LevelSpeed = 1.0f;
    public bool IsInBossBattle = false;
    public bool IsLevelEnded = false;

    [Header("Layer Variables")]
    [Tooltip("The Layer counting start from 0.")]
    public int NOfLayersUnderWater = 4;
    [Tooltip("The number of total layer goes from '0' to '-n'.\n'0' is the layer above the water.\n'-n' is the layer on the sea bed.")]
    public int ActualLayer = 0;
    [Tooltip("The number of total layer goes from '0' to '-n'.\n'0' is the layer above the water.\n'-n' is the layer on the sea bed.")]
    public int FinalLayer = 0;
    public int UnitSpaceBetweenLayer = 3;

    [Header("Player Animation Positions")]
    public float XStartingPosition = -6f;
    public float XIntermediatePosition = 10f;
    public float XEndingPosition = 26f;

    [Header("Player Variables")]
    public int StartingCannonBalls = 5;

    public enum EndLevelType
    {
        None = -1,
        GameOver,
        Victory,
    }

    private void Awake()
    {
        ThisLevel = this;
        GM = GameManager.Instance;



        //Check the Layer limits
        if (ActualLayer > 0)
        {
            ActualLayer = 0;
        }
        if (ActualLayer < - NOfLayersUnderWater)
        {
            ActualLayer = - NOfLayersUnderWater;
        }

#if UNITY_EDITOR
        if (UnitSpaceBetweenLayer == 0)
        {
            T4Debug.Log("Unit Space Between Layer of PlayerMovement.cs can't be 0.", T4Debug.LogType.Warning);
        }
#endif
    }

    private void Start()
    {
        InitializePosition();
        StopLevel();
    }

    private void InitializePosition()
    {
        XEndingPosition = T4Project.XVisualLimit.x;
        XStartingPosition = T4Project.XVisualLimit.y;
    }

    private void Update()
    {
        //if (gameObject)
    }

    private void PopulateLevel()
    {
        //this will populate the level taking a Level file (need level editor)
    }

    /// <summary>
    /// start the level
    /// </summary>
    public void StartLevel()
    {
        T4Debug.Log("[Level] Started");
        MoveLevel();
    }

    /// <summary>
    /// all the level entities will stop
    /// </summary>
    public void StopLevel()
    {
        T4Debug.Log("[Level] Stopped");
        foreach (var levelEntity in LevelObjects)
        {
            levelEntity.IsStopped = true;
        }
    }

    /// <summary>
    /// all the level entities will start move
    /// </summary>
    public void MoveLevel()
    {
        T4Debug.Log("[Level] Move");
        foreach (var levelEntity in LevelObjects)
        {
            levelEntity.IsStopped = false;
        }
    }

    /// <summary>
    /// End the Level with the given outcome
    /// </summary>
    /// <param name="endtype"></param>
    public void EndLevel(EndLevelType endtype)
    {
        T4Debug.Log($"[Level] Ended - {endtype}");
        switch(endtype)
        {
            case EndLevelType.None: { break; }
            case EndLevelType.GameOver: 
            {
                StopLevel();
                
                //call ui game over here
                GM.UIManager.HideAllUICanvas();
                GM.UIManager.ShowUICanvas("GameOverUI");
                break; 
            }
            case EndLevelType.Victory: 
            {
                    //call ui level passed here
                    GM.UIManager.HideAllUICanvas();
                    GM.UIManager.ShowUICanvas("StageCompletedUI");
                break;
            }
        }
    }

    public void SetLayer(int _modifier)
    {
        ActualLayer += _modifier;

        if (ActualLayer > 0)
        {
            ActualLayer = 0;
        }
        if (ActualLayer < -NOfLayersUnderWater)
        {
            ActualLayer = -NOfLayersUnderWater;
        }
    }
}
