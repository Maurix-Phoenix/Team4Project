//UIManager.cs
//by MAURIZIO FISCHETTI
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static T4P;
/// <summary>
/// [UI Manager]  manages, show/hide the UI
/// </summary>
public class UIManager : MonoBehaviour
{
    public string CanvasToShow = "MainMenuUI";
    public GameObject UIContainer { get; private set; }

    [Header("Canvas Animation")]
    [SerializeField] private float _FadeAnimationTime = 0.5f;

    public CanvasGroup CanvasGroup { get; private set; }
    public List<Canvas> UICanvasList;
    public List<TMP_Text> UITextList = new List<TMP_Text>();
    public LevelPanel LevelPanelSelection;
    public FlagCollection FlagsCollection;
    public LevelUI LevelUI;
    public StageCompleteUI StageCompleteUI;
    public TutorialUI TutorialUI;
    public ToggleButtonUI ToggleButtonUI;
    public GameObject LabelPrefab;

    private void Awake()
    {
        Initialize();
    }
    private bool Initialize()
    {
        UIContainer = transform.Find("UI").gameObject;
        CanvasGroup = UIContainer.GetComponent<CanvasGroup>();
        if (UIContainer != null || CanvasGroup != null)
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
        foreach (Canvas c in UIContainer.GetComponentsInChildren<Canvas>(includeInactive: true))
        {
            if (c != null && !UICanvasList.Contains(c) && c != UIContainer.GetComponent<Canvas>())
            {
                UICanvasList.Add(c);
            }
        }

        //Initialize the UITexts with the texts child of UIContainer
        foreach (TMP_Text tmpT in UIContainer.GetComponentsInChildren<TMP_Text>(includeInactive: true))
        {
            if (tmpT != null && !UITextList.Contains(tmpT))
            {
                UITextList.Add(tmpT);
            }
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.EventManager.LevelStart += OnLevelStart;
    }
    private void OnDisable()
    {
        GameManager.Instance.EventManager.LevelStart -= OnLevelStart;
    }

    private void OnLevelStart()
    {
        LevelUI.LoadFlag();
        LevelUI.UpdateLevelUI();

        StageCompleteUI.LoadFlag();
        StageCompleteUI.UpdateStageCompleteUI();
    }

    private GameObject GetUICanvas(string uiCanvasName)
    {
        /// Get by the name the Gameonbject if inside UICanvas list.
        for (int i = 0; i < UICanvasList.Count; i++)
        {
            if (UICanvasList[i] != null && UICanvasList[i].name == uiCanvasName)
            {
                return UICanvasList[i].gameObject;
            }
        }
        T4Debug.Log($"[UI Manager] cannot find any UI Canvas named {uiCanvasName} in the UICanvas list!");
        return null;
    }


    //DELETE THIS
    private TMP_Text GetUIText(string uiTextName)
    {
        //Get by the name of the text inside UICanvas list.
        for (int i = 0; i < UITextList.Count; i++)
        {
            if (UITextList[i] != null && UITextList[i].name == uiTextName)
            {
                return UITextList[i];
            }
        }
        T4Debug.Log($"[UI Manager] cannot find any UI Text named {uiTextName} in the UIText list!");
        return null;
    }


    /// <summary>
    /// Deactivate the canvas that is inside the UICanvas List
    /// </summary>
    /// <param name="canvasName">the name of the canvas gameobject</param>
    public void HideUICanvas(string canvasName)
    {
        GameObject uiC = GetUICanvas(canvasName);
        if (uiC != null)
        {
            uiC.SetActive(false);
        }
        else { T4Debug.Log($"[UI Manager] {canvasName} cannot hide.", T4Debug.LogType.Error); }
    }

    /// <summary>
    /// Activate the canvas that is inside the UICanvas List
    /// </summary>
    /// <param name="canvasName">the name of the canvas gameobject</param>
    public void ShowUICanvas(string canvasName, bool isAnimated = true)
    {
        GameObject uiC = GetUICanvas(canvasName);
        if (uiC != null)
        {
            if(isAnimated)
            {
                StartCoroutine(FadeAnimation(_FadeAnimationTime));
            }
            uiC.SetActive(true);
            
        }
        else { T4Debug.Log($"[UI Manager] {canvasName} cannot show.", T4Debug.LogType.Error); }

    }

    /// <summary>
    /// Show only the specified canvas and hide the others
    /// </summary>
    /// <param name="canvasName"></param>
    public void ShowUICanvasOnly(string canvasName)
    {
        HideAllUICanvas();
        ShowUICanvas(canvasName);
    }

    /// <summary>
    /// Set a canvas to be shown on Load
    /// </summary>
    /// <param name="canvasName"></param>
    public void SetUICanvasOnLoad(string canvasName)
    {
        if (GetUICanvas(canvasName) != null)
        {
            CanvasToShow = canvasName;
        }
    }

    /// <summary>
    /// Use this to show a canvas that was registered for being show when a scene load
    /// </summary>
    public void ShowUICanvasOnLoad()
    {
        ShowUICanvasOnly(CanvasToShow);
        CanvasToShow = null;
    }


    /// <summary>
    /// Deactivates all the ui elements in the list
    /// </summary>
    public void HideAllUICanvas()
    {
        foreach (Canvas obj in UICanvasList)
        {
            HideUICanvas(obj.name);
            CanvasToShow = null;
        }
    }

    /// <summary>
    /// Create a World Coordinates UI Label
    /// </summary>
    /// <param name="text">the text of the label</param>
    /// <param name="position">the world coords (or parent local)</param>
    /// <param name="parent">the parent object if specified the position will be in Local coords</param>
    /// <param name="lifetime">the lifetime in seconds of the label (0: permanent)</param>
    public UILabel CreateUILabel(string text, Vector3 position, Transform parent = null, float lifetime = 0)
    {
        GameObject labelO = Instantiate(LabelPrefab);
        UILabel label = labelO.GetComponentInChildren<UILabel>();

        if (label != null)
        {
            label.UpdateLabel(text, position, parent, lifetime);
        }
        else
        {
            Destroy(labelO.gameObject);
            T4Debug.Log("[UI MANAGER] UILabel is null, destroying the object.", T4Debug.LogType.Warning);
        }

        return label;
    }

    /// <summary>
    /// Update the specified UILabel
    /// </summary>
    /// <param name="label">the Label to Update</param>
    /// <param name="text">the text</param>
    /// <param name="position">the position</param>
    /// <param name="parent">the parent</param>
    /// <param name="lifetime">the lifetime</param>
    public void UpdateUILabel(UILabel label, string text, Vector3 position, Transform parent = null, float lifetime = 0)
    {
        if (label != null)
        {
            label.UpdateLabel(text, position, parent, lifetime);
        }
    }


    IEnumerator FadeAnimation( float time)
    {
        CanvasGroup.alpha = 0;

        float elapsedT = 0;

        while(elapsedT <= time)
        {
            elapsedT += Time.unscaledDeltaTime;
            CanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedT / time);
            yield return null;
        }

        CanvasGroup.alpha = 1;
    }


}
