using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgr : MonoBehaviour
{
    private Dictionary<LevelType, BaseLevel> fsm = new Dictionary<LevelType, BaseLevel>();
    

    public void Init()
    {
        bool[] levelActive = GameManager.Instance.GetLevelActiveArr();
        bool[] levelPass = GameManager.Instance.GetLevelPassArr();
        fsm.Add(LevelType.LevelWar, new LevelWar(0,Constant.SceneBattleWarID, levelActive[0], levelPass[0]));
        fsm.Add(LevelType.LevelCity, new LevelCity(1, Constant.SceneBattleCityID, levelActive[1], levelPass[1]));
    }

    public BaseLevel GetBaseLevel(LevelType type)
    {
        BaseLevel level = null;
        if(fsm.TryGetValue(type,out level))
        {
            return level;
        }
        return null;
    }

    public void UpdateBaseLevel(LevelType type,bool isActive,bool isPass)
    {
        BaseLevel baseLevel = GetBaseLevel(type);
        if(baseLevel != null)
        {
            baseLevel.isActive = isActive;
            baseLevel.isPass = isPass;
        }
    }

}
