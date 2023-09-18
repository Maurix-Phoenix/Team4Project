//StageCompleteUI.cs
//by MAURIZIO FISCHETTI

using UnityEngine;
using UnityEngine.UI;

public class StageCompleteUI : MonoBehaviour
{
    public GameObject ButtonPrevious;
    public GameObject ButtonNext;

    public Image Flag;
    public Image StarComplete;
    public Image StarAce;
    public Image StarEconomy;
    public void LoadFlag()
    {
        Flag.sprite = GameManager.Instance.LevelManager.CurrentLevel.LevelFlagTemplate.FlagSprite;
        if (Flag.sprite == null)
        {
            Flag.sprite = Resources.Load<Sprite>($"Thumbnails/Unknown");
        } 
    }

    public void UpdateStageCompleteUI()
    {
        LevelData levelD = GameManager.Instance.LevelManager.CurrentLevel.LevelData;
        Flag.gameObject.SetActive(levelD.FlagObtained);
        StarComplete.gameObject.SetActive(levelD.StarCompleted);
        StarAce.gameObject.SetActive(levelD.StarAce);
        StarEconomy.gameObject.SetActive(levelD.StarDoubloons);

        ButtonPrevious.SetActive(false);
        ButtonNext.SetActive(false);

        if (GameManager.Instance.LevelManager.CurrentLevel.LevelID > 0)
        { 
            ButtonPrevious.SetActive(true);
        }
        if(GameManager.Instance.LevelManager.CurrentLevel.LevelID < GameManager.Instance.LevelManager.LevelPrefabsList.Count -1)
        {
            ButtonNext.SetActive(true);
        }
    }
}
