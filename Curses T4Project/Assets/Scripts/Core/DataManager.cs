//DataManager.cs
//by MAURIZIO FISCHETTI
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using static T4P;

/// <summary>
/// DataManager: responsible of the datas savings and loading on multiplatform system.
/// </summary>
public class DataManager : MonoBehaviour
{

    [SerializeField] private bool _IsEnabled = true;
    public bool IsEnabled { get { return _IsEnabled; } }

    [SerializeField]private string _FileFolder = "Data";
    [SerializeField]private string _FileName = "SaveFile.TDSAVE";
    private string _FullPath = "";

    [SerializeField] private string _LevelFileFolder = "Levels";
    private string[] _LevelFileName;
    private string[] _LevelFullPath;


    [SerializeField]private bool _CryptData = false;
    [SerializeField]private string _CryptKey = "T4TheDrowned";

    public GameData GameData;

    public List<Level> LevelList = new List<Level>();
    public List<LevelData> LevelData = new List<LevelData>();


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_IsEnabled)
        {
            //initialize blank datas
            GameData = new GameData();
            
            //generic patch of the system in use (windows, linux, iOs, Android etc...) + folder + filename
            _FullPath = Path.Combine(Application.persistentDataPath, _FileFolder, _FileName);

            //check if the file exists, if not create a new one, if yes load it.
            if (File.Exists(_FullPath))
            {
                GameData = Load();
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, _FileFolder));
                Save(GameData);
            }



            //Levels
            _LevelFileName = new string[Resources.LoadAll<GameObject>("Levels/").Length];
            _LevelFullPath = new string[Resources.LoadAll<GameObject>("Levels/").Length];

            for (int i = 0; i < Resources.LoadAll<GameObject>("Levels/").Length; i++)
            {
                Level level = Resources.LoadAll<GameObject>("Levels/")[i].GetComponent<Level>();
                LevelList.Add(level);
                LevelData.Add(new LevelData());

                LevelData[i].LevelID = LevelList[i].LevelID;
                LevelData[i].LevelName = LevelList[i].LevelName;
                LevelData[i].LevelDesigner = LevelList[i].LevelDesigner;

                _LevelFileName[i] = $"{LevelList[i].LevelID}.TDLEVEL";
                _LevelFullPath[i] = Path.Combine(Application.persistentDataPath, _FileFolder, _LevelFileFolder, _LevelFileName[i]);

                if (File.Exists(_LevelFullPath[i]))
                {
                    LevelData[i] = LoadLevel(i);
                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, _FileFolder, _LevelFileFolder));
                    SaveLevel(LevelData[i].LevelID);
                }
            }         
        }
    }


    public LevelData GetLevelData(Level level)
    {
        foreach (LevelData levelData in LevelData)
        {
            if(level.LevelID ==  levelData.LevelID)
            {
                return levelData;
            }
        }
        T4Debug.Log($"[Data Manager] Cannot find LevelData for {level.name}", T4Debug.LogType.Warning);
        return null;
    }

    /// <summary>
    /// Save the game
    /// </summary>
    public void Save(GameData gd)
    {
        if (_IsEnabled)
        {
            T4Debug.Log("[Data Manager] Saving...");
            T4Debug.Log($"[Data Manager] Saving Path: {_FullPath}");
            string dataString = JsonUtility.ToJson(gd, true);

            if (_CryptData)
            {
                dataString = DeEncrypt(dataString);
            }

            FileStream fs = new FileStream(_FullPath, FileMode.Create, FileAccess.Write);

            if (fs.CanWrite)
            {
                byte[] byteString = Encoding.Default.GetBytes(dataString);
                fs.Write(byteString);
            }

            fs.Flush(); //clears the buffer (clear the memory used for writing the file)
            fs.Close();
        }
    }

    public void SaveLevel(int id)
    {
        if (_IsEnabled)
        {
            string dataString = JsonUtility.ToJson(LevelData[id], true);
            if(_CryptData)
            {
                dataString = DeEncrypt(dataString);
            }

            FileStream fs = new FileStream(_LevelFullPath[id], FileMode.Create, FileAccess.Write);
            if(fs.CanWrite)
            {
                byte[] byteString = Encoding.Default.GetBytes(dataString);
                fs.Write(byteString);
            }
            fs.Flush();
            fs.Close();
        }
    }

    public LevelData LoadLevel(int id)
    {
        if (_IsEnabled)
        {

            if (File.Exists(_LevelFullPath[id]))
            {
                string dataString = "";
                LevelData loadedData;

                FileStream fs = new FileStream(_LevelFullPath[id], FileMode.Open, FileAccess.Read);
                if(fs.CanRead)
                {
                    byte[] byteString = new byte[fs.Length];
                    int readedBytes = fs.Read(byteString);
                    dataString = Encoding.Default.GetString(byteString, 0, readedBytes);
                }
                    
                fs.Close();

                if(_CryptData)
                {
                    dataString = DeEncrypt(dataString);
                }

                loadedData = JsonUtility.FromJson<LevelData>(dataString);

                return loadedData;
            }
        }
        return null;
        
    }

    /// <summary>
    /// Load the game
    /// </summary>
    public GameData Load()
    {
        if (_IsEnabled)
        {
            if (File.Exists(_FullPath))
            {
                T4Debug.Log("[Data Manager] Loading...");
                T4Debug.Log($"[Data Manager] Loading Path: {_FullPath}");
                string dataString = "";
                GameData loadedData;

                //read from file
                FileStream fs = new FileStream(_FullPath, FileMode.Open, FileAccess.Read);
                if (fs.CanRead)
                {
                    byte[] byteString = new byte[fs.Length];
                    int readedBytes = fs.Read(byteString);
                    dataString = Encoding.Default.GetString(byteString, 0, readedBytes);
                }
                else { T4Debug.Log($"[DataManager] cannot read '{_FullPath}'"); }
                fs.Close();

                //decrypt after read from file
                if (_CryptData)
                {
                    dataString = DeEncrypt(dataString);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataString);

                return loadedData;
            }
            return null;
        }
        return null;
    }

    private string DeEncrypt(string fileDatas)
    {
        //encrypt / decrypt the data
        //It works with a simple XOR operation through the keyword.
        //note: break throught it, it's simple for people that have a little knowledge of CyberSec
        //(weakness #1: all the info like key and crypt method are stored on clientside, weakness #2: symmetric key)
        string result = "";
        for(int i = 0;  i < fileDatas.Length; i++)
        {
            result += (char)(fileDatas[i] ^ _CryptKey[i%_CryptKey.Length]);
        }
        return result;
    }
}
