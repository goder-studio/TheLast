using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;

    private bool isMove;
    private bool canDestroy;
    public bool CanDestroy
    {
        get { return canDestroy; }
        set { canDestroy = value; }
    }
    private EntityEnemy entity;

    private Vector3 curDir;
    private Vector3 targetDir;
    public Vector3 TargetDir
    {
        get { return targetDir; }
        set
        {
            if(value == Vector3.zero)
            {
                isMove = false;
                SetBlend(Constant.BlendIdle);
            }
            else
            {
                isMove = true;
                targetDir = value;
                //if (entity.Hp <= 30)
                //{
                //    entity.SetBlend(Constant.BlendRun);
                //}
                //else
                //{
                //    entity.SetBlend(Constant.BlendWalk);
                //}
            }
        }
    }

    private void Update()
    {
        //暂停状态下，跳过Update()的逻辑处理
        if (GameManager.Instance.isPauseGame)
            return;
        
        if (curDir != targetDir)
        {
            //Debug.Log("curDir:" + curDir + "targetDir:" + targetDir);
            UpdateDir();
        }

        if(isMove)
        {
            SetMove();
        }

        if(canDestroy)
        {
            //延迟两秒销毁
            Destroy(this.gameObject,2.0f);
        }

    }


    public void SetMove()
    {
        //controller.Move(transform.forward * Time.deltaTime * Constant.EnemyMoveSpeed);
        if(entity.aniState == AniState.Walk)
        {
            controller.SimpleMove(transform.forward * Constant.EnemyWalkSpeed);
        }
        else if(entity.aniState == AniState.Run)
        {
            controller.SimpleMove(transform.forward * Constant.EnemyRunSpeed);
        }

    }

    public void SetEntity(EntityEnemy e)
    {
        entity = e;
    }

    public void SetBlend(float blend)
    {
        animator.SetFloat("Blend", blend);
    }

    public void SetAction(int actionID)
    {
        animator.SetInteger("Action", actionID);
    }

    public void SetAtkDir(Vector3 atkDir)
    {
        float angle = Vector3.SignedAngle(atkDir, Vector3.forward, Vector3.up);
        Vector3 eulerAngles = new Vector3(0, -angle, 0);
        transform.eulerAngles = eulerAngles;
    }

    public void UpdateDir()
    {
        if(Mathf.Abs(targetDir.x - curDir.x) <= Constant.smoothSpeed * Time.deltaTime && Mathf.Abs(targetDir.y - curDir.y) <= Constant.smoothSpeed * Time.deltaTime)
        {
            curDir = targetDir;
        }
        else
        {
            if(curDir.x < targetDir.x)
            {
                curDir.x += Constant.smoothSpeed * Time.deltaTime;
            }
            else
            {
                curDir.x -= Constant.smoothSpeed * Time.deltaTime;
            }
            
            if(curDir.y < targetDir.y)
            {
                curDir.y += Constant.smoothSpeed * Time.deltaTime;

            }
            else
            {
                curDir.y -= Constant.smoothSpeed * Time.deltaTime;
            }
        }

        float angle = Vector3.SignedAngle(curDir, Vector3.forward,Vector3.up);
        Vector3 eulerAngles = new Vector3(0, -angle, 0);
        transform.eulerAngles = eulerAngles;
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if(hit.collider.tag == "Bullet")
    //    {
    //        Debug.Log("碰撞");
    //        //进入受击状态
    //        entity.Hit();
    //        //播放流血的粒子动画TODO

    //        //销毁子弹
    //        Destroy(hit.gameObject);
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, Vector3.forward * 3);
    //    Gizmos.DrawRay(transform.position, targetDir);
    //}


}
