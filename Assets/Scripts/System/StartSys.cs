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
    }

    public void OpenInstructionPanel()
    {
        settingPanel.SetWindowState(false);
        instructionPanel.SetWindowState(true);
    }

    public void OpenSettingPanel()
    {
        instructionPanel.SetWindowState(false);
        settingPanel.SetWindowState(true);
    }
}
