using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagCollection : MonoBehaviour
{
    public GameObject FlagPanelPrefab;
    public GameObject Content;

    public Sprite DefaultFlag;
    public List<Sprite> FlagSprites = new List<Sprite>();

    public List<GameObject> LevelFlagList = new List<GameObject>();

    private void Awake()
    {
        foreach(Sprite sprite in Resources.LoadAll<Sprite>("Flags"))
        {
           FlagSprites.Add(sprite);
        }

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

            GameObject fp = Instantiate(FlagPanelPrefab, Content.transform);
            TMP_Text t = fp.transform.Find("FlagText").GetComponent<TMP_Text>();
            t.text = $"Level {level.LevelID} Flag";

            foreach(Sprite s in FlagSprites)
            {
                if (s.name == level.LevelID.ToString())
                {
                    sprite = s;
                }
            }
            if(sprite == null)
            {
                sprite = DefaultFlag;
            }

            fp.GetComponent<Image>().sprite = sprite;

            LevelFlagList.Add(fp);
            fp.SetActive(false);
        }
    }

    public void UpdateFlagCollection()
    {
        for(int i = 0; i < LevelFlagList.Count; i++)
        {
            if (GameManager.Instance.DataManager.LevelData[i].FlagObtained)
            {
                LevelFlagList[i].SetActive(true);
            }
            else
            {
                LevelFlagList[i].SetActive(false);
            }
        }
    }

}
