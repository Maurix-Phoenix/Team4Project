//Level.cs
//by MAURIZIO FISCHETTI

using System.Collections.Generic;
using UnityEngine;
using static T4P;

public class Level : MonoBehaviour
{
    public static Level ThisLevel;

    [Header("References")]
    public GameObject Content;

    [Header("Level Entities")]
    public List<LevelEntity> LevelObjects;
    public List<LevelEntityTemporary> TemporaryObjects;

    [Header("Level Variables")]
    public float LevelSpeed = 1.0f;
    [Tooltip("The Layer counting start from 0.")]
    public int NOfLayersUnderWater = 4;
    [Tooltip("The number of total layer goes from '0' to '-n'.\n'0' is the layer above the water.\n'-n' is the layer on the sea bed.")]
    public int ActualLayer = 0;
    public int UnitSpaceBetweenLayer = 3;
    public float StartingXPosition = 0f;

    [Header("Player Variables")]
    public int StartingCannonBalls = 5;


    private void Awake()
    {
        ThisLevel = this;
        //TMP
        if (Content == null)
        {
            Content = gameObject.transform.Find("Content").gameObject;
            foreach (var levelEntity in Content.GetComponentsInChildren<LevelEntity>())
            {
                LevelObjects.Add(levelEntity);
            }
            if (Content == null)
            {
                T4Debug.Log("[Level]: 'Content' child object missing!", T4Debug.LogType.Warning);
            }
        }

#if UNITY_EDITOR
        if (UnitSpaceBetweenLayer == 0)
        {
            T4Debug.Log("Unit Space Between Layer of PlayerMovement.cs can't be 0.", T4Debug.LogType.Error);
        }
#endif

        //Check the Layer limits
        if (ActualLayer > 0)
        {
            ActualLayer = 0;
        }
        if (ActualLayer < -NOfLayersUnderWater)
        {
            ActualLayer = -NOfLayersUnderWater;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (LevelObjects.Count > 0)
        {
            StartLevel();
        }

    }

    private void Update()
    {

    }

    private void PopulateLevel()
    {
        //this will populate the level taking a Level file (need level editor)
    }

    public void StartLevel()
    {
        //raise LevelStarted event
    }

    public void EndLevel()
    {
        //Raise LevelEnded event
    }

    public void SetLayer(int _modifier)
    {
        ActualLayer += _modifier;
    }
}
