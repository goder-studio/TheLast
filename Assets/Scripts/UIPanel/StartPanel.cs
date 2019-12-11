using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    public Button btn_Start;
    public Button btn_Instruction;
    public Button btn_Settings;
    public Button btn_Exit;

    protected override void InitWindow()
    {
        base.InitWindow();
        btn_Start.onClick.AddListener(OnStartBtnClick);
        btn_Instruction.onClick.AddListener(OnInstructionBtnClick);
        btn_Settings.onClick.AddListener(OnSettingBtnClick);
        btn_Exit.onClick.AddListener(OnExitBtnClick);
    }

    private void OnStartBtnClick()
    {
        SetWindowState(false);
        StartSys.Instance.settingPanel.SetWindowState(false);
        StartSys.Instance.instructionPanel.SetWindowState(false);
        BattleSys.Instance.EnterBattle();
    }

    private void OnInstructionBtnClick()
    {
        StartSys.Instance.OpenInstructionPanel();
    }

    private void OnSettingBtnClick()
    {
        StartSys.Instance.OpenSettingPanel();
    }

    private void OnExitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
