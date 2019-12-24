using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : IState
{
    public void OnEnter(EntityEnemy entity)
    {
        entity.aniState = AniState.Attack;
        Debug.Log("Enter Attack");
    }

    public void OnExit(EntityEnemy entity)
    {
        Debug.Log("Exit Attack");
    }

    public void OnProcess(EntityEnemy entity)
    {
        entity.SetAction(Constant.ActionAttack);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.AttackDamage();
        }, entity.GetAniLength(Constant.AniAttackName) * 0.5f);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constant.ActionDefault);
            entity.Idle();
        },entity.GetAniLength(Constant.AniAttackName));
        Debug.Log("Process Attack");
    }

}
