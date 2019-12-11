using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMgr : MonoBehaviour
{
    private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();

    public void Init()
    {
        fsm.Add(AniState.Idle, new StateIdle());
        fsm.Add(AniState.Walk, new StateWalk());
        fsm.Add(AniState.Run, new StateRun());
        fsm.Add(AniState.Attack, new StateAttack());
        fsm.Add(AniState.Born, new StateBorn());
        fsm.Add(AniState.Die, new StateDie());
        fsm.Add(AniState.Hit, new StateHit());
        Debug.Log("StateMgr Init Done");
    }

    public void ChangeState(EntityEnemy entity,AniState targetAniState)
    {
        if(entity.aniState == targetAniState)
        {
            return;
        }

        if(fsm.ContainsKey(targetAniState))
        {
            if(entity.aniState != AniState.None)
            {
                IState currentState = fsm[entity.aniState];
                currentState.OnExit(entity);
            }
            IState targetState = fsm[targetAniState];
            targetState.OnEnter(entity);
            targetState.OnProcess(entity);
        }
    }
}
