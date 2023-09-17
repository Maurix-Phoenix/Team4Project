//FishTemplate.cs
//by MAURIZIO FISCHETTI
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "The Drowned/Level Entities/New Fish")]
public class FishTemplate : ScriptableObject
{
    public string Name;
    public GameObject fishPrefab;
    public float ChangeDirectionIntervail = 1.0f;
    public float Speed = 1.0f;
    public float SpeedVariation = 0.5f;
    public bool ConsiderLevelSpeed = true;
    public int Population = 0;

    
}
