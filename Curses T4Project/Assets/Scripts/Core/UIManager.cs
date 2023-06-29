//UIManager.cs
//by MAURIZIO FISCHETTI
using System;
using System.Collections.Generic;
using UnityEngine;
using static T4P;
/// <summary>
/// [UI Manager]  manages, show/hide the UI
/// </summary>
public class UIManager : MonoBehaviour
{

    public GameObject UIContainer { get; private set; }
    public List<Canvas> UICanvasList;

    private void Awake()
    {
        Initialize();
    }
    private bool Initialize()
    {
        UIContainer = transform.Find("UI").gameObject;
        if(UIContainer != null ) 
        {
            InitializeList();
            T4Debug.Log("[UI Manager] Initializated");
        }
        else { T4Debug.Log("[UI Manager]  Canvas named 'UI' not found as child of UIManager", T4Debug.LogType.Warning); }
        return true;
    }

    private void InitializeList()
    {
        //Initialize the UICanvas List with the canvases child of the UIContainer
        foreach(Canvas c in UIContainer.GetComponentsInChildren<Canvas>())
        { 
            if(c != null && !UICanvasList.Contains(c))
            {
                UICanvasList.Add(c);
            }
        }
    }

    private GameObject GetUICanvas(string uiCanvasName)
    {
        /// Get by the name the Gameonbject if inside UICanvas list.
        for (int i = 0; i < UICanvasList.Count; i++)
        {
            if(UICanvasList[i] != null && UICanvasList[i].name == uiCanvasName)
            {
                return UICanvasList[i].gameObject;
            }
        }
        T4Debug.Log($"[UI Manager] cannot find any UI Canvas named {uiCanvasName} in the UICanvas list!");
        return null;
    }

     
    /// <summary>
    /// Deactivate the canvas that is inside the UICanvas List
    /// </summary>
    /// <param name="canvasName">the name of the canvas gameobject</param>
    public void HideUICanvas(string canvasName)
    {
        GameObject uiC = GetUICanvas(canvasName);
        if(uiC != null)
        {
            uiC.SetActive(false);
        }
        else { T4Debug.Log($"[UI Manager] {canvasName} cannot hide.", T4Debug.LogType.Error); }
    }

    /// <summary>
    /// Activate the canvas that is inside the UICanvas List
    /// </summary>
    /// <param name="canvasName">the name of the canvas gameobject</param>
    public void ShowUICanvas(string canvasName)
    {
        GameObject uiC = GetUICanvas(canvasName);
        if (uiC != null)
        {
            uiC.SetActive(false);
        }
        else { T4Debug.Log($"[UI Manager] {canvasName} cannot show.", T4Debug.LogType.Error); }
    }
}
