using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TutorialText;
    [SerializeField] private Image _CaptainImage;

    public void UpdateTextUI(string newText, Sprite CaptainSprite)
    {
        _CaptainImage.sprite = CaptainSprite;
        _TutorialText.text = newText;
    }
}
