using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;

public class QuickSlot : Slot, IDropHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Slot slot;
    private GameObject dragVisual;

    public override void UpdateSlotUI()
    {
        if (slot == null)
        {
            ClearSlot();
            return;
        }

        switch (slot.slotType)
        {
            case SlotType.Item:
            case SlotType.Equipment:
                // 아이템 또는 장비 슬롯 참조
                Equipment equipment = slot.GetEquipment();
                if (equipment != null)
                {
                    itemIcon.sprite = equipment.itemImage;
                    itemIcon.gameObject.SetActive(true);
                }
                else
                {
                    ClearSlot();
                }
                break;
            case SlotType.Skill:
                Skill skill = slot.GetSkill();
                if (skill != null)
                {
                    itemIcon.sprite = skill.skillIcon;
                    itemIcon.gameObject.SetActive(true);
                }
                else
                {
                    ClearSlot();
                }
                break;
        }
    }

    public override void ClearSlot()
    {
        slot = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            Slot slot = droppedObject.GetComponent<Slot>();
            if (slot != null)
            {
                this.slot = slot;
                UpdateSlotUI();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // 마우스 위치로 시각적 표현 이동
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }
        // 마우스 포인터 아래의 "Slot" 태그를 가진 오브젝트만 검사
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            // 드랍 위치의 슬롯 처리
            QuickSlot slot = hit.Value.gameObject.GetComponent<QuickSlot>();
            if (this.slot != null && slot != null)
            {
                // 드랍 성공: 아이템을 새 슬롯에 할당
                slot.AssignSlot(slot);
                slot.UpdateSlotUI();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot != null)
        {
            // 시각적 표현 생성
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(FindObjectOfType<Canvas>().transform); // Canvas를 부모로 설정
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // 현재 슬롯의 아이템 이미지 사용
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // 크기 조절
            visualImage.raycastTarget = false; // 이벤트 레이캐스트 무시
            ClearSlot(); // 슬롯 클리어
        }
    }

    public void AssignSlot(Slot slot)
    {
        this.slot = slot;
    }
}
