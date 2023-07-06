//LevelEntityTemporary.cs
//by MAURIZIO FISCHETTI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntityTemporary : MonoBehaviour
{
    public bool IsPaused { get ; set; }

    private void Start()
    {
        Level.ThisLevel.TemporaryObjects.Add(this);
    }

    private void OnDestroy()
    {
        Level level = Level.ThisLevel;
        for (int i = level.TemporaryObjects.Count - 1; i >= 0; i--)
        {
            if (level.TemporaryObjects[i] == this)
            {
                level.TemporaryObjects.Remove(this);
            }
        }
    }
}
