//LevelPanel.cs
//by Maurizio Fischetti

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
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

    public Image SpriteStarCompleted;
    public Image SpriteStarAce;
    public Image SpriteStarEconomy;
    public Image SpriteFlagObtained;



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
        level.LevelData = GameManager.Instance.DataManager.LevelDatas[level.LevelID];

        SpriteStarCompleted.gameObject.SetActive(false);
        SpriteStarEconomy.gameObject.SetActive(false);
        SpriteStarEconomy.gameObject.SetActive(false);
        SpriteFlagObtained.gameObject.SetActive(false);

        //LevelData ld = GameManager.Instance.DataManager.GetLevelData(level);

        Color obtainedCol = new Color(1, 1, 1, 1);
        Color notObtainedCol = new Color(0, 0, 0, 1);
        
        string designText;
        Sprite levelThumbanil;

        if(level.LevelData.Unlocked)
        {
            LevelCompletitionText.gameObject.SetActive(false);

            designText = $"Level {level.LevelName}";
            levelThumbanil = SetLevelThumbnail(level);

            SpriteStarCompleted.gameObject.SetActive(true);
            SpriteStarAce.gameObject.SetActive(true);
            SpriteStarEconomy.gameObject.SetActive(true);
            SpriteFlagObtained.gameObject.SetActive(true);

            SpriteStarAce.GetComponent<Image>().color = level.LevelData.StarAce ? obtainedCol : notObtainedCol;
            SpriteStarEconomy.GetComponent<Image>().color = level.LevelData.StarDoubloons ? obtainedCol : notObtainedCol;
            SpriteStarCompleted.GetComponent<Image>().color = level.LevelData.StarCompleted ? obtainedCol : notObtainedCol;
            SpriteFlagObtained.GetComponent<Image>().color = level.LevelData.FlagObtained ? obtainedCol : notObtainedCol;
        }
        else
        {
            SpriteStarCompleted.gameObject.SetActive(false);
            SpriteStarAce.gameObject.SetActive(false);
            SpriteStarEconomy.gameObject.SetActive(false);
            SpriteFlagObtained.gameObject.SetActive(false);

            LevelCompletitionText.gameObject.SetActive(true);
            LevelCompletitionText.text = $"Level {level.LevelName} is locked.\nComplete the previous levels first!";
            designText = "LOCKED";
            levelThumbanil = DefaultThumbnail;
        }



        LevelDesignInfoText.text = designText;
        LevelThumbnail.sprite = levelThumbanil;

    }

    public void LevelButtonPlay()
    {
        if(GameManager.Instance.LevelManager.CurrentLevel.LevelData.Unlocked)
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
