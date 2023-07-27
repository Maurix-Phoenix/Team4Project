//Level.cs
//by MAURIZIO FISCHETTI

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static T4P;
using JetBrains.Annotations;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [Header("LevelEditor Save Data")]
    [SerializeField] public string LevelName;
    [SerializeField] public string LevelDesigner;
    [SerializeField] public int LevelID;
    [SerializeField] public Sprite LevelThumbnail = null;

    [Header("Level Completition")]
    [SerializeField] public bool Unlocked = false;
    [SerializeField] public int  StarsObtained = 0;
    [SerializeField] public bool StarCompleted = false;
    [SerializeField] public bool StarDoubloons = false;
    [SerializeField] public bool StarAce = false;
    [SerializeField] public int  TotalFlags = 3;
    [SerializeField] public bool FlagObtained;
    [SerializeField] public int  TotalDoubloons = 0;

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
    public int StartingHealth = 3;
    public int StartingCannonBalls = 5;

    public enum EndLevelType
    {
        None = -1,
        GameOver,
        Victory,
    }

    private void Awake()
    {
        //SHOULD BE IN PLAYER
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
        XStartingPosition = T4Project.XVisualLimit.x;
        XEndingPosition = T4Project.XVisualLimit.y;
    }

    private void Update()
    {
        //if (gameObject)
    }

    public void SaveLevel()
    {
#if UNITY_EDITOR
        gameObject.name = $"{LevelID}-{LevelName}-{LevelDesigner}";
        PrefabUtility.SaveAsPrefabAsset(gameObject, $"Assets/Resources/Levels/{gameObject.name}.prefab");
        T4Debug.Log($"LEVEL {gameObject.name} SAVED in Assets/Resources/Levels/");       

#endif
    }

    public void StartLevel()
    {
        T4Debug.Log("[Level] Started");
        GameManager.Instance.EventManager.RaiseOnLevelStart();
    }

    /// <summary>
    /// all the level entities will stop, should be called once, when player position.x < 0
    /// </summary>
    public void StopLevel()
    {
        T4Debug.Log("[Level] Stopped");
        GameManager.Instance.EventManager.RaiseOnLevelStop();
    }

    /// <summary>
    /// End the Level with the given outcome
    /// </summary>
    /// <param name="endtype"></param>
    public void EndLevel(EndLevelType endtype)
    {
        Time.timeScale = 0;
        T4Debug.Log($"[Level] Ended - {endtype}");
        switch(endtype)
        {
            case EndLevelType.None: { break; }
            case EndLevelType.GameOver: 
            {
                //call ui game over here
                GameManager.Instance.UIManager.HideAllUICanvas();
                GameManager.Instance.UIManager.ShowUICanvas("GameOverUI");
                break; 
            }
            case EndLevelType.Victory: 
            {
                    //check endlevel stars here
                    //check if player has all the flags and unlock the new one
                    Player player = GameManager.Instance.LevelManager.Player;


                    if(!StarCompleted)
                    {
                        StarCompleted = true;
                        StarsObtained++;
                    }

                    if(player.NOfDoubloons >= (TotalDoubloons * 70) / 100)
                    {
                        if(!StarDoubloons)
                        {
                            StarDoubloons = true;
                            StarsObtained++;
                        }
                    }
                    if(player.Health == StartingHealth)
                    {
                        if(!StarAce)
                        {
                            StarAce = true;
                            StarsObtained++;
                        }

                    }

                    //call ui level passed here
                    GameManager.Instance.UIManager.HideAllUICanvas();
                    GameManager.Instance.UIManager.ShowUICanvas("StageCompleteUI");
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 scale = new Vector3(2, 1, 1);
        Gizmos.DrawCube(T4Project.LanePosition[0], scale);
        Gizmos.DrawCube(T4Project.LanePosition[1], scale);
        Gizmos.DrawCube(T4Project.LanePosition[2], scale);
    }


    //SHOULD BE IN PLAYER
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
