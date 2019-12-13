using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : BasePanel
{
    public Button btn_Sure;
    public Button btn_No;

    private void Awake()
    {
        btn_Sure.onClick.AddListener(OnBtnSureClick);
        btn_No.onClick.AddListener(OnBtnNoClick);
    }

    private void OnBtnSureClick() 
    {
        GameManager.Instance.isPauseGame = false;
        SceneManager.LoadScene(Constant.SceneMainID);
        Destroy(GameManager.Instance.GetComponent<BattleMgr>());
        StartSys.Instance.EnterStart();
        BattleSys.Instance.HideAllPanels();
        //resSvc.AsyncLoadScene(Constant.SceneMainID, () => 
        //{
        //    Destroy(GameManager.Instance.GetComponent<BattleMgr>());
        //    StartSys.Instance.EnterStart();
        //    SetWindowState(false);
        //    BattleSys.Instance.playerPanel.SetWindowState(false);
        //}, false);
    }

    private void OnBtnNoClick()
    {
        GameManager.Instance.isPauseGame = false;
        GameManager.Instance.HideCursor();
        SetWindowState(false);
    }
}
