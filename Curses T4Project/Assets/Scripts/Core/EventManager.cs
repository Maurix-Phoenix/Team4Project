//EventManager.cs
//by MAURIZIO FISCHETTI
using System;
using System.ComponentModel;
using UnityEngine;
using static T4P;

public class EventManager :  MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        T4Debug.Log("[EventManager] Initialized.");
    }
    #region GameManager Events
    //GameManager main Events
    public Action GameStart;
    public Action GamePause;
    public Action GameUnpause;
    public Action GameQuit;

    public void RaiseOnGameStart()
    {
        GameStart?.Invoke();
    }
    public void RaiseOnGamePause()
    {
        GamePause?.Invoke();
    }
    public void RaiseOnGameUnpause()
    {
        GameUnpause?.Invoke();
    }
    public void RaiseOnGameQuit()
    {
        GameQuit?.Invoke();
    }
    #endregion


}
