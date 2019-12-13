using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionBtnInteraction : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler,IPointerUpHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        AudioSvc.Instance.PlayEffectSound(PathDefine.btnClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        AudioSvc.Instance.PlayEffectSound(PathDefine.btnHoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}
