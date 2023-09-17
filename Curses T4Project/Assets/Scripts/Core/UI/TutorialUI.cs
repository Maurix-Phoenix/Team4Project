using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TutorialText;
    [SerializeField] private int _TutorialTextIndex;
    [SerializeField] [TextArea(1,10)] private List<string> _TutorialTexts;
    [SerializeField] private Image _CaptainImage;

    public void UpdateTutorialTextUI(Sprite CaptainSprite, bool restartTutorial)
    {
        if (restartTutorial)
        {
            _TutorialTextIndex = 0;
        }
        _CaptainImage.sprite = CaptainSprite;
        _TutorialText.text = _TutorialTexts[_TutorialTextIndex];
        _TutorialTextIndex++;
    }

    public void UpdateStandardTextUI(Sprite CaptainSprite, string TextToShow)
    {
        _CaptainImage.sprite = CaptainSprite;
        _TutorialText.text = TextToShow;
    }
}
