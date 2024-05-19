using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    [SerializeField] private RectTransform windowRectTransform;
    [SerializeField] private RectTransform dragAreaRectTransform;
    private Vector2 offset;
    private bool isDragging = false;

    //#region â Ŭ�� �̺�Ʈ
    //// �ش� â Ŭ�� �� ȣ��
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    GameObject clickedObject = eventData.pointerPress;
    //    GameObject dragArea = clickedObject.transform.childCount > 0 ? clickedObject.transform.GetChild(1).gameObject : null;
    //    GameManager.Instance.uiManager.gameSceneUI.hudController.PointerClick(dragArea);

    //    QuickBar quickBar = clickedObject.GetComponent<QuickBar>();

    //    if (quickBar != null)
    //    {
    //        quickBar.UpdateHUDData(-2);
    //    }
    //}
    //#endregion

    #region â ��ư �ٿ� �̺�Ʈ
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
    #endregion

    #region â �巡�� �̺�Ʈ
    // �ش� â �巡�� ���� �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging) 
        {
            Vector2 newPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.position) + offset;
            windowRectTransform.position = newPosition;
        }
    }
    #endregion

    #region â �巡�� ���� �̺�Ʈ
    // �巡�� ���� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        GameObject dragObject = eventData.pointerDrag;
        QuickBar quickBar = dragObject.GetComponent<QuickBar>();

        if (quickBar != null)
        {
            quickBar.UpdateHUDData(-2);
        }
    }
    #endregion

    #region â �ֻ����� �̵�
    // �ش� â�� �ֻ����� �̵�(���̾��Ű �� ������ �ڽ����� �̵�)
    public void SetAsLast()
    {
        windowRectTransform.SetAsLastSibling(); // Ŭ�� �� �ش� â�� �ֻ����� �̵�
    }
    #endregion
}
