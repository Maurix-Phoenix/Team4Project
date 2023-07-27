//LevelEntityTemporary.cs
//by MAURIZIO FISCHETTI
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LevelEntityTemporary : MonoBehaviour
{
    public float LifeTime = 5.0f;
    protected virtual void Start()
    {
        GameManager.Instance.LevelManager.CurrentLevel.TemporaryObjects.Add(this);
    }

    protected virtual void Update()
    {
        LifeTime -= Time.deltaTime;
        if(LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance.LevelManager.CurrentLevel.TemporaryObjects.Count > 0)
        {
            for (int i = GameManager.Instance.LevelManager.CurrentLevel.TemporaryObjects.Count - 1; i >= 0; i--)
            {
                if (GameManager.Instance.LevelManager.CurrentLevel.TemporaryObjects[i] == this)
                {
                    GameManager.Instance.LevelManager.CurrentLevel.TemporaryObjects.Remove(this);
                }
            }
        }
    }
}
