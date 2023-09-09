using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag", menuName = "The Drowned/Flags/Create Flag")]
public class FlagTemplate : ScriptableObject
{
    public int FlagID = -1;
    public string FlagName;
    [TextArea(3, 10)] public string FlagDescription;
    public Sprite FlagSprite;
}
