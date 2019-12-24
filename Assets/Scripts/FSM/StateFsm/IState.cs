using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AniState
{
    None,
    Idle,
    Walk,
    Run,
    Born,
    Hit,
    Attack,
    Die
}

public interface IState
{
    void OnEnter(EntityEnemy entity);
    void OnProcess(EntityEnemy entity);
    void OnExit(EntityEnemy entity);
}
