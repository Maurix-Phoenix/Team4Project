using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static T4P;

public class LevelEditorManager : MonoBehaviour
{
    private string SpecialsFolder = "Specials";
    private string ObstaclesFolder = "Obstacles";
    private string EnemiesFolder = "Enemies";
    private string PickupsFolder = "Pickups";
    private string DecorationsFolder = "Decorations";

    public List<Level> Levels = new List<Level>();
    public List<GameObject> Specials = new List<GameObject>();
    public List<GameObject> Obstacles = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> Pickups = new List<GameObject>();
    public List<GameObject> Decorations = new List<GameObject>();


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        GetLevels();
        GetResources();
    }

    private void GetResources()
    {
        //get resources from the Resources Folder and add them to their list

        //specials
        GameObject[] specialsPrefabs = Resources.LoadAll<GameObject>(SpecialsFolder);
        foreach(GameObject goItem in specialsPrefabs)
        {
            T4Debug.Log($"[LevelEditor] importing {goItem.name} from {SpecialsFolder} folder.");
            Specials.Add(goItem);
        }
        //obstacles
        GameObject[] obstaclesPrefabs = Resources.LoadAll<GameObject>(ObstaclesFolder);
        foreach (GameObject goItem in obstaclesPrefabs)
        {
            T4Debug.Log($"[LevelEditor] importing {goItem.name} from {Obstacles} folder.");
            Obstacles.Add(goItem);
        }
        //enemies
        GameObject[] enemiesPrefabs = Resources.LoadAll<GameObject>(EnemiesFolder);
        foreach (GameObject goItem in enemiesPrefabs)
        {
            T4Debug.Log($"[LevelEditor] importing {goItem.name} from {EnemiesFolder} folder.");
            Enemies.Add(goItem);
        }
        //pickups
        GameObject[] pickupsPrefabs = Resources.LoadAll<GameObject>(PickupsFolder);
        foreach (GameObject goItem in pickupsPrefabs)
        {
            T4Debug.Log($"[LevelEditor] importing {goItem.name} from {PickupsFolder} folder.");
            Pickups.Add(goItem);
        }
        //decorations
        GameObject[] decorationsPrefabs = Resources.LoadAll<GameObject>(DecorationsFolder);
        foreach (GameObject goItem in decorationsPrefabs)
        {
            T4Debug.Log($"[LevelEditor] importing {goItem.name} from {DecorationsFolder} folder.");
            Decorations.Add(goItem);
        }
    }

    private void GetLevels()
    {
        T4Debug.Log("[LevelEditor] No levels implementation at the moment.");
    }

    private void ShowGrid()
    {

    }

    private void HideGrid()
    {

    }
}
