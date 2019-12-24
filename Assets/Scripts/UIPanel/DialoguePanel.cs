using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialoguePanel : BasePanel
{
    public Image[] img_RoleArray;
    public Image img_Bg;
    public Text txt_Name;
    public Text txt_Detail;
    public Button btn_next;
    public Animation anim;

    private void Awake()
    {
        btn_next.onClick.AddListener(OnBtnNextClick);
    }

    public void SetCurDialogue(int roleID,string name,string detail,string spritePath,string bgPath)
    {
        Sprite sprite = resSvc.LoadSprite(spritePath);
        Sprite bg = resSvc.LoadSprite(bgPath);

        img_Bg.sprite = bg;

        for (int i = 0; i < img_RoleArray.Length; i++)
        {
            if(i == roleID)
            {
                img_RoleArray[i].gameObject.SetActive(true);
                img_RoleArray[i].sprite = sprite;
            }
            else
            {
                img_RoleArray[i].gameObject.SetActive(false);
            }
        }
        txt_Name.text = name;
        txt_Detail.text = "";
        txt_Detail.DOText(detail, detail.Length * 0.05f).OnComplete(()=> { btn_next.gameObject.SetActive(true); });

    }

    private void HideDialogueDetail()
    {
        for (int i = 0; i < img_RoleArray.Length; i++)
        {
            img_RoleArray[i].gameObject.SetActive(false);
        }
        txt_Name.gameObject.SetActive(false);
        txt_Detail.gameObject.SetActive(false);
        btn_next.gameObject.SetActive(false);
    }

    private void OnBtnNextClick()
    {
        DialogueSys.Instance.SetCurDialogue();
        btn_next.gameObject.SetActive(false);
    }
}
