using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//滑动方式：根据拖拽结束时ScrollView的HorizontalNormalizedPosition值确定最近的页面
public class ScrollViewControllerOne : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private ScrollRect scrollRect;
    private GridLayoutGroup layout;
    //每个页面的normalizedPosition值
    private float[] page;
    //拖动是否结束
    private bool isDrag = false;
    //目标位置
    private float targetHorizontalPosition = 0.0f;
    //当前位置
    private int currentHorizontalPositionIndex = 0;
    //滑动速度
    public float smoothingSpeed = 10.0f;


    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        layout = GetComponentInChildren<GridLayoutGroup>();
        scrollRect.onValueChanged.AddListener((vector2) => { Debug.Log(scrollRect.horizontalNormalizedPosition); });

        InitPagePosition();
    }

    void Update()
    {
        if(!isDrag)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetHorizontalPosition, smoothingSpeed * Time.deltaTime);
        }
    }

    public void SwitchNextItem()
    {
        if (currentHorizontalPositionIndex == page.Length - 1)
            return;
        currentHorizontalPositionIndex++;
        targetHorizontalPosition = page[currentHorizontalPositionIndex];
    }

    public void SwitchPrevItem()
    {
        if (currentHorizontalPositionIndex == 0)
            return;
        currentHorizontalPositionIndex--;
        targetHorizontalPosition = page[currentHorizontalPositionIndex];
    }

    private void InitPagePosition()
    {
        int length = scrollRect.content.childCount;
        //初始化content的宽度
        scrollRect.content.sizeDelta = new Vector2((layout.cellSize.x + layout.spacing.x) * (length - 1),scrollRect.GetComponent<RectTransform>().sizeDelta.y);
        //初始化数组

        page = new float[length];
        for(int i = 0; i < length; i++)
        {
            page[i] = i * 1.0f / (length - 1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        float posX = scrollRect.horizontalNormalizedPosition;
        int index = 0;
        float offsetX = Mathf.Abs(posX - page[index]);
        for(int i = 1; i < page.Length; i++)
        {
            float tempX = Mathf.Abs(posX - page[i]);
            if(tempX < offsetX)
            {
                offsetX = tempX;
                index = i;
            }
        }
        targetHorizontalPosition = page[index];
    }
}
