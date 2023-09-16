using UnityEngine;
using UnityEngine.InputSystem;
using static T4P;

public class TriggerTutorial : LevelEntity
{
    [Header("Button To Press")]
    [SerializeField] private bool _PressAny;
    [SerializeField] private bool _PressMoveUp;
    [SerializeField] private bool _PressMoveDown;
    [SerializeField] private bool _PressSpacebar;

    [Header("SFX")]
    [SerializeField] private AudioClip _SkipAudio;
    [SerializeField] private Sprite _CaptainSprite;

    [Header("Instruction")]
    [SerializeField] [TextArea(1, 10)] private string _TutorialTextToShow;
    [SerializeField] private bool _IsTutorialTriggered;

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
            ShowTutorialUI();
        }
    }

    private void ShowTutorialUI()
    {
        GameManager.Instance.LevelManager.CurrentLevel.StopLevel();
        FindObjectOfType<PlayerInput>().enabled = false;
        if (_PressSpacebar)
        {
            FindObjectOfType<PlayerShoot>().enabled = true;
        }
        GameManager.Instance.UIManager.ShowUICanvas("TutorialUI");
        GameManager.Instance.UIManager.GetComponentInChildren<TutorialUI>().UpdateTextUI(_TutorialTextToShow, _CaptainSprite);
    }

    private void HideTutorialUI()
    {
        GameManager.Instance.LevelManager.CurrentLevel.StartLevel();
        FindObjectOfType<PlayerInput>().enabled = true;
        GameManager.Instance.UIManager.HideUICanvas("TutorialUI");
    }
}
