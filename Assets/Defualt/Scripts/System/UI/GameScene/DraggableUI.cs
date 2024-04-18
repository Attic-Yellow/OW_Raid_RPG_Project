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

    // �ش� â Ŭ�� �� ȣ��
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

    // �ش� â �巡�� ���� �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging) 
        {
            Vector2 newPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.position) + offset;
            windowRectTransform.position = newPosition;
        }
    }

    // �巡�� ���� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    // �ش� â�� �ֻ����� �̵�(���̾��Ű �� ������ �ڽ����� �̵�)
    public void SetAsLast()
    {
        windowRectTransform.SetAsLastSibling(); // Ŭ�� �� �ش� â�� �ֻ����� �̵�
    }
}
