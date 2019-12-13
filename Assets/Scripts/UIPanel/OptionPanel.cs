using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : BasePanel
{
    public Button btn_Top;
    public Button btn_Bottom;
    public Button btn_Left;
    public Button btn_Right;

    public Animation anim;

    private void Start()
    {
        btn_Top.onClick.AddListener(OnBtnTopClick);
        btn_Bottom.onClick.AddListener(OnBtnBottomClick);
        btn_Left.onClick.AddListener(OnBtnLeftClick);
        btn_Right.onClick.AddListener(OnBtnRightClick);
    }

    private void OnBtnTopClick()
    {
        BattleSys.Instance.SwitchWeapon(0);
        GameManager.Instance.HideCursor();
        GameManager.Instance.isPauseGame = false;
        SetWindowState(false);
    }

    private void OnBtnBottomClick()
    {
        BattleSys.Instance.SwitchWeapon(1);
        GameManager.Instance.HideCursor();
        GameManager.Instance.isPauseGame = false;
        SetWindowState(false);
    }

    private void OnBtnLeftClick()
    {
        GameManager.Instance.HideCursor();
        GameManager.Instance.isPauseGame = false;
        SetWindowState(false);
    }

    private void OnBtnRightClick()
    {
        GameManager.Instance.HideCursor();
        GameManager.Instance.isPauseGame = false;
        SetWindowState(false);
    }
}
