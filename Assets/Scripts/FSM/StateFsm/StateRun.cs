using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : IState
{
    public void OnEnter(EntityEnemy entity)
    {
        entity.aniState = AniState.Run;
        Debug.Log("Enter Run");
    }

    public void OnExit(EntityEnemy entity)
    {
        Debug.Log("Exit Run");
    }

    public void OnProcess(EntityEnemy entity)
    {
        entity.SetBlend(Constant.BlendRun);
        entity.SetSpeed(Constant.EnemyRunSpeed);
        Debug.Log("Process Run");
    }
}
