using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoratorDesignMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ChristmasTree christmasTree = new ChristmasTree();
        christmasTree = new ChristmasTreeDecorator_Star(christmasTree);
        christmasTree = new ChristmasTreeDecorator_Lights(christmasTree);
        christmasTree = new ChristmasTreeDecorator_Gift(christmasTree);

        christmasTree.Show();

    }
}


public class ChristmasTree
{
    private readonly string core = "圣诞树";
    public virtual void Show()
    {
        Debug.Log($"展示圣诞节的核心{core}");
    }
}

/// <summary>
/// 圣诞树装饰器基类
/// </summary>
public class ChristmasTreeDecorator : ChristmasTree
{
    /// <summary>
    /// 被装饰的圣诞树状态
    /// </summary>
    protected ChristmasTree christmasTree = null;
    
    public ChristmasTreeDecorator(ChristmasTree tempChristmasTree)
    {
        christmasTree = tempChristmasTree;
    }

    public override void Show()
    {
        christmasTree.Show();
    }

}

/// <summary>
/// 圣诞星装饰器
/// </summary>
public class ChristmasTreeDecorator_Star : ChristmasTreeDecorator
{
    public ChristmasTreeDecorator_Star(ChristmasTree tempChristmasTree) : base(tempChristmasTree)
    {

    }

    public override void Show()
    {
        Debug.Log("在圣诞树顶上装饰了一个圣诞星");
        base.Show();
    }
}

/// <summary>
/// 彩灯装饰器
/// </summary>
public class ChristmasTreeDecorator_Lights : ChristmasTreeDecorator
{
    public ChristmasTreeDecorator_Lights(ChristmasTree tempChristmasTree):base(tempChristmasTree)
    {

    }

    public override void Show()
    {
        base.Show();
        Debug.Log("在圣诞树上装饰了一盏彩灯");
    }
}

/// <summary>
/// 礼物装饰器
/// </summary>
public class ChristmasTreeDecorator_Gift : ChristmasTreeDecorator
{
    public ChristmasTreeDecorator_Gift(ChristmasTree tempChristmasTree):base(tempChristmasTree)
    {

    }

    public override void Show()
    {
        base.Show();
        Debug.Log("在圣诞树底下装饰了一个礼物");
    }
}



