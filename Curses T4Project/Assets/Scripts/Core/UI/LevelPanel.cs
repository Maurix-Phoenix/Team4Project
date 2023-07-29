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
        //LevelThumbanilImage = null; //TODO - ADD the thumbnail image of the level
        LevelData ld = GameManager.Instance.DataManager.GetLevelData(level);
        LevelThumbnail.sprite = SetLevelThumbnail(level);
        LevelDesignInfoText.text = $"{level.LevelID}-{level.LevelName}\nby {level.LevelDesigner}";        

        string flagString;

        if (ld.FlagObtained)
        {
            flagString = "v";
        }
        else
        {
            flagString = "x";
        }

        LevelCompletitionText.text = $"Stars {ld.StarsObtained}/3\tFlag [{flagString}]";
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
