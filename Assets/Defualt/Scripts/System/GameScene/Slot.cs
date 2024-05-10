using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType
{
    Item,
    Equipment,
    CurrentEquip,
    Skill,
    Quick,
}

public enum DragType
{
    Idle,
    Drag
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

    // 아이템 이동 시 메서드
    public virtual void AssignItem(Consumable consumable, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType, int itemCount)
    {

    }

    // 장비 이동 시 메서드
    public virtual void AssignEquipment(Equipment newEquipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newslotType)
    {

    }

    // 아이템, 스킬 퀵 슬롯 할당 시 메서드
    public virtual void AssignSlot(GameObject newslot)
    {

    }

    // 아이템 갯수 지정
    public virtual void SetItemCount(int itemCount)
    {

    }

    // 소모품 반환
    public virtual Consumable GetItem()
    {
        return null;
    }

    // 장비 반환
    public virtual Equipment GetEquipment()
    {
        return null;
    }

    // 스킬 반환
    public virtual Skill GetSkill()
    {
        return null;
    }

 
    // 아이템 갯수 반환
    public virtual int GetItemCount()
    {
        return 0;
    }

    public virtual void Use()
    {

    }
}
