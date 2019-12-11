using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBorn : IState
{
    public void OnEnter(EntityEnemy entity)
    {
        entity.aniState = AniState.Born;
        Debug.Log("Enter Born");
    }

    public void OnExit(EntityEnemy entity)
    {
        Debug.Log("Exit Born");
    }

    public void OnProcess(EntityEnemy entity)
    {
        entity.SetAction(Constant.ActionBorn);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constant.ActionDefault);
            entity.Idle();
        }, entity.GetAniLength(Constant.AniBornName));
        Debug.Log("Process Born");
    }
}
