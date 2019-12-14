using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHit : IState
{
    public void OnEnter(EntityEnemy entity)
    {
        entity.aniState = AniState.Hit;
        Debug.Log("Enter Hit");
    }

    public void OnExit(EntityEnemy entity)
    {
        Debug.Log("Exit Hit");
    }

    public void OnProcess(EntityEnemy entity)
    {
        //播放受击音效TODO 

        //受击停止移动
        if (entity.ControllerMode == EnemyControllerMode.ModeCharacterController)
        {
            entity.SetDir(Vector3.zero);
        }
        else if (entity.ControllerMode == EnemyControllerMode.ModeNavMeshAgent)
        {
            entity.StopInNav();
        }

        entity.SetAction(Constant.ActionHit);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constant.ActionDefault);
            if(entity.aniState != AniState.Die)
            {
                entity.Idle();
            }
            
        },entity.GetAniLength(Constant.AniHitName));
    }
}
