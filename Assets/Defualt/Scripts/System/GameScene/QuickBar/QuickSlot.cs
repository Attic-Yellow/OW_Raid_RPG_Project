using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.LookDev;

public class QuickSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
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
                // ������ �Ǵ� ��� ���� ����
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

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // ���콺 ��ġ�� �ð��� ǥ�� �̵�
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
            QuickSlot slot = hit.Value.gameObject.GetComponent<QuickSlot>();
            if (this.slot != null && slot != null)
            {
                // ��� ����: �������� �� ���Կ� �Ҵ�
                Slot tempSlot = slot.slot;
                slot.AssignSlot(this.slot);
                this.slot = tempSlot;
                slot.UpdateSlotUI();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot != null)
        {
            // �ð��� ǥ�� ����
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(FindObjectOfType<Canvas>().transform); // Canvas�� �θ�� ����
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // ���� ������ ������ �̹��� ���
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // ũ�� ����
            visualImage.raycastTarget = false; // �̺�Ʈ ����ĳ��Ʈ ����
            ClearSlot(); // ���� Ŭ����
        }
    }


    public override void AssignSlot(Slot slot)
    {
        this.slot = slot;
        UpdateSlotUI();
    }
}
