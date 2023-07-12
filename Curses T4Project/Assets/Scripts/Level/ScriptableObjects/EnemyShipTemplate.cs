using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyShip", menuName ="T4P/Enemies/Create EnemyShip")]
public class EnemyShipTemplate : ScriptableObject
{
    public GameObject Prefab;
    public int Health;

}
