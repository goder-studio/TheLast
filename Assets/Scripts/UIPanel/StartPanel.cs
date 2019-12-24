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
        StartSys.Instance.OpenChooseLevelPanel();
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
