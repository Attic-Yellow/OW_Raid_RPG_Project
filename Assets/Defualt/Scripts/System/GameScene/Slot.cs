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

    // ������ �̵� �� �޼���
    public virtual void AssignItem(Consumable consumable, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType, int itemCount)
    {

    }

    // ��� �̵� �� �޼���
    public virtual void AssignEquipment(Equipment newEquipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newslotType)
    {

    }

    // ������, ��ų �� ���� �Ҵ� �� �޼���
    public virtual void AssignSlot(GameObject newslot)
    {

    }

    // ������ ���� ����
    public virtual void SetItemCount(int itemCount)
    {

    }

    // �Ҹ�ǰ ��ȯ
    public virtual Consumable GetItem()
    {
        return null;
    }

    // ��� ��ȯ
    public virtual Equipment GetEquipment()
    {
        return null;
    }

    // ��ų ��ȯ
    public virtual Skill GetSkill()
    {
        return null;
    }

 
    // ������ ���� ��ȯ
    public virtual int GetItemCount()
    {
        return 0;
    }

    public virtual void Use()
    {

    }
}
