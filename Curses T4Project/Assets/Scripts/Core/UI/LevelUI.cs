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

    public Slider DoubloonSlider;
    public Image DoubloonIcon;

    public void LoadFlag()
    {
        Level currentLevel = GameManager.Instance.LevelManager.CurrentLevel;

        Flag.sprite = currentLevel.LevelFlagTemplate.FlagSprite;

        if (Flag.sprite == null)
        {
            Flag.sprite = Resources.Load<Sprite>($"Thumbnails/Unknown");
        }

        FlagCover.fillAmount = currentLevel.LevelData.FlagObtained ? 0 : 1;
        Flag.fillAmount = currentLevel.LevelData.FlagObtained ? 1 : 0;
    }

    public void UpdateLevelFlagUI()
    {
        Player player = GameManager.Instance.LevelManager.Player;
        Level currentLevel = GameManager.Instance.LevelManager.CurrentLevel;

        if (!currentLevel.LevelData.FlagObtained)
        {
            if (currentLevel.TotalFlags > 0)
            {
                float proportion = ((float)player.NOfFlags / (float)currentLevel.TotalFlags);
                float amount = 1.0f - proportion;
                FlagCover.fillAmount = amount;
                Flag.fillAmount = proportion;
            }
        }
    }

    public void UpdateLevelLifesUI()
    {
        Player player = GameManager.Instance.LevelManager.Player;

        LifesText.text = player ? player.Health.ToString() : "0";
    }

    public void UpdateDoubloonsUI()
    {
        Player player = GameManager.Instance.LevelManager.Player;
        Level currentLevel = GameManager.Instance.LevelManager.CurrentLevel;

        if(player)
        {
            DoubloonsText.text = player.NOfDoubloons.ToString();
            if (currentLevel.TotalDoubloons > 0)
            {
                DoubloonSlider.value = ((float)player.NOfDoubloons / (float)((currentLevel.TotalDoubloons * 70) / 100));
            }
        }
        else
        {
            DoubloonsText.text = "0";
            DoubloonSlider.value = 0;
        }
        DoubloonIcon.color = new Color(DoubloonSlider.value, DoubloonSlider.value, DoubloonSlider.value, 1);


    }

    public void UpdateCannonballsUI()
    {
        Player player = GameManager.Instance.LevelManager.Player;
        CannonballsText.text = player ? player.NOfCannonball.ToString() : "0";
    }

    public void UpdateLevelUI()
    {
        UpdateLevelFlagUI();
        UpdateLevelLifesUI();
        UpdateCannonballsUI();
        UpdateDoubloonsUI();
    }
}
