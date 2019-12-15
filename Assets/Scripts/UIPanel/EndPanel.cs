using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPanel : BasePanel
{
    public Text txt_Tips;
    public Button btn_Restart;
    public Button btn_Back;

    private void Awake()
    {
        btn_Restart.onClick.AddListener(OnBtnRestartClick);
        btn_Back.onClick.AddListener(OnBtnBackClick);
    }

    public void SetInfo(string info, Color textColor)
    {
        SetText(txt_Tips, info);
        txt_Tips.color = textColor;
    }

    private void OnBtnRestartClick()
    {
        GameManager.Instance.isPauseGame = false;
        BattleSys.Instance.HideAllPanels();
        Destroy(GameManager.Instance.GetComponent<BattleMgr>());
        Destroy(GameManager.Instance.GetComponent<StateMgr>());

        BattleSys.Instance.EnterBattle(false);
    }

    private  void OnBtnBackClick()
    {
        GameManager.Instance.isPauseGame = false;
        SceneManager.LoadScene(Constant.SceneMainID);
        Destroy(GameManager.Instance.GetComponent<BattleMgr>());
        StartSys.Instance.EnterStart();
        BattleSys.Instance.HideAllPanels();
    }
}
