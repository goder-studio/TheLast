using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SystemRoot
{
    #region 单例模式
    private static BattleSys _instance = null;
    public static BattleSys Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<BattleSys>();
            }
            return _instance;
        }
    }
    #endregion

    public PlayerPanel playerPanel;
    public OptionPanel optionPanel;
    private BattleMgr battleMgr;

    public override void InitSys()
    {
        base.InitSys();
        Debug.Log("Init BattleSys Done");
    }

    public void EnterBattle()
    {
        resSvc.AsyncLoadScene(Constant.SceneMainID, () =>
        {
            battleMgr = gameObject.AddComponent<BattleMgr>();
            battleMgr.Init();
            playerPanel.SetWindowState(true);
            audioSvc.StopBgMusic();
            //GameManager.Instance.HideCursor();
        });
    }

    public void ShakeCamera(float duration, float strength = 3, int vibrato = 10, float randomness = 90)
    {
        GetPlayer().controller.GetCurWeapon().ShakeCamera(duration, strength, vibrato, randomness);
    }

    #region UI Operation
    public void SetPlayerPanel(PlayerPanel playerPanel)
    {
        this.playerPanel = playerPanel;
    }

    public void InitPlayerPanel(string weaponName, int totalAmmo, ShootMode shootmode, string spritePath)
    {
        if (playerPanel != null)
        {
            playerPanel.InitPanel(weaponName, totalAmmo, shootmode, spritePath);
        }
    }

    public void SetCurrentWeapon(string weaponName)
    {
        playerPanel.SetCurWeapon(weaponName);
    }

    public void SetCurrentAmmo(int count)
    {
        playerPanel.SetCurrentAmmo(count);
    }

    public void SetTotalAmmo(int count)
    {
        playerPanel.SetTotalAmmo(count);
    }

    public void SetCurrentShootMode(ShootMode shootmode)
    {
        playerPanel.SetCurrentShootMode(shootmode);
    }

    public void SetWeaponIcon(Sprite sprite)
    {
        playerPanel.SetWeaponIcon(sprite);
    }

    public void SetHp(int hp)
    {
        playerPanel.SetHp(hp);
    }

    public void ShowSwitchShootModeTips(ShootMode shootmode)
    {
        playerPanel.ShowSwitchShootModeTips(shootmode);
    }

    public void ShowOptionPanel()
    {
        if(optionPanel.gameObject.activeInHierarchy)
        {
            optionPanel.animation.clip = resSvc.LoadAnimationClip(PathDefine.AniOpenOptionPanel);
            optionPanel.animation.Play();
        }
        else
        {
            optionPanel.SetWindowState(true);
        }
    }

    public void CloseOptionPanel()
    {
        optionPanel.animation.clip = resSvc.LoadAnimationClip(PathDefine.AniCloseOptionPanel);
        optionPanel.animation.Play();
    }
    #endregion

    #region BattleMgr Operation
    public List<EntityEnemy> GetAllEnemy()
    {
        if (battleMgr != null)
        {
            return battleMgr.GetAllEnemy();
        }
        return null;
    }

    public EntityEnemy GetEnemyByName(string name)
    {
        if (battleMgr != null)
        {
            return battleMgr.GetEnemyByName(name);
        }
        return null;
    }

    public Dictionary<string, EntityEnemy> GetEnemyDict()
    {
        if (battleMgr != null)
        {
            return battleMgr.GetEnemyDict();
        }
        return null;
    }

    public EntityPlayer GetPlayer()
    {
        if (battleMgr != null)
        {
            return battleMgr.entityPlayer;
        }
        return null;
    }

    public void AddKillCount()
    {
        battleMgr.AddKillCount();
    }

    public int GetTotalKillCount()
    {
        return battleMgr.GetTotalKillCount();
    }
    #endregion
}
