using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSys : SystemRoot
{
    #region 单例模式
    private static DialogueSys _instance = null;
    public static DialogueSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<DialogueSys>();
            }
            return _instance;
        }
    }
    #endregion

    public override void InitSys()
    {
        base.InitSys();
        Debug.Log("Init DialogueSys Done");
    }

    public DialoguePanel dialoguePanel;

    private DialogueCfg dialogueCfg = new DialogueCfg();
    private int curDialogueIndex;
    private DialogueInfo curDialogue;


    public void EnterDialogue(LevelType type,bool isWait)
    {
        DialogueCfg cfg = resSvc.GetCurDialogueCfg((int)type);
        resSvc.AsyncLoadScene(Constant.SceneDialogueID, () => {
            dialoguePanel.SetWindowState(true);
            //设置当前的对话配置文件
            dialogueCfg = cfg;
            curDialogueIndex = 0;
            SetCurDialogue();
        },isWait);
    }

    public void SetCurDialogue()
    {
        if(curDialogueIndex >= dialogueCfg.dialoguesList.Count)
        {
            HideAllPanel();
            BattleSys.Instance.EnterBattle(dialogueCfg.sceneID, false);
        }
        else
        {
            curDialogue = dialogueCfg.dialoguesList[curDialogueIndex];
            int roleID = curDialogue.roleID;
            string name = curDialogue.name;
            string detail = curDialogue.detail;
            string spritePath = curDialogue.spritePath;
            string bgPath = curDialogue.bgPath;
            dialoguePanel.SetCurDialogue(roleID, name, detail, spritePath,bgPath);
            curDialogueIndex++;
        }
    }

    private void HideAllPanel()
    {
        dialoguePanel.SetWindowState(false);
    }
}
