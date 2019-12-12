using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : IState
{
    public void OnEnter(EntityEnemy entity)
    {
        entity.aniState = AniState.Die;
        Debug.Log("Enter Die");
    }

    public void OnExit(EntityEnemy entity)
    {
        Debug.Log("Exit Die");
    }

    public void OnProcess(EntityEnemy entity)
    {
        entity.SetAction(Constant.ActionDie);
        BattleSys.Instance.AddKillCount();
        //动画播放结束后销毁敌人
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.Destroy();
        },entity.GetAniLength(Constant.AniDieName));
        Debug.Log("Process Die");
    }
}
