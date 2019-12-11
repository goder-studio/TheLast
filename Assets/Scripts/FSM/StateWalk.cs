using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalk : IState
{
    public void OnEnter(EntityEnemy entity)
    {
        entity.aniState = AniState.Walk;
        Debug.Log("Enter Move");
    }

    public void OnExit(EntityEnemy entity)
    {
        Debug.Log("Exit Move");
    }

    public void OnProcess(EntityEnemy entity)
    {
        entity.SetBlend(Constant.BlendWalk);
        Debug.Log("Process Move");
    }

}
