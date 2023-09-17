using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static T4P;

public class TriggerTutorial : LevelEntity
{
    [Header("SFX")]
    [SerializeField] private AudioClip _SkipAudio;
    [SerializeField] private Sprite _CaptainSprite;

    [Header("Button To Press")]
    [SerializeField] private bool _PressAny = true;
    [SerializeField] private bool _PressMoveUp;
    [SerializeField] private bool _PressMoveDown;
    [SerializeField] private bool _PressSpacebar;


    [Header("Tutorial")]
    [SerializeField] private bool _IsTutorialText = true;
    [SerializeField] private bool _RestartTutorial = false;
    [SerializeField] private bool _IsTutorialTriggered;
    [SerializeField] [TextArea(1,10)] private string _StandardText;

    protected override void Start()
    {
        base.Start();
        CheckRequirement();
        FindObjectOfType<PlayerInput>().enabled = false;
        FindObjectOfType<PlayerShoot>().enabled = false;
    }
    protected override void Update()
    {
        base.Update();
        CheckInput();
    }

    private void CheckRequirement()
    {
        int requirement = 0;
        if (_PressAny)
        {
            requirement++;
        }
        if (_PressMoveDown)
        {
            requirement++;
        }
        if (_PressMoveDown)
        {
            requirement++;
        }
        if (_PressSpacebar)
        {
            requirement++;
        }

        if (requirement <= 0)
        {
            T4Debug.Log($"No requirement assigned for the {gameObject.name}");
        }
    }

    private void CheckInput()
    {
        if (_IsTutorialTriggered)
        {
            if (_PressMoveUp && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                GameManager.Instance.AudioManager.PlaySFX(_SkipAudio);
                HideTutorialUI();
                _IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }

            if (_PressMoveDown && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                GameManager.Instance.AudioManager.PlaySFX(_SkipAudio);
                HideTutorialUI();
                _IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }

            if (_PressAny && Input.anyKeyDown)
            {
                GameManager.Instance.AudioManager.PlaySFX(_SkipAudio);
                HideTutorialUI();
                _IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }

            if (_PressSpacebar && Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.AudioManager.PlaySFX(_SkipAudio);
                HideTutorialUI();
                _IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _IsTutorialTriggered = true;
            if (_PressSpacebar)
            {
                FindObjectOfType<PlayerShoot>().enabled = true;
            }
            ShowTutorialUI();
        }
    }

    private void ShowTutorialUI()
    {
        FindObjectOfType<PlayerInput>().enabled = false;
        GameManager.Instance.LevelManager.CurrentLevel.StopLevel();
        GameManager.Instance.UIManager.ShowUICanvasOnly("TutorialUI");
        if (_IsTutorialText)
        {
            GameManager.Instance.UIManager.TutorialUI.UpdateTutorialTextUI(_CaptainSprite, _RestartTutorial);
        }
        else
        {
            GameManager.Instance.UIManager.TutorialUI.UpdateStandardTextUI(_CaptainSprite, _StandardText);
        }
    }

    private void HideTutorialUI()
    {
        GameManager.Instance.LevelManager.CurrentLevel.StartLevel();
        FindObjectOfType<PlayerInput>().enabled = true;
        GameManager.Instance.UIManager.ShowUICanvasOnly("LevelUI");
    }
}
