using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : BasePanel
{
    public Text txtCurWeapon;
    public Text txtCurrentAmmo;
    public Text txtTotalAmmo;
    public Image imgWeaponIcon;
    public Text txtCurrentShootMode;
    public Text txtSwitchShootMode;
    public Text txtHp;
    public Text txtCountDown;
    public Image imgHp;

    public Animation animCountDown;

    private bool changeHp = false;
    private Color targetColor = Constant.colorGreen;
    private float targetFillAmount;

    protected override void InitWindow()
    {
        base.InitWindow();
    }

    private void Update()
    {
        if(GameManager.Instance.isPauseGame)
        {
            StopCountDownAnim();
        }
        else
        {
            RePlayCountDownAnim();
        }

        if(changeHp)
        {
            txtHp.color = imgHp.color = Color.Lerp(imgHp.color, targetColor, Constant.colorSmoothSpeed);
            imgHp.fillAmount = Mathf.Lerp(imgHp.fillAmount, targetFillAmount, Constant.fillAmountSmoothSpeed);
        }
        if(imgHp.fillAmount == targetFillAmount && imgHp.color == targetColor)
        {
            changeHp = true;
        }
    }

    public void InitPanel(string weaponName,int totalAmmo,ShootMode shootmode,string spritePath)
    {
        txtCountDown.color = Color.white;
        txtCountDown.transform.localScale = Vector3.one;
        SetText(txtCurrentAmmo, weaponName);
        SetText(txtTotalAmmo, totalAmmo);
        SetText(txtCurrentShootMode, shootmode.ToString());
        SetImage(imgWeaponIcon, ResSvc.Instance.LoadSprite(spritePath));
    }

    public void SetCurWeapon(string weaponName)
    {
        SetText(txtCurWeapon, weaponName);
    }

    public void SetCurrentAmmo(int count)
    {
        SetText(txtCurrentAmmo, count);
    }

    public void SetTotalAmmo(int count)
    {
        SetText(txtTotalAmmo, count);
    }

    public void SetCurrentShootMode(ShootMode shootmode)
    {
        SetText(txtCurrentShootMode, shootmode.ToString());
    }

    public void SetWeaponIcon(Sprite sprite)
    {
        SetImage(imgWeaponIcon, sprite);
    }

    public void SetHp(int hp)
    {
        SetText(txtHp, hp);
        changeHp = true;
        targetFillAmount = hp * 1.0f / 100;
        if (hp >= 80 && hp <= 100)
        {
            targetColor = Constant.colorGreen;
        }
        else if(hp > 20 && hp < 80)
        {
            targetColor = Constant.colorYellow;
        }
        else if(hp >= 0 && hp <= 20)
        {
            targetColor = Constant.colorRed;
        }
    }

    public void ShowSwitchShootModeTips(ShootMode shootmode)
    {
        SetActive(txtSwitchShootMode, true);
        string tips = "开火模式：";
        tips += shootmode.ToString();
        SetText(txtSwitchShootMode, tips);
        txtSwitchShootMode.GetComponent<Animation>().Play();
    }

    public void ShowCountDown()
    {
        SetActive(txtCountDown, true);
    }

    public void HideCountDown()
    {
        txtCountDown.color = Color.white;
        txtCountDown.transform.localScale = Vector3.one;
        SetActive(txtCountDown, false);
    }
    
    private void StopCountDownAnim()
    {
        AnimationState state = animCountDown["CountDown"];
        state.speed = 0.0f;
    }

    private void RePlayCountDownAnim()
    {
        AnimationState state = animCountDown["CountDown"];
        state.speed = 1.0f;
    }

    public void SetCountDown(int time)
    {
        string res = "";
        int minute = time / 60;
        int second = time % 60;
        if(minute / 10 > 0)
        {
            res += minute.ToString();
        }
        else
        {
            res += "0" + minute.ToString();
        }
        res += ":";
        if (second / 10 > 0)
        {
            res += second.ToString();
        }
        else
        {
            res += "0" + second.ToString();
        }

        SetText(txtCountDown,res);

        if(time == 3)
        {
            animCountDown.clip = resSvc.LoadAnimationClip(PathDefine.AniCountDown);
            animCountDown.Play();
        }
    }
}
