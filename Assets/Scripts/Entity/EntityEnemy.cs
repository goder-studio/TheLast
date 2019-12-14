using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEnemy : EntityBase
{
    public StateMgr stateMgr = null;
    public BattleMgr battleMgr = null;

    private EnemyController controller;

    //敌人控制模式（CharacterController/NavMeshAgent）
    private EnemyControllerMode enemyControllerMode;
    public EnemyControllerMode ControllerMode
    {
        get { return enemyControllerMode; }
        set
        {
            enemyControllerMode = value;
            if (enemyControllerMode == EnemyControllerMode.ModeNavMeshAgent)
            {
                controller.controller.enabled = false;
                controller.agent.enabled = true;
            }
            else if (enemyControllerMode == EnemyControllerMode.ModeCharacterController)
            {
                controller.controller.enabled = true;
                controller.agent.enabled = false;
            }
        }
    }

    public AniState aniState = AniState.None;

    //AI逻辑检测间隔
    private float checkTime = 0.5f;
    private float checkCounter = 0.0f;

    //AI攻击间隔
    private float atkTime = 2.0f;
    private float atkCounter = 0.0f;

    private float atkAngle;
    private float atkDistance;
    private bool runAI = true;

    public EntityEnemy()
    {
        entityType = EntityType.Enemy;
    }

    public void TickAllLogic()
    {
        if (controller.CanDestroy)
        {
            return;
        }

        if(runAI == false)
        {
            Idle();
            return;
        }

        if(aniState == AniState.Idle || aniState == AniState.Walk || aniState == AniState.Run)
        {
            checkCounter += Time.deltaTime;
            if(checkCounter >= checkTime)
            {
                //计算目标方向
                Vector3 dir = CalcTargetDir();
                if(!InAtkRange())
                {
                    //没有在范围内，朝目标方向移动
                    if(ControllerMode == EnemyControllerMode.ModeNavMeshAgent)
                    {
                        StartInNav();
                    }
                    else if(ControllerMode == EnemyControllerMode.ModeCharacterController)
                    {
                        SetDir(dir);
                    }

                    if (Hp <= Props.hp * 0.3f) 
                    {
                        //敌人变红
                        Crazy();
                        Run();
                    }
                    else
                    {
                        Walk();
                    }
                }
                else
                {
                    //在范围内，停止移动并攻击
                    if (ControllerMode == EnemyControllerMode.ModeNavMeshAgent)
                    {
                        StopInNav();
                    }
                    else if (ControllerMode == EnemyControllerMode.ModeCharacterController)
                    {
                        SetDir(Vector3.zero);
                    }
                 
                    atkCounter += checkCounter;
                    if(atkCounter >= atkTime)
                    {
                        //设置攻击方向
                        SetAtkDir(dir);
                        atkCounter = 0;
                        Attack();
                    }
                    else
                    {
                        Idle();
                    }
                }

                checkCounter = 0;
            }
        }
    }

    private Vector3 CalcTargetDir()
    {
        EntityPlayer entityPlayer = battleMgr.entityPlayer;
        if(entityPlayer == null || entityPlayer.Hp <= 0)
        {
            runAI = false;
            return Vector3.zero;
        }
        Vector3 targetPos = entityPlayer.GetPos();
        Vector3 selfPos = GetPos();
        Vector3 tempDir = targetPos - selfPos;
        Vector3 dir = new Vector3(tempDir.x, 0, tempDir.z);
        return dir.normalized;
    }

    private bool InAtkRange()
    {
        EntityPlayer entityPlayer = battleMgr.entityPlayer;
        if (entityPlayer == null || entityPlayer.Hp <= 0)
        {
            runAI = false;
            return false;
        }
        Vector3 targetPos = entityPlayer.GetPos();
        Vector3 selfPos = GetPos();
        targetPos.y = 0;
        selfPos.y = 0;
        float distance = Vector3.Distance(targetPos, selfPos);
        if(distance <= atkDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 计算攻击伤害
    /// </summary>
    public void AttackDamage()
    {
        EntityPlayer player = battleMgr.entityPlayer;
        Debug.Log(InDistance(this.GetPos(), player.GetPos(), atkDistance) + " " + InAngle(this.GetTrans(), player.GetPos(), atkAngle));
        if (InDistance(this.GetPos(), player.GetPos(), atkDistance) && InAngle(this.GetTrans(), player.GetPos(), atkAngle))
        {
            player.Hp -= this.Props.damage;
            BattleSys.Instance.ShakeCamera(0.5f, 0.2f, 20);
        }
    }

    private bool InDistance(Vector3 from, Vector3 to, float distance)
    {
        float Distance = Vector3.Distance(from, to);
        if (Distance <= distance)
        {
            return true;
        }
        return false;
    }

    private bool InAngle(Transform trans, Vector3 to, float angle)
    {
        Vector3 delta = to - trans.position;
        float tmpAngle = Vector3.Angle(delta.normalized, trans.forward);
        if (tmpAngle <= angle * 0.5f)
        {
            return true;
        }
        return false;
    }

    public void PlayBloodEffect(Vector3 spawnPos,Quaternion rotation)
    {
        if(battleMgr != null)
        {
            battleMgr.PlayBloodEffect(spawnPos, rotation);
        }
    }

    #region Set Operation
    public void SetControllerMode(EnemyControllerMode mode)
    {
        ControllerMode = mode;
    }

    public void SetAtkProps(float atkDis,float atkAngle)
    {
        this.atkDistance = atkDis;
        this.atkAngle = atkAngle;
    }

    public void SetController(EnemyController ctr)
    {
        controller = ctr;
        controller.SetEntity(this);
    }

    public void SetSpeed(float speed)
    {
        controller.SetSpeed(speed);
    }

    public void SetBlend(float blend)
    {
        if(controller != null)
        {
            controller.SetBlend(blend);
        }
    }

    public void SetAction(int action)
    {
        if(controller != null)
        {
            controller.SetAction(action);
        }
    }

    public void SetDir(Vector3 dir)
    {
        if(controller != null)
        {
            controller.TargetDir = dir;
        }
    }

    public void SetAtkDir(Vector3 atkDir)
    {
        if(controller != null)
        {
            controller.SetAtkDir(atkDir);
        }
    }

    public override void SetActive(bool active)
    {
        if (controller != null)
        {
            controller.gameObject.SetActive(active);
        }
    }

    public void Destroy()
    {
        if (controller != null)
        {
            controller.CanDestroy = true;
        }
    }

    //停止寻路
    public void StopInNav()
    {
        controller.agent.isStopped = true;
    }

    public void StartInNav()
    {
        controller.agent.isStopped = false;
        controller.agent.SetDestination(BattleSys.Instance.GetPlayer().controller.transform.position);
    }
    #endregion

    public override Vector3 GetPos()
    {
        return controller.transform.position;
    }

    public override Transform GetTrans()
    {
        return controller.transform;
    }



    /// <summary>
    /// 血量低时变得狂暴
    /// </summary>
    private void Crazy()
    {
        Material material =  this.controller.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material;
        material.SetColor("_Color", Constant.colorEnemyCrazy);
        atkTime /= 2;
    }

    private IEnumerator DelayGradient(float time)
    {
        Material material = this.controller.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material;
        float curTime = Time.time;
        while (material.color != Constant.colorEnemyCrazy)
        {
            float t = (Time.time - curTime) / time;
            material.color = Color.Lerp(material.color, Constant.colorEnemyCrazy, t);
            yield return 0;
        }
    }

    #region Animation Clip Operation
    public AnimationClip[] GetAniClip()
    {
        if (controller != null)
        {
            return controller.animator.runtimeAnimatorController.animationClips;
        }
        return null;
    }

    public float GetAniLength(string aniName)
    {
        AnimationClip[] clips = GetAniClip();
        foreach(AnimationClip clip in clips)
        {
            string clipName = clip.name;
            if (clipName.Contains(aniName))
            {
                return clip.length * 1000;
            }
        }
        return 0;
    }
    #endregion

    #region 逻辑状态改变
    public void Idle()
    {
        stateMgr.ChangeState(this, AniState.Idle);
    }

    public void Walk()
    {
        stateMgr.ChangeState(this, AniState.Walk);
    }

    public void Run()
    {
        stateMgr.ChangeState(this, AniState.Run);
    }

    public void Attack()
    {
        stateMgr.ChangeState(this, AniState.Attack);
    }

    public void Hit()
    {
        stateMgr.ChangeState(this, AniState.Hit);
    }

    public void Born()
    {
        stateMgr.ChangeState(this, AniState.Born);
    }

    public void Die()
    {
        stateMgr.ChangeState(this, AniState.Die);
    }
    #endregion

    
}
