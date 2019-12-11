using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public Text txtCurWeapon;
    public Text txtCurrentAmmo;
    public Text txtTotalAmmo;
    public Image imgWeaponIcon;
    public Text txtCurrentShootMode;
    public Text txtSwitchShootMode;
    public Text txtHp;
    public Image imgHp;

    private bool changeHp = false;
    private Color targetColor = Constant.colorGreen;
    private float targetFillAmount;

    private void Update()
    {
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
        txtCurrentAmmo.text = weaponName;
        txtTotalAmmo.text = totalAmmo.ToString();
        txtCurrentShootMode.text = shootmode.ToString();
        imgWeaponIcon.sprite = ResSvc.Instance.LoadSprite(spritePath);
        //imgWeaponIcon.sprite = Resources.Load<Sprite>(spritePath);
    }

    public void SetCurWeapon(string weaponName)
    {
        txtCurWeapon.text = weaponName;
    }

    public void SetCurrentAmmo(int count)
    {
        txtCurrentAmmo.text = count.ToString();
    }

    public void SetTotalAmmo(int count)
    {
        txtTotalAmmo.text = count.ToString();
    }

    public void SetCurrentShootMode(ShootMode shootmode)
    {
        txtCurrentShootMode.text = shootmode.ToString();
    }

    public void SetWeaponIcon(Sprite sprite)
    {
        imgWeaponIcon.sprite = sprite;
    }

    public void SetHp(int hp)
    {
        txtHp.text = hp.ToString();
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
        txtSwitchShootMode.gameObject.SetActive(true);
        string tips = "开火模式：";
        tips += shootmode.ToString();
        txtSwitchShootMode.text = tips;
        txtSwitchShootMode.GetComponent<Animation>().Play();
    }
}
