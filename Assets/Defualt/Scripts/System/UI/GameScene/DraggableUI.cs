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

    //#region 창 클릭 이벤트
    //// 해당 창 클릭 시 호출
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

    #region 창 버튼 다운 이벤트
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
    #endregion

    #region 창 드래그 이벤트
    // 해당 창 드래그 영역 드래그 시 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging) 
        {
            Vector2 newPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.position) + offset;
            windowRectTransform.position = newPosition;
        }
    }
    #endregion

    #region 창 드래그 종료 이벤트
    // 드래그 종료 시 호출
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

    #region 창 최상위로 이동
    // 해당 창을 최상위로 이동(하이어라키 상 최하위 자식으로 이동)
    public void SetAsLast()
    {
        windowRectTransform.SetAsLastSibling(); // 클릭 시 해당 창을 최상위로 이동
    }
    #endregion
}
