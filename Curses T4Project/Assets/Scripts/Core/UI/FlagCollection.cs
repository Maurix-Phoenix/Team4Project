using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlagCollection : MonoBehaviour
{
    public GameObject FlagPanelPrefab;
    public GameObject Content;
    public Sprite DefaultFlag;

    public TMP_Text currentFlagName;
    public TMP_Text currentFlagDescription;
    public Image currentFlagImage;

    public List<FlagPanel> LevelFlagList = new List<FlagPanel>();

    private void Awake()
    {
        InitializeFlagsCollection();
    }

    private void Start()
    {
        UpdateFlagCollection();
    }

    private void InitializeFlagsCollection()
    {

        foreach(GameObject levelobj in GameManager.Instance.LevelManager.LevelPrefabsList)
        {
            Sprite sprite = null;
            Level level = levelobj.GetComponent<Level>();

            GameObject fpObj = Instantiate(FlagPanelPrefab, Content.transform);
            FlagPanel flagPanel = fpObj.GetComponent<FlagPanel>();
            flagPanel.FT = level.LevelFlagTemplate;
            flagPanel.FlagLabel.text = $"{level.LevelFlagTemplate.FlagID}. {level.LevelFlagTemplate.FlagName}";

            sprite = level.LevelFlagTemplate.FlagSprite;
            if(sprite == null)
            {
                sprite = DefaultFlag;
            }

            flagPanel.FlagSprite.sprite = sprite;

            LevelFlagList.Add(flagPanel);
            fpObj.SetActive(false);
        }
    }

    public void UpdateFlagCollection()
    {
        for(int i = 0; i < LevelFlagList.Count; i++)
        {
            if (GameManager.Instance.DataManager.LevelDatas[i].FlagObtained)
            {
                LevelFlagList[i].gameObject.SetActive(true);
            }
            else
            {
                LevelFlagList[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateDescription(FlagTemplate flagT)
    {
        if(flagT == null)
        {
            currentFlagImage.sprite = DefaultFlag;
            currentFlagName.text = "Flag 404";
            currentFlagDescription.text = "If you are reading this something went wrong...";
        }


        currentFlagImage.sprite = flagT.FlagSprite;
        currentFlagName.text = flagT.FlagName;
        currentFlagDescription.text = flagT.FlagDescription;
    }

}
