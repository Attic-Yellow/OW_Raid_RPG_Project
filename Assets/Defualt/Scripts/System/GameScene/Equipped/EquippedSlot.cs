using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquippedSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Equipment equipment;
    private GameObject dragVisual;
    private Equipment tempEquipment; // 임시로 저장할 장비 데이터

    public override void UpdateSlotUI()
    {
        if (equipment != null && equipment.equipment != EquipmentType.None)
        {
            itemIcon.sprite = equipment.itemImage;
            itemIcon.gameObject.SetActive(true);
        }
    }

    public override void ClearSlot()
    {
        equipment = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }

    #region 드래그 시작
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // 마우스 위치로 시각적 표현 이동
        }
    }
    #endregion

    #region 드래그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        if (tempEquipment == null)
        {
            return;
        }

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
            Slot slot = hit.Value.gameObject.GetComponent<Slot>();
            if (slot != null && equipmentType == slot.equipmentType)
            {
                // 드랍 성공: 아이템을 새 슬롯에 할당
                slot.AssignEquipment(tempEquipment, slotIndex, slot.slotIndex, slotType, slot.slotType);
                slot.UpdateSlotUI();
            }
            else
            {
                equipment = tempEquipment;
                AssignEquipment(equipment, slotIndex, slot.slotIndex, slotType, slot.slotType);
            }
        }
        else
        {
            // 드랍 실패: 원래 슬롯에 아이템을 다시 할당
            equipment = tempEquipment;
            AssignEquipment(equipment, slotIndex, slotIndex, slotType, slotType);
        }

        // 임시 데이터 초기화
        tempEquipment = null;
    }
    #endregion

    #region 드래그 중
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipment != null && equipment.itemImage != null)
        {
            tempEquipment = equipment;

            // 시각적 표현 생성
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Canvas를 부모로 설정
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // 현재 슬롯의 아이템 이미지 사용
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // 크기 조절
            visualImage.raycastTarget = false; // 이벤트 레이캐스트 무시

            RemoveSlot(equipment, slotIndex); // 장비 제거
            ClearSlot(); // 슬롯 클리어
        }
        else
        {
            tempEquipment = null;
        }
    }
    #endregion

    #region 슬롯 할당
    public override void AssignEquipment(Equipment newEquipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        equipment = newEquipment; // 새로운 장비를 할당

        if (equipment != null)
        {
            switch (newEquipment.equipment)
            {
                case EquipmentType.Weapon:
                    Equipped.Instance.AssignWeaponAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Head:
                    Equipped.Instance.AssignHeadAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Body:
                    Equipped.Instance.AssignBodyAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Hands:
                    Equipped.Instance.AssignHandsAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Legs:
                    Equipped.Instance.AssignLegsAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Feet:
                    Equipped.Instance.AssignFeetAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Auxiliary:
                    Equipped.Instance.AssignAuxiliaryAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Earring:
                    Equipped.Instance.AssignEarringAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Necklace:
                    Equipped.Instance.AssignNecklaceAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Bracelet:
                    Equipped.Instance.AssignBraceletAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Ring:
                    Equipped.Instance.AssignRingAtIndex(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.None:
                    ClearSlot();
                    break;
            }
            if (equipment != null)
            {
                UpdateSlotUI(); // 슬롯의 UI를 업데이트
            }
        }
    }
    #endregion

    #region 슬롯 초기화
    private void RemoveSlot(Equipment newEquipment, int index)
    {
        switch (newEquipment.equipment)
        {
            case EquipmentType.Weapon:
                Equipped.Instance.RemoveWeapon(newEquipment, index);
                break;
            case EquipmentType.Head:
                Equipped.Instance.RemoveHead(newEquipment, index);
                break;
            case EquipmentType.Body:
                Equipped.Instance.RemoveBody(newEquipment, index);
                break;
            case EquipmentType.Hands:
                Equipped.Instance.RemoveHands(newEquipment, index);
                break;
            case EquipmentType.Legs:
                Equipped.Instance.RemoveLegs(newEquipment, index);
                break;
            case EquipmentType.Feet:
                Equipped.Instance.RemoveFeet(newEquipment, index);
                break;
            case EquipmentType.Auxiliary:
                Equipped.Instance.RemoveAuxiliary(newEquipment, index);
                break;
            case EquipmentType.Earring:
                Equipped.Instance.RemoveEarring(newEquipment, index);
                break;
            case EquipmentType.Necklace:
                Equipped.Instance.RemoveNecklace(newEquipment, index);
                break;
            case EquipmentType.Bracelet:
                Equipped.Instance.RemoveBracelet(newEquipment, index);
                break;
            case EquipmentType.Ring:
                Equipped.Instance.RemoveRing(newEquipment, index);
                break;
        }
    }
    #endregion

    public override Equipment GetEquipment()
    {
        return equipment;
    }
}
