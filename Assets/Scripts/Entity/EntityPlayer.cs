using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityBase
{
    public FpsController controller;

    public override int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            //更新血条显示
            BattleSys.Instance.SetHp(hp);
            //血量小于零显示失败页面
            if (hp <= 0)
            {
                BattleSys.Instance.ShowEndPanel(Constant.failTips, Constant.failTipsColor);
            }
        }
    }
    
    public EntityPlayer()
    {
        entityType = EntityType.Player;
    }

    public override Vector3 GetPos()
    {
        return controller.transform.position;
    }

    public override Transform GetTrans()
    {
        return controller.transform;
    }

    public void SetController(FpsController fpsController)
    {
        this.controller = fpsController;
        this.controller.Entity = this;
    }
}
