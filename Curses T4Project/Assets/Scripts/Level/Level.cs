//Level.cs
//by MAURIZIO FISCHETTI

using System;
using System.Collections.Generic;
using UnityEngine;
using static T4P;


public class Level : MonoBehaviour
{
    [Header("LevelEditor Save Data")]
    [SerializeField] public string LevelName;
    [SerializeField] public string LevelDesigner;
    [SerializeField] public int LevelID;
    [SerializeField] public Sprite LevelThumbnail = null;

    [Header("Level Resources")]
    [SerializeField] private int _TotalCannonballs = 0;
    [SerializeField] private int _TotalDoubloons = 0;
    [SerializeField] private int _TotalFlags = 0;

    [Header("Level Savings")]
    public LevelData LevelData = new LevelData();

    [Header("Level Entities")]
    public List<LevelEntity> LevelObjects;
    public List<LevelEntityTemporary> TemporaryObjects;

    [Header("Level Conditions")]
    public float LevelSpeed = 1.0f;
    public bool IsInBossBattle = false;
    public bool IsLevelEnded = false;
    public bool IsFinalArrivalBeach = false;
    public bool PlayerHasReachBeach = false;

    [Header("Layer Variables")]
    public T4Project.LaneType StartingLane;
    [Tooltip("The Layer counting start from 0.")]
    [Range(0, 2)] public int NOfLayersUnderWater = 2;
    [Tooltip("The number of total layer goes from '0' to '-n'.\n'0' is the layer above the water.\n'-n' is the layer on the sea bed.")]
    public int ActualLayer = 0;
    [Tooltip("The number of total layer goes from '0' to '-n'.\n'0' is the layer above the water.\n'-n' is the layer on the sea bed.")]
    public int FinalLayer = 0;
    public int UnitSpaceBetweenLayer = 3;

    [Header("Player Positions")]
    public float XStartingPosition = -5f;
    public float XIntermediatePosition = 5f;
    public float XEndingPosition = 30f;
    public float YLanePosition;

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
        for(int i = 0; i < GameManager.Instance.DataManager.LevelData.Count; i++)
        {
            if (GameManager.Instance.DataManager.LevelData[i].LevelID == LevelID)
            {
                LevelData = GameManager.Instance.DataManager.GetLevelData(this);
            }
        }
        //Check the Layer limits for debugging at the start
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

        if (!IsFinalArrivalBeach)
        {
            if (FindObjectOfType<EndWall>() == null)
            {
                T4Debug.Log("IsFinalArrivalBeach in Level Script is set to FALSE. EndWall NEED to be placed and active!!! \n FinalArrivalBeach (GameObject) is not required in the scene.", T4Debug.LogType.Error);
            }
        }
        else
        {
            if (FindObjectOfType<FinalArrivalBeach>() == null)
            {
                T4Debug.Log("IsFinalArrivalBeach in Level Script is is set to TRUE. FinalArrivalBeach NEED to be placed and active!!! \n EndWall (GameObject) is not required in the scene.", T4Debug.LogType.Error);
            }
        }
    }

    private void Start()
    {
        InitializeStartingPosition();
        StopLevel();
    }

    private void InitializeStartingPosition()
    {
        switch (StartingLane)
        {
            case T4Project.LaneType.AboveWater:
                ActualLayer = 0;
                break;
            case T4Project.LaneType.UnderWater:
                ActualLayer = -1;
                break;
            case T4Project.LaneType.SeaBed:
                ActualLayer = -2;
                break;
        }
        YLanePosition = ActualLayer * UnitSpaceBetweenLayer;
        GameManager.Instance.LevelManager.Player.transform.position = new Vector3(XStartingPosition, YLanePosition, 0f);
    }

    public void StartLevel()
    {
        CalculateFlags();
        CalculateCannonballs();
        CalculateDoubloons();
        T4Debug.Log($"[Level] Started [Resources Cannonballs {_TotalCannonballs} - Flags:{_TotalFlags} - TotalDoubloons:{_TotalDoubloons}");

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
        switch (endtype)
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


                    if(!LevelData.StarCompleted)
                    {
                        LevelData.StarCompleted = true;
                        LevelData.StarsObtained++;
                    }

                    if(!LevelData.StarDoubloons && _TotalDoubloons > 0)
                    {
                        if (player.NOfDoubloons >= (_TotalDoubloons * 70) / 100)
                        {
                            if (!LevelData.StarDoubloons)
                            {
                                LevelData.StarDoubloons = true;
                                LevelData.StarsObtained++;
                            }
                        }
                    }

                    if(player.Health == StartingHealth)
                    {
                        if(!LevelData.StarAce)
                        {
                            LevelData.StarAce = true;
                            LevelData.StarsObtained++;
                        }

                    }

                    if(!LevelData.FlagObtained && _TotalFlags > 0)
                    {
                        if (player.NOfFlags == _TotalFlags)
                        {
                            LevelData.FlagObtained = true;
                        }
                    }


                    //call ui level passed here
                    GameManager.Instance.UIManager.HideAllUICanvas();

                    GameManager.Instance.DataManager.SaveLevel(LevelData.LevelID);

                    GameManager.Instance.UIManager.ShowUICanvas("StageCompleteUI");
                    
                break;
            }
        }
    }

    //Calculate the Flags based on the objects inside the list and their loot (minimun value)
    private void CalculateFlags()
    {
        foreach (LevelEntity le in LevelObjects)
        {
            Pickup pk;
            if (le.TryGetComponent(out pk))
            {
                if (pk.PickupType == T4Project.PickupsType.Flag)
                {
                    _TotalFlags++;
                }
            }

            if (le.CanDropLoot)
            {
                foreach (LevelEntity.Loot loot in le.LootList)
                {
                    if (loot.PickupPrefab.TryGetComponent(out pk))
                    {
                        if (pk.PickupType == T4Project.PickupsType.Flag)
                        {
                           _TotalFlags += loot.DropQuantityRange.x;
                        }
                    }
                }
            }
        }
    }

    //Calculate the doubloons based on the objects inside the list and their loot (minimun value)
    private void CalculateDoubloons()
    {
        foreach(LevelEntity le in LevelObjects)
        {
            
            Pickup pk;
            if(le.TryGetComponent(out pk))
            {
                if (pk.PickupType == T4Project.PickupsType.Doubloon)
                {
                    _TotalDoubloons++;
                }
            }

            if(le.CanDropLoot)
            {
                foreach(LevelEntity.Loot loot in le.LootList)
                {
                    if(loot.PickupPrefab.TryGetComponent(out pk))
                    {
                        if(pk.PickupType == T4Project.PickupsType.Doubloon)
                        {
                            _TotalDoubloons += loot.DropQuantityRange.x;
                        }
                    }
                }
            }
        }
    }

    //Calculate the Cannonballs based on the objects inside the list and their loot (minimun value)
    private void CalculateCannonballs()
    {
        foreach (LevelEntity le in LevelObjects)
        {
            Pickup pk;
            if (le.TryGetComponent(out pk))
            {
                if (pk.PickupType == T4Project.PickupsType.Cannonball)
                {
                    _TotalCannonballs++;
                }
            }

            if (le.CanDropLoot)
            {
                foreach (LevelEntity.Loot loot in le.LootList)
                {
                    if (loot.PickupPrefab.TryGetComponent(out pk))
                    {
                        if (pk.PickupType == T4Project.PickupsType.Cannonball)
                        {
                            _TotalCannonballs += loot.DropQuantityRange.x;
                        }
                    }
                }
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
