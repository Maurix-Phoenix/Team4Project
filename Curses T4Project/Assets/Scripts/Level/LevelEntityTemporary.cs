//LevelEntityTemporary.cs
//by MAURIZIO FISCHETTI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntityTemporary : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Level.TemporaryObjects.Add(this);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance.Level.TemporaryObjects.Count > 0)
        {
            for (int i = GameManager.Instance.Level.TemporaryObjects.Count - 1; i >= 0; i--)
            {
                if (GameManager.Instance.Level.TemporaryObjects[i] == this)
                {
                    GameManager.Instance.Level.TemporaryObjects.Remove(this);
                }
            }
        }
    }
}
