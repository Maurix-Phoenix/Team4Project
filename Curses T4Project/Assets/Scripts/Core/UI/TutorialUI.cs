using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI TutorialText;

    public void UpdateTextUI(string newText)
    {
        TutorialText.text = newText;
    }
}
