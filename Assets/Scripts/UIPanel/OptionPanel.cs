using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : BasePanel
{
    public Button btn_Top;
    public Button btn_Bottom;
    public Button btn_Left;
    public Button btn_Right;

    public Animation animation;

    protected override void InitWindow()
    {
        base.InitWindow();
    }
}
