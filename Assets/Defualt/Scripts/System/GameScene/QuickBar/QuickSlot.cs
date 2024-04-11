using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuickSlot : Slot
{
    public Slot slot;
    private GameObject dragVisual;

    public override void UpdateSlotUI()
    {
        switch (slot.slotType)
        {
            case SlotType.Item:
                break;
            case SlotType.Equipment:
                itemIcon.sprite = slot.GetEquipment().itemImage;
                itemIcon.gameObject.SetActive(true);
                break;
            case SlotType.Skill:
                break;
        }
    }

    public override void ClearSlot()
    {
        slot = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }
}
