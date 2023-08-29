//GameManager.cs
//by MAURIZIO FISCHETTI;
using UnityEngine;
using UnityEngine.SceneManagement;
using static T4P;

/// <summary>
/// The main manager wich checks and sets the state of the game
/// </summary>
public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    //Child Managers
    public EventManager EventManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public DataManager DataManager { get; private set; }
    public LevelManager LevelManager { get; private set; }
    public Scene CurrentScene { get; private set; }

    private void Awake()
    {
        //Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance); //used in case of scenechange or reloading
        }

        Initialize();
    }

    public enum States
    {
        None,
        Initializing,
        Starting,
        Playing,
        Paused,
        Quitting,
    }
    public States GameState { get; private set; }

    /// <summary>
    /// Set the state of the game. 
    /// </summary>
    /// <param name="newState">the new State</param>
    public void SetState(States newState)
    {
        GameState = newState;
        switch (newState)
        {
            case States.None: { T4Debug.Log("Can't Set the game state to 'None'", T4Debug.LogType.Warning); return; }
            case States.Initializing: { T4Debug.Log("Can't Set the game state to 'Initialize'", T4Debug.LogType.Warning); return; }
            case States.Starting: { StateStarting(); return; }
            case States.Playing: { StatePlaying(); return; }
            case States.Paused: { StatePause(); return; }
            case States.Quitting: { StateQuitting(); return; }
        }
    }


    private bool Initialize()
    {
        
        GameState = States.Initializing;
        //Initialize other managers
        DataManager = GetComponentInChildren<DataManager>();
        EventManager = GetComponentInChildren<EventManager>();
        UIManager = GetComponentInChildren<UIManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        LevelManager = GetComponentInChildren<LevelManager>();
        LoadGame();

        return true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SetState(States.Starting);
    }

    #region States Methods
    private void StateStarting()
    {
        //operations to do after the GameManager is fully instantiated
        T4Debug.Log("[GameManager] Game started!");

        EventManager.RaiseOnGameStart();
        SetState(States.Playing);
    }
    private void StatePlaying()
    {
        UIManager.HideUICanvas("PauseMenuUI");
        AudioManager.AudioSourceMusic.UnPause();
        Time.timeScale = 1;
        //operations to do after game state switch to playing


        //raise unpause events
        EventManager.RaiseOnGameUnpause();
    }
    private void StatePause()
    {
        UIManager.ShowUICanvas("PauseMenuUI");
        AudioManager.AudioSourceMusic.Pause();
        Time.timeScale = 0;

        //raise pause events
        EventManager.RaiseOnGamePause();
    }
    private void StateQuitting()
    {
        //operation to do before the game application quit
        SaveGame();

        //raise quit event
        EventManager.RaiseOnGameQuit();

        //close the Application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    #endregion


    #region Save & Load Game
    private void SaveGame()
    {
        DataManager.Save(DataManager.GameData);
    }
    private void LoadGame()
    {
        DataManager.Load();
    }
    #endregion

    #region Scene Management
    /// <summary>
    /// Load the scene with the given name.
    /// </summary>
    /// <param name="sceneToLoad">name of the scene</param>
    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentScene = scene;
        Time.timeScale = 1;

        T4Debug.Log($"[GameManager] Scene '{scene.name}' loaded.");


        if(scene.name == "MainMenu")
        {
            AudioManager.PlayMusic("MainMenuMusic");
            if (UIManager.CanvasToShow != null)
            {
                UIManager.ShowUICanvasOnLoad();
            }
            else
            {
                UIManager.ShowUICanvasOnly("MainMenuUI");
            }
            UIManager.FlagsCollection.UpdateFlagCollection();


            LevelManager.CurrentLevel = LevelManager.LevelToLoad.GetComponent<Level>();
            UIManager.LevelPanelSelection.UpdateLevelPanel(LevelManager.CurrentLevel);
        }
        if(scene.name == "Level")
        {
            UIManager.HideAllUICanvas();
            UIManager.ShowUICanvasOnly("LevelUI");
            LevelManager.LoadLevel(LevelManager.LevelToLoad);

            UIManager.StageCompleteUI.UpdateStageCompleteUI();
            AudioManager.PlayMusic("LevelMusic");
        }
    }
    #endregion

    /// <summary>
    /// Set the state in pause if the game is in playing state
    /// or viceversa.
    /// </summary>
    public void PauseUnpauseGame()
    {
        if(GameState == States.Paused)
        {
            SetState(States.Playing);
        }
        else
        {
            SetState(States.Paused);
        }
    }

    public void QuitGame()
    {
        SetState(States.Quitting);
    }
}
