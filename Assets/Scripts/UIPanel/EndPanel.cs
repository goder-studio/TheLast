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

        //根据当前所在场景重新加载
        BattleSys.Instance.EnterBattle(BattleSys.Instance.CurLevel.sceneID,false);

        //switch(BattleSys.Instance.CurLevel)
        //{
        //    case BattleSceneState.SceneWar:
        //        BattleSys.Instance.EnterBattle(Constant.SceneBattleWarID, false);
        //        break;
        //    case BattleSceneState.SceneCity:
        //        BattleSys.Instance.EnterBattle(Constant.SceneBattleCityID, false);
        //        break;
        //    default:
        //        break;
        //}
        
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
