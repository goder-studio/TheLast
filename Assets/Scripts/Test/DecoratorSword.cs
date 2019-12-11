using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoratorSword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Weapon sword = new Sword();
        sword = new RedDiamond(sword);
        sword = new BlueDiamond(sword);
        sword = new YellowDiamond(sword);

        sword.Attack();
    }
}

/// <summary>
/// 武器基类
/// </summary>
public abstract class Weapon
{
    /// <summary>
    /// 攻击方法
    /// </summary>
    public abstract void Attack();
}

/// <summary>
/// 武器剑类
/// </summary>
public class Sword:Weapon
{
    public override void Attack()
    {
        Debug.Log("剑普攻");
    }
}

public class WeaponDecorator:Weapon
{
    protected Weapon weapon;
    public WeaponDecorator(Weapon tempWeapon)
    {
        weapon = tempWeapon;
    }

    public override void Attack()
    {
        weapon.Attack();
    }
}

/// <summary>
/// 红宝石装饰
/// </summary>
public class RedDiamond : WeaponDecorator
{
    public RedDiamond(Weapon tempWeapon):base(tempWeapon)
    {

    }

    public override void Attack()
    {
        base.Attack();
        Dizziness();
    }

    private void Dizziness()
    {
        Debug.Log("红宝石效果：眩晕");
    }
}

/// <summary>
/// 蓝宝石装饰
/// </summary>
public class BlueDiamond : WeaponDecorator
{
    public BlueDiamond(Weapon tempWeapon):base(tempWeapon)
    {

    }

    public override void Attack()
    {
        base.Attack();
        Frozen();
    }

    private void Frozen()
    {
        Debug.Log("蓝宝石效果：冰冻");
    }
}

public class YellowDiamond : WeaponDecorator
{
    public YellowDiamond(Weapon tempWeapon):base(tempWeapon)
    {

    }

    public override void Attack()
    {
        base.Attack();
        Explode();
    }

    private void Explode()
    {
        Debug.Log("黄宝石效果：爆炸");
    }
}



