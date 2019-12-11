using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdapterDesignMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ISoilder soldier = new ISoilder(100);
        AutoRifle autoRifle = new AutoRifle(20, 20, 10);
        soldier.SetWeapon(autoRifle);

        IEnemy enemy = new IEnemy(100);
        Gun gun = new Gun(10, 10, 10);
        enemy.SetWeapon(gun);

        soldier.Attack(enemy);
        enemy.Attack(soldier);
    }
}
public abstract class ICharacter
{
    protected IWeapon m_weapon;
    protected int m_hp;

    public ICharacter(int hp)
    {
        m_hp = hp;
    }

    public void SetWeapon(IWeapon weapon)
    {
        m_weapon = weapon;
        m_weapon.SetCharacter(this);
    }

    public abstract void UnderAttack(int damage);
    public abstract void Attack(ICharacter character);
}

public class ISoilder : ICharacter
{
    public ISoilder(int hp) : base(hp) { }

    public override void UnderAttack(int damage)
    {
        m_hp -= damage;
        Debug.Log("Soilder受到伤害：" + damage + " 剩余血量：" + m_hp);
    }

    public override void Attack(ICharacter character)
    {
        Debug.Log("Soilder开火");
        m_weapon.Fire(character);
    }
}

public class IEnemy : ICharacter
{
    public IEnemy(int hp) : base(hp) { }

    public override void UnderAttack(int damage)
    {
        m_hp -= damage;
        Debug.Log("Enemy受到伤害：" + damage + " 剩余血量：" + m_hp);
    }

    public override void Attack(ICharacter character)
    {
        Debug.Log("Enemy开火");
        m_weapon.SetExtraAtkValue();
        m_weapon.Fire(character);
    }
}

public abstract class IWeapon
{
    protected int m_atkValue;
    protected int m_atkRange;
    protected int m_extraAtkValue;

    //武器拥有者
    protected ICharacter m_character;

    public IWeapon(int atkValue,int atkRange,int extraAtkValue)
    {
        m_atkValue = atkValue;
        m_atkRange = atkRange;
        m_extraAtkValue = extraAtkValue;
    }

    public void SetExtraAtkValue()
    {
        m_atkValue += m_extraAtkValue;
    }

    public void SetCharacter(ICharacter character)
    {
        m_character = character;
    }

    public abstract void Fire(ICharacter character);
}

public class AutoRifle : IWeapon
{
    public AutoRifle(int atkValue, int atkRange, int extraAtkValue) : base(atkValue,atkRange,extraAtkValue) { }

    public override void Fire(ICharacter character)
    {
        Debug.Log("AutoRifle Fire");
        character.UnderAttack(m_atkValue);
    }
}

public class Gun : IWeapon
{
    public Gun(int atkValue, int atkRange, int extraAtkValue) : base(atkValue,atkRange,extraAtkValue) { }

    public override void Fire(ICharacter character)
    {
        Debug.Log("Gun Fire");
        character.UnderAttack(m_atkValue);
    }
}
