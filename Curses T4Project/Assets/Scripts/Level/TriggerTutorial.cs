using UnityEngine;
using UnityEngine.InputSystem;
using static T4P;

public class TriggerTutorial : LevelEntity
{
    [Header("Button To Press")]
    [SerializeField] private bool PressEsc;
    [SerializeField] private bool PressMoveUp;
    [SerializeField] private bool PressMoveDown;
    [SerializeField] private bool PressSpacebar;

    [Header("Instruction")]
    [SerializeField][TextArea(1, 10)] private string TutorialTextToShow;
    [SerializeField] private bool IsTutorialTriggered;

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
        if (PressEsc)
        {
            requirement++;
        }
        if (PressMoveDown)
        {
            requirement++;
        }
        if (PressMoveDown)
        {
            requirement++;
        }
        if (PressSpacebar)
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
        if (IsTutorialTriggered)
        {
            if (PressMoveUp && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                HideTutorialUI();
                IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }

            if (PressMoveDown && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                HideTutorialUI();
                IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }

            if (PressEsc && Input.GetKeyDown(KeyCode.Escape))
            {
                HideTutorialUI();
                IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }

            if (PressSpacebar && Input.GetKeyDown(KeyCode.Space))
            {
                HideTutorialUI();
                IsTutorialTriggered = false;
                gameObject.SetActive(false);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IsTutorialTriggered = true;
            ShowTutorialUI();
        }
    }

    private void ShowTutorialUI()
    {
        GameManager.Instance.LevelManager.CurrentLevel.StopLevel();
        FindObjectOfType<PlayerInput>().enabled = false;
        if (PressSpacebar)
        {
            FindObjectOfType<PlayerShoot>().enabled = true;
        }
        GameManager.Instance.UIManager.ShowUICanvas("TutorialUI");
        GameManager.Instance.UIManager.GetComponentInChildren<TutorialUI>().UpdateTextUI(TutorialTextToShow);
    }

    private void HideTutorialUI()
    {
        GameManager.Instance.LevelManager.CurrentLevel.StartLevel();
        FindObjectOfType<PlayerInput>().enabled = true;
        GameManager.Instance.UIManager.HideUICanvas("TutorialUI");
    }
}
