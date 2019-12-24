using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelPanel : BasePanel
{
    public Button[] btn_Scenes;
    public Image[] img_Locks;
    public Image[] img_Passes;
    public Button btn_Next;
    public Button btn_Prev;

    private void Awake()
    {
        btn_Scenes[0].onClick.AddListener(OnBtnSceneWarClick);
        btn_Scenes[1].onClick.AddListener(OnBtnSceneCityClick);
        btn_Next.onClick.AddListener(OnBtnNextClick);
        btn_Prev.onClick.AddListener(OnBtnPrevClick);
    }

    protected override void InitWindow()
    {
        base.InitWindow();
        bool[] levelActiveArr = GameManager.Instance.GetLevelActiveArr();
        bool[] levelPassArr = GameManager.Instance.GetLevelPassArr();
        for(int i = 0; i < levelActiveArr.Length; i++)
        {
            if(levelActiveArr[i] == true)
            {
                btn_Scenes[i].interactable = true;
                img_Locks[i].gameObject.SetActive(false);
            }
            else
            {
                btn_Scenes[i].interactable = false;
                img_Locks[i].gameObject.SetActive(true);
            }
        }
        for(int i = 0; i <levelPassArr.Length; i++)
        {
            if(levelPassArr[i] == true)
            {
                img_Passes[i].gameObject.SetActive(true);
            }
            else
            {
                img_Passes[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnBtnSceneWarClick()
    {
        SetWindowState(false);
        StartSys.Instance.HideAllPanel();
       
        BaseLevel levelWar = GameManager.Instance.levelMgr.GetBaseLevel(LevelType.LevelWar);
        if(!levelWar.isPass)
        {
            DialogueSys.Instance.EnterDialogue(LevelType.LevelWar, true);
        }
        else
        {
            BattleSys.Instance.EnterBattle(Constant.SceneBattleWarID, true);
        }
        
    }

    private void OnBtnSceneCityClick()
    {
        SetWindowState(false);
        StartSys.Instance.HideAllPanel();

        BaseLevel levelCity = GameManager.Instance.levelMgr.GetBaseLevel(LevelType.LevelCity);
        //如果没有过关进入剧情对话，如果已经过关了就直接进入战斗
        if (!levelCity.isPass)
        {
            DialogueSys.Instance.EnterDialogue(LevelType.LevelCity, true);
        }
        else
        {
            BattleSys.Instance.EnterBattle(Constant.SceneBattleCityID, true);
        }
    }

    private void OnBtnNextClick()
    {
        GetComponentInChildren<ScrollViewControllerOne>().SwitchNextItem();
    }

    private void OnBtnPrevClick()
    {
        GetComponentInChildren<ScrollViewControllerOne>().SwitchPrevItem();
    }
}
