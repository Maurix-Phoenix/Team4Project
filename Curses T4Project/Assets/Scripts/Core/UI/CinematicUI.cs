using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static T4P;

public class CinematicUI : MonoBehaviour
{
    [Header("CinematicUI")]
    [SerializeField] private GameObject _TopBar;
    [SerializeField] private GameObject _Panel;
    [SerializeField] private GameObject _BottomBar;
    [SerializeField] private float _TransitionSpeedModifier = 0.8f;
    [SerializeField] private float _PanelTransparency = 0.8f;
    private bool _Continue = false;

    [Header("Dialogue - Captain")]
    [SerializeField] private float _ChangeTextureSpeed = 0.8f;
    [SerializeField] private GameObject _CaptainTexture;
    [SerializeField] private List<Sprite> _CaptainImages;
    [SerializeField] private float _Distance = 100;
    [SerializeField] private float _ChangePositionSpeed = 0.8f;
    private float _StartDistance;
    private bool _CaptainInPosition = false;

    [Header("Dialogue - Button")]
    [SerializeField] private float _ButtonTimerSpawn = 3;
    [SerializeField] private List<Button> _NextDialogueButton;

    [Header("Dialogue - Panel")]
    [SerializeField] private float _TransitionPanelSpeed = 0.8f;
    [SerializeField] private GameObject _DialoguePanel;

    [Header("Dialogue - Text")]
    [SerializeField] private float _TransitionTextSpeed = 0.8f;
    [SerializeField] private TextMeshProUGUI _DialogueText;
    [SerializeField] private TextMeshProUGUI _SkipText;
    [SerializeField] private int _DialogueStringIndex;
    [SerializeField][Multiline(3)] private List<string> _DialogueString;
    [SerializeField] private AudioClip _NextPanelAudio;

    private void Awake()
    {
        _StartDistance = _Distance;
        ResetValue();
    }

    private void Start()
    {
        ResetValue();
    }

    private void Update()
    {
        if (Input.anyKeyDown && _Continue)
        {
            _Continue = false;
            GameManager.Instance.AudioManager.PlaySFX(_NextPanelAudio);
            switch (_DialogueStringIndex)
            {
                case 0:
                    NextDialogButton(true);
                    break;
                case 1:
                    NextDialogButton(false);
                    ActivateDeactivePanelButton();
                    ChangeCaptainPosition(-100);
                    ChangeCaptainTexture(1);
                    break;
                case 2:
                    NextDialogButton(true);
                    break;
                case 3:
                    DisableAll();
                    break;
                default:
                    break;
            }
        }
    }

    private void OnEnable()
    {
        ResetValue();
    }

    private void ResetValue()
    {
        if (GameManager.Instance.LevelManager.CurrentLevel.IsTutorial && GameManager.Instance.CurrentScene.name == "Level")
        {
            _DialogueStringIndex = 0;
            _Distance = _StartDistance;
            _SkipText.gameObject.SetActive(false);

            _CaptainTexture.GetComponent<Image>().sprite = _CaptainImages[0];

            if (_NextDialogueButton.Count <= 0)
            {
                T4Debug.Log($"[UIManager] CinematicUI has empty list of next buttons.");
            }
            else
            {
                for (int i = 0; i < _NextDialogueButton.Count; i++)
                {
                    _NextDialogueButton[i].gameObject.SetActive(false);
                }
            }

            ResetBlackBar();
            StartCoroutine(ShowCutsceneUI());
        }
    }

    public void DisableAll()
    {
        _NextDialogueButton[_DialogueStringIndex].gameObject.SetActive(false);
        _SkipText.gameObject.SetActive(false);
        StartCoroutine(HideDialoguePanel());
        StartCoroutine(HideDialogueText());
        StartCoroutine(HideCaptain());
        StartCoroutine(HideCutsceneUI());
    }
    #region Cutscene
    private void ResetBlackBar()
    {
        _TopBar.GetComponent<RectTransform>().localScale = new Vector3(1, 0, 1);
        _Panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        _BottomBar.GetComponent<RectTransform>().localScale = new Vector3(1, 0, 1);

        _DialoguePanel.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _DialogueText.color = new Color(0, 0, 0, 0);
    }
    private IEnumerator ShowCutsceneUI()
    {
        float newScale = 0;
        float localAlpha = 0;
        while (newScale <= 1)
        {
            newScale += (Time.deltaTime * _TransitionSpeedModifier);

            _TopBar.GetComponent<RectTransform>().localScale = new Vector3(1, newScale, 1);
            _BottomBar.GetComponent<RectTransform>().localScale = new Vector3(1, newScale, 1);

            localAlpha = newScale * _PanelTransparency;
            localAlpha += ((Time.deltaTime * _TransitionSpeedModifier));

            _Panel.GetComponent<Image>().color = new Color(0, 0, 0, localAlpha);

            yield return null;
        }

        _TopBar.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        _BottomBar.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        StartCoroutine(ShowCaptain());
        StartCoroutine(MoveCaptain(_Distance));
        StartCoroutine(ShowDialoguePanel());
    }
    private IEnumerator HideCutsceneUI()
    {
        float newScale = 1;
        float localAlpha = 1;
        while (newScale >= 0)
        {
            newScale -= (Time.deltaTime * _TransitionSpeedModifier);

            _TopBar.GetComponent<RectTransform>().localScale = new Vector3(1, newScale, 1);
            _BottomBar.GetComponent<RectTransform>().localScale = new Vector3(1, newScale, 1);

            localAlpha = newScale * _PanelTransparency;
            localAlpha -= ((Time.deltaTime * _TransitionSpeedModifier));

            _Panel.GetComponent<Image>().color = new Color(0, 0, 0, localAlpha);

            yield return null;
        }

        _TopBar.GetComponent<RectTransform>().localScale = new Vector3(1, 0, 1);
        _BottomBar.GetComponent<RectTransform>().localScale = new Vector3(1, 0, 1);
        _Panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        GameManager.Instance.LevelManager.CurrentLevel.StartLevel();

        GameManager.Instance.UIManager.ShowUICanvasOnly("LevelUI");
        gameObject.SetActive(false);
    }
    #endregion
    #region DialoguePanel
    private void EnableCorrectButton()
    {
        _Continue = true;
        _SkipText.gameObject.SetActive(true);
        _NextDialogueButton[_DialogueStringIndex].gameObject.SetActive(true);
    }
    public void ActivateDeactivePanelButton()
    {
        if (_DialoguePanel.GetComponent<Image>().color.a > 0.5)
        {
            StartCoroutine(HideDialoguePanel());
        }
        else
        {
            StartCoroutine(ShowDialoguePanel());
        }
    }
    private IEnumerator ShowDialoguePanel()
    {
        float localAlpha = 0;
        while (localAlpha <= 1)
        {
            if (_CaptainInPosition)
            {
                localAlpha += ((Time.deltaTime * _TransitionPanelSpeed));

                _DialoguePanel.GetComponent<Image>().color = new Color(1, 1, 1, localAlpha);
            }
            yield return null;
        }

        _DialoguePanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        StartCoroutine(ShowDialogueText());
    }
    private IEnumerator HideDialoguePanel()
    {
        CancelInvoke("EnableCorrectButton");
        float localAlpha = 1;
        while (localAlpha >= 0)
        {
            localAlpha -= ((Time.deltaTime * _TransitionPanelSpeed));

            _DialoguePanel.GetComponent<Image>().color = new Color(1, 1, 1, localAlpha);

            yield return null;
        }

        _DialoguePanel.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    #endregion
    #region DialogueText
    public void NextDialogButton(bool SpawnNextDialogue)
    {
        _SkipText.gameObject.SetActive(false);
        StartCoroutine(DialogueSwitch(SpawnNextDialogue));
    }
    private IEnumerator DialogueSwitch(bool SpawnNextDialogue)
    {
        _NextDialogueButton[_DialogueStringIndex].gameObject.SetActive(false);

        float localAlpha = 1;
        while (localAlpha >= 0)
        {
            localAlpha -= ((Time.deltaTime * _TransitionTextSpeed));

            _DialogueText.color = new Color(1, 1, 1, localAlpha);

            yield return null;
        }

        _DialogueText.color = new Color(1, 1, 1, 0);
        _DialogueStringIndex++;
        _DialogueText.text = _DialogueString[_DialogueStringIndex];

        if (SpawnNextDialogue)
        {
            localAlpha = 0;
            while (localAlpha <= 1)
            {
                if (_CaptainInPosition)
                {
                    localAlpha += ((Time.deltaTime * _TransitionTextSpeed));

                    _DialogueText.color = new Color(1, 1, 1, localAlpha);
                }
                yield return null;
            }

            _DialogueText.color = new Color(1, 1, 1, 1);
            Invoke("EnableCorrectButton", _ButtonTimerSpawn);
        }
    }
    private IEnumerator ShowDialogueText()
    {
        _DialogueText.text = _DialogueString[_DialogueStringIndex];
        float localAlpha = 0;
        while (localAlpha <= 1)
        {
            if (_CaptainInPosition)
            {
                localAlpha += ((Time.deltaTime * _TransitionTextSpeed));

                _DialogueText.color = new Color(1, 1, 1, localAlpha);
            }
            yield return null;
        }

        _DialogueText.color = new Color(1, 1, 1, 1);

        Invoke("EnableCorrectButton", _ButtonTimerSpawn);
    }
    private IEnumerator HideDialogueText()
    {
        float localAlpha = 1;
        while (localAlpha >= 0)
        {
            localAlpha -= ((Time.deltaTime * _TransitionTextSpeed));

            _DialogueText.color = new Color(1, 1, 1, localAlpha);

            yield return null;
        }

        _DialogueText.color = new Color(1, 1, 1, 0);
    }
    #endregion
    #region Captain
    public void ChangeCaptainPosition(float newDistance)
    {
        _Distance = newDistance;
        StartCoroutine(MoveCaptain(_Distance));
    }
    public void ChangeCaptainTexture(int nextCaptainImageIndex)
    {
        StartCoroutine(ChangeTexture(nextCaptainImageIndex));
    }
    private IEnumerator MoveCaptain(float newDistance)
    {
        float traveledDistance = 0;
        _CaptainInPosition = false;

        if (newDistance > 0)
        {
            while (traveledDistance < Mathf.Abs(newDistance))
            {
                traveledDistance += Time.deltaTime * _ChangePositionSpeed;
                _CaptainTexture.GetComponent<RectTransform>().position = new Vector3(_CaptainTexture.GetComponent<RectTransform>().position.x + Time.deltaTime * _ChangePositionSpeed, 540, 0);
                yield return null;
            }
            _CaptainInPosition = true;
        }
        else
        {
            while (traveledDistance < Mathf.Abs(newDistance))
            {
                traveledDistance += Time.deltaTime * _ChangePositionSpeed;
                _CaptainTexture.GetComponent<RectTransform>().position = new Vector3(_CaptainTexture.GetComponent<RectTransform>().position.x - Time.deltaTime * _ChangePositionSpeed, 540, 0);
                yield return null;
            }
            _CaptainInPosition = true;
        }
        StartCoroutine(ShowDialoguePanel());
    }
    private IEnumerator ShowCaptain()
    {
        float localAlpha = 0;
        while (localAlpha <= 1)
        {
            localAlpha += ((Time.deltaTime * _TransitionPanelSpeed));

            _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, localAlpha);

            yield return null;
        }

        _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
    private IEnumerator HideCaptain()
    {
        float localAlpha = 1;
        while (localAlpha >= 0)
        {
            localAlpha -= ((Time.deltaTime * _TransitionPanelSpeed));

            _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, localAlpha);

            yield return null;
        }

        _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    private IEnumerator ChangeTexture(int nextCaptainImageIndex)
    {
        if (_CaptainTexture.GetComponent<Image>().color.a == 1)
        {
            float localAlpha = 1;
            while (localAlpha >= 0)
            {
                localAlpha -= ((Time.deltaTime * _ChangeTextureSpeed));

                _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, localAlpha);

                yield return null;
            }

            _CaptainTexture.GetComponent<Image>().sprite = _CaptainImages[nextCaptainImageIndex];

            while (localAlpha <= 1)
            {
                localAlpha += ((Time.deltaTime * _ChangeTextureSpeed));

                _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, localAlpha);

                yield return null;
            }

            _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            StartCoroutine(ShowCaptain());
        }
    }
    #endregion
}
