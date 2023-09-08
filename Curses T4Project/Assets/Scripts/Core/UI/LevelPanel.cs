//LevelPanel.cs
//by Maurizio Fischetti

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    public Image LevelThumbnail;
    public TMP_Text LevelDesignInfoText;
    public TMP_Text LevelCompletitionText;

    public Sprite DefaultThumbnail;
    public List<Sprite> LevelThumbailsList = new List<Sprite>();


    private void Awake()
    {
        foreach(Sprite thumbnail in Resources.LoadAll<Sprite>("LevelsThumbnails"))
        {
            LevelThumbailsList.Add(thumbnail);
        }
    }
    private void Start()
    {
        //T4P.T4Debug.Log(GameManager.Instance.LevelManager.LevelToLoad.GetComponent<Level>().LevelName); //this works
        UpdateLevelPanel(GameManager.Instance.LevelManager.LevelToLoad.GetComponent<Level>());
    }

    public void UpdateLevelPanel(Level level)
    {
        LevelData ld = GameManager.Instance.DataManager.GetLevelData(level);
        

        string flagString;
        string completitionText;
        string designText;
        Sprite levelThumbanil;

        if(level.IsUnlocked)
        {
            flagString = ld.FlagObtained ? "v" : "x";
            designText = $"{level.LevelID}-{level.LevelName}";
            completitionText = $"Stars {ld.StarsObtained}/3\tFlag [{flagString}]";
            levelThumbanil = SetLevelThumbnail(level);
        }
        else
        {
            completitionText = "LOCKED\nComplete the previous levels first!";
            designText = "LOCKED";
            levelThumbanil = DefaultThumbnail;
        }

        LevelCompletitionText.text = completitionText;
        LevelDesignInfoText.text = designText;
        LevelThumbnail.sprite = levelThumbanil;

    }

    public void LevelButtonPlay()
    {
        if(GameManager.Instance.LevelManager.CurrentLevel.IsUnlocked)
        {
            GameManager.Instance.AudioManager.PlaySFX("UISFX_Click1");
            GameManager.Instance.LoadScene("Level");
        }
        else
        {
            GameManager.Instance.AudioManager.PlaySFX("UISFX_ClickNotValid");
        }
    }

    private Sprite SetLevelThumbnail(Level level)
    {
        Sprite Thumbnail = null;
        if (level.LevelThumbnail == null)
        {
            foreach (Sprite sprite in LevelThumbailsList)
            {
                if (sprite.name == level.LevelID.ToString())
                {
                    Thumbnail = sprite;
                    return sprite;
                }
            }
            if(Thumbnail == null)
            {
                Thumbnail = DefaultThumbnail;
                return Thumbnail;
            }
        }
        else
        {
            Thumbnail = level.LevelThumbnail;
            return Thumbnail;
        }
        return null;
    }
}
