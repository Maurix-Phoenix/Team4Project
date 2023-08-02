//LevelManager.cs
//by MAURIZIO FISCHETTI

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static T4P;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> LevelPrefabsList = new List<GameObject>();

    public GameObject LevelToLoad = null;
    public Level CurrentLevel = null;

    public GameObject PlayerPrefab;
    public Player Player { get; private set; }
    public LevelPanel LevelPanel = null;

    private void Awake()
    {
        foreach (GameObject level in Resources.LoadAll("Levels/"))
        {
            LevelPrefabsList.Add(level);
        }

        PlayerPrefab = Resources.Load<GameObject>("Specials/Player");

        if (LevelPrefabsList.Count == 0)
        {
            T4Debug.Log($"[Level Manager] LevelPrefabs List is Empty!", T4Debug.LogType.Warning);
        }
        else
        {
            LevelToLoad = LevelPrefabsList[0];
            CurrentLevel = LevelToLoad.GetComponent<Level>();
        }

    }
    private void Start()
    {


    }
    /// <summary>
    /// Load the level with the given prefab
    /// </summary>
    /// <param name="levelObj"></param>
    public void LoadLevel(GameObject levelObj)
    {
        foreach (GameObject level in LevelPrefabsList)
        {
            if (level == levelObj)
            {
                LevelToLoad = levelObj;
                InstantiateLevel(level);
                return;
            }
        }
        T4Debug.Log($"[Level Manager] Could not find a Level gameobject: {levelObj}");
    }
    /// <summary>
    /// Load the level with the given name
    /// </summary>
    /// <param name="levelName">the name of the level</param>
    public void LoadLevel(string levelName)
    {
        foreach (GameObject level in LevelPrefabsList)
        {
            if (level.GetComponent<Level>().LevelName == levelName)
            {
                LevelToLoad = level;
                InstantiateLevel(level);
                return;
            }
        }
        T4Debug.Log($"[Level Manager] Could not find a Level with name: {levelName}");
        
    }
    /// <summary>
    /// Load the level with the given ID
    /// </summary>
    /// <param name="id">the ID of the level</param>
    public void LoadLevel(int id)
    {
        foreach(GameObject level in LevelPrefabsList)
        {
            if(level.GetComponent<Level>().LevelID == id)
            {
                LevelToLoad = level;
                InstantiateLevel(level);
                return;
            }
        }
        T4Debug.Log($"[Level Manager] Could not find a Level with ID: {id}");
    }
    public void LevelSelectionPrevious() 
    {
        if(GameManager.Instance.CurrentScene.name == "LevelSelection")
        {
            int index = LevelPrefabsList.IndexOf(CurrentLevel.gameObject) -1;
            if(index < 0)
            {
                index = LevelPrefabsList.Count - 1;
            }


            CurrentLevel = LevelPrefabsList[index].GetComponent<Level>();
            LevelPanel.UpdateLevelPanel(CurrentLevel);
            LevelToLoad = CurrentLevel.gameObject;

        }
    }
    public void LevelSelectionNext() 
    {
        if (GameManager.Instance.CurrentScene.name == "LevelSelection")
        {
            int index = LevelPrefabsList.IndexOf(CurrentLevel.gameObject) +1;
            if (index > LevelPrefabsList.Count - 1)
            {
                index = 0;
            }

            CurrentLevel = LevelPrefabsList[index].GetComponent<Level>();
            LevelPanel.UpdateLevelPanel(CurrentLevel);
            LevelToLoad = CurrentLevel.gameObject;

        }
    }
    public void LoadNextLevel()
    {

        int index = CurrentLevel.LevelID + 1;
        T4Debug.Log($"selected index: {index}");
        if (index > LevelPrefabsList.Count - 1)
        {
            index = 0;
        }

        LevelToLoad = LevelPrefabsList[index];
        T4Debug.Log($"adjusted index: {index}");

        GameManager.Instance.LoadScene("Level");
    }
    public void LoadPreviousLevel()
    {
        int index = CurrentLevel.LevelID - 1;
        T4Debug.Log($"selected index: {index}");
        if (index < 0)
        {
            index = LevelPrefabsList.Count - 1;
        }

        LevelToLoad = LevelPrefabsList[index];
        T4Debug.Log($"adjusted index: {index}");

        GameManager.Instance.LoadScene("Level");
    }
    private void InstantiateLevel(GameObject level)
    {
        CurrentLevel = Instantiate(level).GetComponent<Level>();
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        Player = Instantiate(PlayerPrefab).GetComponent<Player>();
    }
}
