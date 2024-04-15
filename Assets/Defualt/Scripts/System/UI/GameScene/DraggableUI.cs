using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    [SerializeField] private RectTransform windowRectTransform;
    [SerializeField] private RectTransform dragAreaRectTransform;
    private Vector2 offset;
    private bool isDragging = false;

    // 해당 창 클릭 시 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        SetAsLast();

        if (RectTransformUtility.RectangleContainsScreenPoint(dragAreaRectTransform, eventData.position, eventData.pressEventCamera))
        {
            Vector2 mousePosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.position);
            offset = (Vector2)windowRectTransform.position - mousePosition;
            isDragging = true; 
        }
    }

    // 해당 창 드래그 영역 드래그 시 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging) 
        {
            Vector2 newPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.position) + offset;
            windowRectTransform.position = newPosition;
        }
    }

    // 드래그 종료 시 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    // 해당 창을 최상위로 이동(하이어라키 상 최하위 자식으로 이동)
    public void SetAsLast()
    {
        windowRectTransform.SetAsLastSibling(); // 클릭 시 해당 창을 최상위로 이동
    }
}
