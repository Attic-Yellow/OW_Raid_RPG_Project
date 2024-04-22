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
    private Equipment tempEquipment; // �ӽ÷� ������ ��� ������

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

    #region �巡�� ����
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // ���콺 ��ġ�� �ð��� ǥ�� �̵�
        }
    }
    #endregion

    #region �巡�� ����
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
        // ���콺 ������ �Ʒ��� "Slot" �±׸� ���� ������Ʈ�� �˻�
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            // ��� ��ġ�� ���� ó��
            Slot slot = hit.Value.gameObject.GetComponent<Slot>();
            if (slot != null && equipmentType == slot.equipmentType)
            {
                // ��� ����: �������� �� ���Կ� �Ҵ�
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
            // ��� ����: ���� ���Կ� �������� �ٽ� �Ҵ�
            equipment = tempEquipment;
            AssignEquipment(equipment, slotIndex, slotIndex, slotType, slotType);
        }

        // �ӽ� ������ �ʱ�ȭ
        tempEquipment = null;
    }
    #endregion

    #region �巡�� ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipment != null && equipment.itemImage != null)
        {
            tempEquipment = equipment;

            // �ð��� ǥ�� ����
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Canvas�� �θ�� ����
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // ���� ������ ������ �̹��� ���
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // ũ�� ����
            visualImage.raycastTarget = false; // �̺�Ʈ ����ĳ��Ʈ ����

            RemoveSlot(equipment, slotIndex); // ��� ����
            ClearSlot(); // ���� Ŭ����
        }
        else
        {
            tempEquipment = null;
        }
    }
    #endregion

    #region ���� �Ҵ�
    public override void AssignEquipment(Equipment newEquipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        equipment = newEquipment; // ���ο� ��� �Ҵ�

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
                UpdateSlotUI(); // ������ UI�� ������Ʈ
            }
        }
    }
    #endregion

    #region ���� �ʱ�ȭ
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
