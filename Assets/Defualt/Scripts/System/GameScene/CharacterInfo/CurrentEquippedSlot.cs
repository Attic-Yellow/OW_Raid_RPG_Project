using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class CurrentEquippedSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Equipment equipment;
    private GameObject dragVisual;
    private Equipment tempEquipment; // �ӽ÷� ������ ��� ������

    public override void UpdateSlotUI()
    {
        itemIcon.sprite = IconData.Instance.GetitemIcon(equipment.itemImage);

        if (itemIcon.sprite != null)
        {
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

            CurrentEquipped.Instance.RemoveEquipped(equipment);
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
        CurrentEquipped.Instance.IsEquipped(newEquipment, oldIndex, newIndex, oldSlotType, newSlotType);
        UpdateSlotUI(); // ������ UI�� ������Ʈ
    }
    #endregion
}
