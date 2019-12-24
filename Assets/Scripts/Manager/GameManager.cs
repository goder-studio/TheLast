using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if(_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            return _instance;
        }
    }

    public LoadingPanel loadingPanel;
    public Texture2D img_Cursor;
    public LevelMgr levelMgr;

    private GameConfiguration gameConfiguration;
    public GameConfiguration GameConfig
    {
        get { return gameConfiguration; }
    }

    public bool isPauseGame = false;

    private bool[] levelPass;
    private bool[] levelActive;

    private void Awake()
    {
        InitGameConfiguration();
        levelMgr.Init();

        //服务模块初始化
        TimerSvc timerSvc = GetComponent<TimerSvc>();
        timerSvc.InitSvc();

        ResSvc resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();

        AudioSvc audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();

        //开始系统初始化
        StartSys startSys = GetComponent<StartSys>();
        startSys.InitSys();

        //对话系统初始化
        DialogueSys dialogueSys = GetComponent<DialogueSys>();
        dialogueSys.InitSys();

        //战斗系统初始化
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        Cursor.SetCursor(img_Cursor, Vector2.zero, CursorMode.Auto);

        DontDestroyOnLoad(this);

        SceneManager.LoadScene(Constant.SceneMainID);
        startSys.EnterStart();
    }

    private void Update()
    {

    }

    public bool[] GetLevelPassArr()
    {
        return levelPass;
    }

    public bool[] GetLevelActiveArr()
    {
        return levelActive;
    }

    public void UpdateLevelActiveArr()
    {
        int index = BattleSys.Instance.CurLevel.levelID + 1;
        if (index < levelActive.Length && levelActive[index] != true)
        {
            levelActive[index] = true;
            levelMgr.UpdateBaseLevel((LevelType)index, true, false);
            SaveGameConfigurationByBinary();
        }
    }

    public void UpdateLevelPassArr()
    {
        int index = BattleSys.Instance.CurLevel.levelID;
        if (index < levelPass.Length && levelPass[index] != true)
        {
            levelPass[index] = true;
            levelMgr.UpdateBaseLevel((LevelType)index, true, true);
            SaveGameConfigurationByBinary();
        }
    }

    #region Cursor Operation
    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion



    #region GameConfiguration Operation
    public void InitGameConfiguration()
    {
        LoadGameConfigurationByBinary();
        //如果没有配置文件
        if(gameConfiguration == null)
        {
            AudioSvc.Instance.isActiveBgMusic = true;
            AudioSvc.Instance.isActiveEffectSound = true;
            AudioSvc.Instance.bgMusicAudio.volume = 1.0f;
            AudioSvc.Instance.effectSoundAudio.volume = 1.0f;
            levelActive = new bool[Constant.levelCount];
            levelActive[0] = true;
            levelPass = new bool[Constant.levelCount];
            SaveGameConfigurationByBinary();
            //重新加载
            LoadGameConfigurationByBinary();
        }
        else
        {
            AudioSvc.Instance.isActiveBgMusic = gameConfiguration.isActiveBgMusic;
            AudioSvc.Instance.isActiveEffectSound = gameConfiguration.isActiveEffectSound;
            AudioSvc.Instance.bgMusicAudio.volume = gameConfiguration.bgMusicVolumn;
            AudioSvc.Instance.effectSoundAudio.volume = gameConfiguration.effectSoundVolumn;
            levelPass = gameConfiguration.levelPass;
            levelActive = gameConfiguration.levelActive;
        }
    }

    #region 二进制存储
    public void SaveGameConfigurationByBinary()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/GameConfiguration.txt"))
            {
                gameConfiguration = new GameConfiguration();
                gameConfiguration.isActiveBgMusic = AudioSvc.Instance.isActiveBgMusic;
                gameConfiguration.isActiveEffectSound = AudioSvc.Instance.isActiveEffectSound;
                gameConfiguration.bgMusicVolumn = AudioSvc.Instance.bgMusicAudio.volume;
                gameConfiguration.effectSoundVolumn = AudioSvc.Instance.effectSoundAudio.volume;
                gameConfiguration.levelPass = levelPass;
                gameConfiguration.levelActive = levelActive;

                bf.Serialize(fs, gameConfiguration);
            }
            
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LoadGameConfigurationByBinary()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.streamingAssetsPath + "/GameConfiguration.txt", FileMode.Open))
            {
                gameConfiguration = (GameConfiguration)bf.Deserialize(fs);
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    #endregion

    #region XML存储
    public void SaveGameConfigurationByXml()
    {
        try
        {
            XmlSerializer xmlSerilize = new XmlSerializer(typeof(GameConfiguration));
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/GameConfiguration.txt"))
            {
                gameConfiguration = new GameConfiguration();
                gameConfiguration.isActiveBgMusic = AudioSvc.Instance.isActiveBgMusic;
                gameConfiguration.isActiveEffectSound = AudioSvc.Instance.isActiveEffectSound;
                gameConfiguration.bgMusicVolumn = AudioSvc.Instance.bgMusicAudio.volume;
                gameConfiguration.effectSoundVolumn = AudioSvc.Instance.effectSoundAudio.volume;

                xmlSerilize.Serialize(fs, gameConfiguration);
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LoadGameConfigurationByXml()
    {
        try
        {
            XmlSerializer xmlSerilize = new XmlSerializer(typeof(GameConfiguration));
            using (FileStream fs = File.Open(Application.streamingAssetsPath + "/GameConfiguration.txt", FileMode.Open))
            {
                gameConfiguration = (GameConfiguration)xmlSerilize.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #endregion
    #endregion

}
