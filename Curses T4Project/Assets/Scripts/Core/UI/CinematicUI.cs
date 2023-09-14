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

    [Header("Dialogue - Captain")]
    [SerializeField] private float _ChangeTextureSpeed = 0.8f;
    [SerializeField] private GameObject _CaptainTexture;
    [SerializeField] private List<Sprite> _CaptainImages;
    [SerializeField] private float _startDistance = 100;
    [SerializeField] private float _ChangePositionSpeed = 0.8f;
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
    [SerializeField] private int _DialogueStringIndex;
    [SerializeField][Multiline(3)] private List<string> _DialogueString;

    private void Start()
    {
        ResetValue();
    }

    private void OnEnable()
    {
        ResetValue();
    }

    private void ResetValue()
    {
        _DialogueStringIndex = 0;

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
        StartCoroutine(MoveCaptain(_startDistance));
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

        gameObject.SetActive(false);
    }
    #endregion

    #region DialoguePanel
    private void EnableCorrectButton()
    {
        _NextDialogueButton[_DialogueStringIndex].gameObject.SetActive(true);
    }

    public void ActivateDeactivePanelButton()
    {
        if (_DialoguePanel.GetComponent<Image>().color.a > 0.5)
        {
            CancelInvoke("EnableCorrectButton");
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
    public void NextDialogButton()
    {
        StartCoroutine(DialogueSwitch());
    }
    private IEnumerator DialogueSwitch()
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

    private IEnumerator ShowDialogueText()
    {
        _DialogueText.text = _DialogueString[_DialogueStringIndex];
        Debug.Log(_DialogueText.text.ToString());
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
    /// <summary>
    /// End the Level with the given outcome
    /// </summary>
    /// <param name="_distance"> positive go right, negative go left</param>

    public void ChangeCaptainPosition(float _distance)
    {
        StartCoroutine(MoveCaptain(_distance));
    }

    private IEnumerator MoveCaptain(float _nextDistance)
    {
        float traveledDistance = 0;
        _CaptainInPosition = false;

        if (_nextDistance > 0)
        {
            while (traveledDistance < Mathf.Abs(_nextDistance))
            {
                traveledDistance += Time.deltaTime * _ChangePositionSpeed;
                _CaptainTexture.GetComponent<RectTransform>().position = new Vector3(_CaptainTexture.GetComponent<RectTransform>().position.x + Time.deltaTime * _ChangePositionSpeed, 540, 0);
                yield return null;
            }
            _CaptainInPosition = true;
        }
        else
        {
            while (traveledDistance < Mathf.Abs(_nextDistance))
            {
                traveledDistance += Time.deltaTime * _ChangePositionSpeed;
                _CaptainTexture.GetComponent<RectTransform>().position = new Vector3(_CaptainTexture.GetComponent<RectTransform>().position.x - Time.deltaTime * _ChangePositionSpeed, 540, 0);
                yield return null;
            }
            _CaptainInPosition = true;
        }
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
    private IEnumerator ChangeCaptainTexture(int _nextCaptainImageIndex)
    {
        if (_CaptainTexture.GetComponent<Image>().color.a == 1)
        {
            float localAlpha = 1;
            while (localAlpha >= 0)
            {
                localAlpha -= ((Time.deltaTime * _TransitionPanelSpeed));

                _CaptainTexture.GetComponent<Image>().color = new Color(1, 1, 1, localAlpha);

                yield return null;
            }

            _CaptainTexture.GetComponent<Image>().sprite = _CaptainImages[_nextCaptainImageIndex];

            while (localAlpha <= 1)
            {
                localAlpha += ((Time.deltaTime * _TransitionPanelSpeed));

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
