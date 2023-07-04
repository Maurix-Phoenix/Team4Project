//Level.cs
//by MAURIZIO FISCHETTI

using System.Collections.Generic;
using UnityEngine;
using static T4P;

public class Level : MonoBehaviour
{
    public static Level ThisLevel;
    public GameObject Content;

    public List<LevelEntity> LevelObjects;

    public float LevelSpeed = 1.0f;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        if(LevelObjects.Count > 0)
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
}
