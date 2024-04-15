using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;

public class QuickSlot : Slot, IDropHandler
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
                // 스킬 슬롯 참조
                //Skill skill = slot.GetSkill();
                //if (skill != null)
                //{
                //    itemIcon.sprite = skill.skillIcon;
                //    itemIcon.gameObject.SetActive(true);
                //}
                //else
                //{
                //    ClearSlot();
                //}
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
}
