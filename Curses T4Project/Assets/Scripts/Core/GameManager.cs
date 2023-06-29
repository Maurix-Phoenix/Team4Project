//GameManager.cs
//by MAURIZIO FISCHETTI
using UnityEngine;
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

    private void Awake()
    {
        //Singleton
        if(_instance != null && _instance != this)
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
        switch (newState)
        {
            case States.None: { T4Debug.Log("Can't Set the game state to 'None'", T4Debug.LogType.Warning); return; }
            case States.Initializing: { T4Debug.Log("Can't Set the game state to 'Initialize'", T4Debug.LogType.Warning); return;}
            case States.Starting: { StateStarting(); return; }
            case States.Playing: { StatePlaying();  return; }
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

        LoadGame();

        return true;
    }

    private void Start()
    {
        SetState(States.Starting);
        
    }

    #region States Methods
    private void StateStarting()
    {
        //operations to do after the GameManager is fully instantiated
        T4Debug.Log("GameManager: Game started!");

        EventManager.RaiseOnGameStart();
        StatePlaying();
    }
    private void StatePlaying()
    {
        //operations to do after game state switch to playing

        //raise unpause events
        EventManager.RaiseOnGameUnpause();
    }
    private void StatePause()
    {
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
}
