using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonInteraction : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioSvc.Instance.PlayEffectSound(PathDefine.btnClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSvc.Instance.PlayEffectSound(PathDefine.btnHoverSound);
    }
}
