﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if(_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            return _instance;
        }
    }

    public LoadingPanel loadingPanel;
    public Texture2D img_Cursor;


    private void Awake()
    {
        //服务模块初始化
        TimerSvc timerSvc = GetComponent<TimerSvc>();
        timerSvc.InitSvc();

        ResSvc resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();

        AudioSvc audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();

        //开始系统初始化
        StartSys startSys = GetComponent<StartSys>();
        startSys.InitSys();

        //战斗系统初始化
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        Cursor.SetCursor(img_Cursor, Vector2.zero, CursorMode.Auto);

        DontDestroyOnLoad(this);

        startSys.EnterStart();
    }

    private void Update()
    {

    }

   //public void ShowCursor()
   //{
   //    showCursor = true;
   //    img_Cursor.gameObject.SetActive(true);
   //    Cursor.lockState = CursorLockMode.None;
   //}

   //public void HideCursor()
   //{
   //    showCursor = false;
   //    img_Cursor.gameObject.SetActive(false);
   //    Cursor.lockState = CursorLockMode.Locked;
   //}

    //private void UpdateCursorPosition()
    //{
    //    img_Cursor.transform.position = Input.mousePosition + new Vector3(img_Cursor.rectTransform.sizeDelta.x / 2, -img_Cursor.rectTransform.sizeDelta.y / 2, 0);
    //}

}