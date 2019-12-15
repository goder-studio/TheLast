using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    None,
    Player,
    Enemy,
}

public class BattleProps
{
    public int hp;
    public int damage;
}


public class EntityBase
{
    public EntityType entityType = EntityType.None;

    protected string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    protected BattleProps props;
    public BattleProps Props
    {
        get { return props; }
        set { props = value; }
    }

    protected int hp;
    public virtual int Hp
    {
        get { return hp; } 
        set { hp = value; }
    }

    public virtual Vector3 GetPos() {
        return Vector3.zero;
    }

    public virtual Transform GetTrans()
    {
        return null;
    }

    public void SetBattleProps(BattleProps props)
    {
        Props = props;
        Hp = props.hp;
    }
    public virtual void SetHpVal(string name,int oldVal,int newVal) { }
    public virtual void SetHurt(string name, int hurt) { }
    public virtual void SetActive(bool active){}
}
