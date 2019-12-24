using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSys : SystemRoot
{
    #region 单例模式
    private static StartSys _instance = null;
    public static StartSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<StartSys>();
            }
            return _instance;
        }
    }
    #endregion

    public StartPanel startPanel;
    public InstructionPanel instructionPanel;
    public SettingPanel settingPanel;
    public ChooseLevelPanel chooseLevelPanel;

    public override void InitSys()
    {
        base.InitSys();
        Debug.Log("Init StartSys Done");
    }

    public void EnterStart()
    {
        startPanel.SetWindowState(true);
        audioSvc.PlayBgMusic(PathDefine.bgMusic);
        if (!audioSvc.isActiveBgMusic)
        {
            audioSvc.InActiveBgMusic();
        }
        
    }

    public void OpenInstructionPanel()
    {
        if(settingPanel.gameObject.activeInHierarchy)
        {
            settingPanel.SetWindowState(false);
        }
        if(chooseLevelPanel.gameObject.activeInHierarchy)
        {
            chooseLevelPanel.SetWindowState(false);
        }
        if(!instructionPanel.gameObject.activeInHierarchy)
        {
            instructionPanel.SetWindowState(true);
        }

    }

    public void OpenSettingPanel()
    {
        if (instructionPanel.gameObject.activeInHierarchy)
        {
            instructionPanel.SetWindowState(false);
        }
        if (chooseLevelPanel.gameObject.activeInHierarchy)
        {
            chooseLevelPanel.SetWindowState(false);
        }
        if (!settingPanel.gameObject.activeInHierarchy)
        {
            settingPanel.SetWindowState(true);
        }
    }

    public void OpenChooseLevelPanel()
    {
        if (instructionPanel.gameObject.activeInHierarchy)
        {
            instructionPanel.SetWindowState(false);
        }
        if (settingPanel.gameObject.activeInHierarchy)
        {
            settingPanel.SetWindowState(false);
        }
        if (!chooseLevelPanel.gameObject.activeInHierarchy)
        {
            chooseLevelPanel.SetWindowState(true);
        }

    }

    public void HideAllPanel()
    {
        startPanel.SetWindowState(false);
        settingPanel.SetWindowState(false);
        instructionPanel.SetWindowState(false);
        chooseLevelPanel.SetWindowState(false);
    }
}
