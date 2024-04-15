using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType
{
    Item,
    Equipment,
    Skill,
    Quick,
}

public class Slot : MonoBehaviour
{
    public Image itemIcon;
    public SlotType slotType;
    public EquipmentType equipmentType;
    public int slotIndex;

    public virtual void UpdateSlotUI()
    {

    }

    public virtual void ClearSlot()
    {

    }

    public virtual void AssignItem(Item newItem)
    {

    }

    public virtual void AssignEquipment(Equipment newEquipment, int index)
    {

    }

    public virtual void AssignSkill(Skill newSkill)
    {

    }


    public virtual Equipment GetEquipment()
    {
        return null;
    }

    public virtual Skill GetSkill()
    {
        return null;
    }

    public virtual Item GetItem()
    {
        return null;
    }

    public virtual Slot AssignSlot()
    {
        return null;
    }
}
