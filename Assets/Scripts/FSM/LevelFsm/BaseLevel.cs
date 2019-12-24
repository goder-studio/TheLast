using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLevel
{
    public int levelID;
    //对应的场景ID
    public int sceneID;
    //关卡是否 被激活
    public bool isActive;
    //关卡是否已过关
    public bool isPass;

    public BaseLevel(int levelID, int sceneID,bool isActive,bool isPass)
    {
        this.levelID = levelID;
        this.sceneID = sceneID;
        this.isActive = isActive;
        this.isPass = isPass;
    }
}

