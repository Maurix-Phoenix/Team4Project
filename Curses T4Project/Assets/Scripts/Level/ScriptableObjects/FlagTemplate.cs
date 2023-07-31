using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag", menuName = "T4P/Flags/Create Flag")]
public class FlagTemplate : ScriptableObject
{
    public Sprite Sprite = null;
    public int ID = 0;
    public string Name = "Flag";
}
