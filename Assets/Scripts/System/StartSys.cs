using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public override void InitSys()
    {
        base.InitSys();
        Debug.Log("Init StartSys Done");
    }

    public void EnterStart()
    {
        startPanel.SetWindowState(true);
        audioSvc.PlayBgMusic(PathDefine.bgMusic);
    }

    public void OpenInstructionPanel()
    {
        if(settingPanel.gameObject.activeInHierarchy)
        {
            settingPanel.SetWindowState(false);
        }
        if(!instructionPanel.gameObject.activeInHierarchy)
        {
            instructionPanel.SetWindowState(true);
        }

    }

    public void OpenSettingPanel()
    {
        if(!settingPanel.gameObject.activeInHierarchy)
        {
            settingPanel.SetWindowState(true);
        }
        if(instructionPanel.gameObject.activeInHierarchy)
        {
            instructionPanel.SetWindowState(false);
        }
    }
}
