using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewControllerTwo : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    private ScrollRect scrollRect;
    private RectTransform content;
    private GridLayoutGroup layout;

    private float[] page;
    private int length;

    private bool isDrag;
    //刚拖动时鼠标位置
    private Vector2 mpBeginDrag;
    private Vector2 mpDelta;
    private float targetPagePosition;
    //当前下标
    private int pageIndex;
    public float smoothingSpeed = 10.0f;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        layout = content.GetComponent<GridLayoutGroup>();
        length = content.childCount;

        isDrag = false;
        targetPagePosition = 0.0f;
        pageIndex = 0;

        InitPageArray();
        InitContentLength();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDrag)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetPagePosition, Time.deltaTime * smoothingSpeed);
        }
    }

    private void InitPageArray()
    {
        //初始化数组
        page = new float[length];
        for(int i = 0; i < length; i++)
        {
            page[i] = i * 1.0f / (length - 1);
        }
    }

    private void InitContentLength()
    {
        content.sizeDelta = new Vector2((layout.cellSize.x + layout.spacing.x) * (length - 1), scrollRect.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        mpBeginDrag = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        mpDelta = (Vector2)Input.mousePosition - mpBeginDrag;
        if(mpDelta.x > 0)
        {
            pageIndex = --pageIndex < 0 ? 0 : pageIndex;
            targetPagePosition = page[pageIndex];
        }
        else if(mpDelta.x < 0)
        {
            pageIndex = ++pageIndex >= length ? length - 1 : pageIndex;
            targetPagePosition = page[pageIndex];
        }
        //DOTween.To(() => scrollRect.horizontalNormalizedPosition, (x) => scrollRect.horizontalNormalizedPosition = x, targetPagePosition, 0.5f).SetEase(Ease.InOutQuad);
    }
}
