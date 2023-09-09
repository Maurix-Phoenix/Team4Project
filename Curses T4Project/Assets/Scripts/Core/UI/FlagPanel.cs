using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagPanel : MonoBehaviour
{
    public FlagTemplate FT;

    public Image FlagSprite;
    public TMP_Text FlagLabel;


    public void FlagButtonClick()
    {
       GameManager.Instance.UIManager.FlagsCollection.UpdateDescription(FT);
       GameManager.Instance.UIManager.ShowUICanvas("FlagDescriptionUI",false);
    }
}
