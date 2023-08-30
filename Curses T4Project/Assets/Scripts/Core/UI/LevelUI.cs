//LevelUI.cs
//by MAURIZIO FISCHETTI

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

/// <summary>
/// Manages the Level UI
/// </summary>
public class LevelUI : MonoBehaviour
{
    public Image FlagCover;
    public Image Flag;
    public TMP_Text DoubloonsText;
    public TMP_Text CannonballsText;
    public TMP_Text LifesText;


    public void LoadFlag()
    {
        Flag.sprite = Resources.Load<Sprite>($"Flags/{GameManager.Instance.LevelManager.CurrentLevel.LevelID}");
        if (Flag.sprite == null)
        {
            Flag.sprite = Resources.Load<Sprite>($"Thumbnails/Unknown");
        }

        if (GameManager.Instance.LevelManager.CurrentLevel.LevelData.FlagObtained)
        {
            FlagCover.fillAmount = 0;
        }
        else
        {
            FlagCover.fillAmount = 1;
        }
    }

    public void UpdateLevelFlagUI()
    {
        Player player = GameManager.Instance.LevelManager.Player;
        Level currentLevel = GameManager.Instance.LevelManager.CurrentLevel;

        if (!currentLevel.LevelData.FlagObtained)
        {
            if (currentLevel.TotalFlags > 0)
            {
                float amount = 1.0f - ((float)player.NOfFlags / (float)currentLevel.TotalFlags);
                FlagCover.fillAmount = amount;
            }
        }
    }

    public void UpdateLevelLifesUI()
    {
        LifesText.text = GameManager.Instance.LevelManager.Player.Health.ToString();
    }

    public void UpdateDoubloonsUI()
    {
        DoubloonsText.text = GameManager.Instance.LevelManager.Player.NOfDoubloons.ToString();
    }

    public void UpdateCannonballsUI()
    {
        CannonballsText.text = GameManager.Instance.LevelManager.Player.NOfCannonball.ToString();
    }

    public void UpdateLevelUI()
    {
        UpdateLevelFlagUI();
        UpdateLevelLifesUI();
        UpdateCannonballsUI();
        UpdateDoubloonsUI();
    }
}
