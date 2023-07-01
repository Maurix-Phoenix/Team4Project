using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="T4P/Enemies/Create Enemy")]
public class EnemyTemplate : ScriptableObject
{
    public GameObject Prefab;
    public int Health;
    public float FireTriggerDistance;
    public float FireIntervail;
}
