using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected TimerSvc timerSvc = null;
    protected AudioSvc audioSvc = null;

    public void SetWindowState(bool isActive = true)
    {
        if(gameObject.activeInHierarchy != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if(isActive)
        {
            InitWindow();
        }
        else
        {
            ClearWindow();
        }
    }

    protected virtual void InitWindow()
    {
        resSvc = ResSvc.Instance;
        timerSvc = TimerSvc.Instance;
        audioSvc = AudioSvc.Instance;
    }

    protected virtual void ClearWindow()
    {
        resSvc = null;
        timerSvc = null;
        audioSvc = null;
    }


    #region TextToolFunction
    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Text txt, int num = 0)
    {
        txt.text = num.ToString();
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    #endregion

    #region ImageToolFunction
    public void SetImage(Image image, string path)
    {
        Sprite sprite = resSvc.LoadSprite(path);
        image.sprite = sprite;
    }

    public void SetImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }
    #endregion

    #region Show/Hide ToolFunction
    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool isActive = true)
    {
        trans.gameObject.SetActive(isActive);
    }
    protected void SetActive(RectTransform rectTrans, bool isActive = true)
    {
        rectTrans.gameObject.SetActive(isActive);
    }
    protected void SetActive(Text txt, bool isActive = true)
    {
        txt.gameObject.SetActive(isActive);
    }
    protected void SetActive(Image image, bool isActive = true)
    {
        image.gameObject.SetActive(isActive);
    }

    #endregion
}
