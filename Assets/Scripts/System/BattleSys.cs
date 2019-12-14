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
    public PausePanel pausePanel;
    public MipMapPanel mipmapPanel;

    private BattleMgr battleMgr;

    public override void InitSys()
    {
        base.InitSys();
        Debug.Log("Init BattleSys Done");
    }

    public void EnterBattle()
    {
        resSvc.AsyncLoadScene(Constant.SceneBattleID, () =>
        {
            battleMgr = gameObject.AddComponent<BattleMgr>();
            battleMgr.Init();
            playerPanel.SetWindowState(true);
            mipmapPanel.SetWindowState(true);
            audioSvc.StopBgMusic();
            //GameManager.Instance.HideCursor();
        },true);
    }

    public void ShakeCamera(float duration, float strength = 3, int vibrato = 10, float randomness = 90)
    {
        GetPlayer().controller.GetCurWeapon().ShakeCamera(duration, strength, vibrato, randomness);
    }

    #region UI Operation
    public void HideAllPanels()
    {
        playerPanel.SetWindowState(false);
        optionPanel.SetWindowState(false);
        pausePanel.SetWindowState(false);
        mipmapPanel.SetWindowState(false);
    }

    //PlayerPanel
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

    //OptionPanel
    public void ShowOptionPanel()
    {
        GameManager.Instance.ShowCursor();
        GameManager.Instance.isPauseGame = true;
        optionPanel.SetWindowState(true);
        optionPanel.anim.clip = resSvc.LoadAnimationClip(PathDefine.AniOpenOptionPanel);
        optionPanel.anim.Play();
        //if (optionPanel.gameObject.activeInHierarchy)
        //{
        //    optionPanel.anim.clip = resSvc.LoadAnimationClip(PathDefine.AniOpenOptionPanel);
        //    optionPanel.anim.Play();
        //}
        //else
        //{
        //    optionPanel.SetWindowState(true);
        //}

    }

    public void CloseOptionPanel()
    {
        GameManager.Instance.HideCursor();
        GameManager.Instance.isPauseGame = false;
        optionPanel.anim.clip = resSvc.LoadAnimationClip(PathDefine.AniCloseOptionPanel);
        optionPanel.anim.Play();
        //播放完隐藏panel
        timerSvc.AddTimeTask((int tid) => {
            optionPanel.SetWindowState(false);
        },optionPanel.anim["CloseOptionPanel"].length * 1000);
    }
    //PausePanel 
    public void ShowPausePanel()
    {
        GameManager.Instance.ShowCursor();
        GameManager.Instance.isPauseGame = true;
        pausePanel.SetWindowState(true);
    }

    public void ShowCountDown()
    {
        playerPanel.ShowCountDown();
    }

    public void HideCountDown()
    {
        playerPanel.HideCountDown();
    }

    public void SetCountDown(int time)
    {
        playerPanel.SetCountDown(time);
    }

    //MipMapPanel
    public void ShowMipmapPanel()
    {
        mipmapPanel.SetWindowState(true);
    }

    public void HideMipmapPanel()
    {
        mipmapPanel.SetWindowState(false);
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

    public void SwitchWeapon(int index)
    {
        battleMgr.GetFpsController().SetCurWeapon(index);
    }

    #endregion
}
