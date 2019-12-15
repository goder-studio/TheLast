using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPanel : BasePanel
{
    public Button btn_Start;
    public Button btn_Instruction;
    public Button btn_Settings;
    public Button btn_Exit;

    private void Awake()
    {
        btn_Start.onClick.AddListener(OnStartBtnClick);
        btn_Instruction.onClick.AddListener(OnInstructionBtnClick);
        btn_Settings.onClick.AddListener(OnSettingBtnClick);
        btn_Exit.onClick.AddListener(OnExitBtnClick);
    }


    private void OnStartBtnClick()
    {
        //audioSvc.PlayEffectSound(PathDefine.btnClickSound);
        SetWindowState(false);
        StartSys.Instance.settingPanel.SetWindowState(false);
        StartSys.Instance.instructionPanel.SetWindowState(false);
        BattleSys.Instance.EnterBattle(true);
    }

    private void OnInstructionBtnClick()
    {
        //audioSvc.PlayEffectSound(PathDefine.btnClickSound);
        StartSys.Instance.OpenInstructionPanel();
    }

    private void OnSettingBtnClick()
    {
        //audioSvc.PlayEffectSound(PathDefine.btnClickSound);
        StartSys.Instance.OpenSettingPanel();
    }

    private void OnExitBtnClick()
    {
        //audioSvc.PlayEffectSound(PathDefine.btnClickSound);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
