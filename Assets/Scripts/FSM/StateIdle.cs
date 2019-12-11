using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState
{
    public void OnEnter(EntityEnemy entity)
    {
        entity.aniState = AniState.Idle;
        Debug.Log("Enter Idle");
    }

    public void OnExit(EntityEnemy entity)
    {
        Debug.Log("Exit Idle");
    }

    public void OnProcess(EntityEnemy entity)
    {
        entity.SetDir(Vector3.zero);
        entity.SetBlend(Constant.BlendIdle);
        Debug.Log("Process Idle");
    }
}
