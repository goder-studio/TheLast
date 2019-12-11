using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityBase
{
    public FpsController controller;
    
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
