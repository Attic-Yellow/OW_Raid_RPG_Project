using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipped : MonoBehaviour
{
    public static Equipped Instance;

    public LinkState linkState = LinkState.Idle;

    public delegate void OnGearChanged();
    public OnGearChanged onChangeGear;

    #region 파츠 별 장비함 리스트
    public List<Equipment> weapon = new List<Equipment>();
    public List<Equipment> head = new List<Equipment>();
    public List<Equipment> body = new List<Equipment>();
    public List<Equipment> hands = new List<Equipment>();
    public List<Equipment> legs = new List<Equipment>();
    public List<Equipment> feet = new List<Equipment>();
    public List<Equipment> auxiliary = new List<Equipment>();
    public List<Equipment> earring = new List<Equipment>();
    public List<Equipment> necklace = new List<Equipment>();
    public List<Equipment> bracelet = new List<Equipment>();
    public List<Equipment> ring = new List<Equipment>();
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #region 장비 습득 메서드
    public bool AddWeapon(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Weapon)
        {
            return false;
        }

        if (weapon.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < weapon.Count; i++)
        {
            if (weapon[i] != null && weapon[i].equipment == EquipmentType.None)
            {
                weapon[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddHead(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Head)
        {
            return false;
        }

        if (head.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < head.Count; i++)
        {
            if (head[i] != null && head[i].equipment == EquipmentType.None)
            {
                head[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddBody(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Body)
        {
            return false;
        }

        if (body.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < body.Count; i++)
        {
            if (body[i] != null && body[i].equipment == EquipmentType.None)
            {
                body[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddHands(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Hands)
        {
            return false;
        }

        if (hands.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i] != null && hands[i].equipment == EquipmentType.None)
            {
                hands[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddLegs(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Legs)
        {
            return false;
        }

        if (legs.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < legs.Count; i++)
        {
            if (legs[i] != null && legs[i].equipment == EquipmentType.None)
            {
                legs[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddFeet(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Feet)
        {
            return false;
        }

        if (feet.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < feet.Count; i++)
        {
            if (feet[i] != null && feet[i].equipment == EquipmentType.None)
            {
                feet[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddAuxiliary(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Auxiliary)
        {
            return false;
        }

        if (auxiliary.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < auxiliary.Count; i++)
        {
            if (auxiliary[i] != null && auxiliary[i].equipment == EquipmentType.None)
            {
                auxiliary[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddEarring(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Earring)
        {
            return false;
        }

        if (earring.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < earring.Count; i++)
        {
            if (earring[i] != null && earring[i].equipment == EquipmentType.None)
            {
                earring[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddNecklace(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Necklace)
        {
            return false;
        }

        if (necklace.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < necklace.Count; i++)
        {
            if (necklace[i] != null && necklace[i].equipment == EquipmentType.None)
            {
                necklace[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddBracelet(Equipment equipment)
    {
        if (equipment.equipment != EquipmentType.Bracelet)
        {
            return false;
        }

        if (bracelet.Count > 30)
        {
            return false;
        }

        for (int i = 0; i < bracelet.Count; i++)
        {
            if (bracelet[i] != null && bracelet[i].equipment == EquipmentType.None)
            {
                bracelet[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool AddRing(Equipment equipment)
    {
        print(equipment);
        if (equipment.equipment != EquipmentType.Ring)
        {
            print("타입 불일치");
            return false;
        }

        if (ring.Count > 30)
        {
            print("공간 부족");
            return false;
        }

        for (int i = 0; i < ring.Count; i++)
        {
            print(i);
            if (ring[i] != null && ring[i].equipment == EquipmentType.None)
            {
                print(ring[i] + "에 들어감");
                ring[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }
    #endregion

    #region 장비 이동 및 스왑 메서드
    public bool AssignWeaponAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Weapon)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (weapon[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(weapon[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                weapon[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                weapon[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (weapon[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                weapon[oldIndex] = weapon[newIndex];
                weapon[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                weapon[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignHeadAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Head)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (head[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(head[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                head[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                head[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (head[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                head[oldIndex] = head[newIndex];
                head[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                head[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignBodyAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Body)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (body[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(body[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                body[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                body[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (body[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                body[oldIndex] = body[newIndex];
                body[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                body[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignHandsAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Hands)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (hands[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(hands[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                hands[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                hands[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (hands[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                hands[oldIndex] = hands[newIndex];
                hands[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                hands[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignLegsAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Legs)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (legs[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(legs[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                legs[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                legs[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (legs[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                legs[oldIndex] = legs[newIndex];
                legs[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                legs[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignFeetAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Feet)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (feet[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(feet[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                feet[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                feet[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (feet[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                feet[oldIndex] = feet[newIndex];
                feet[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                feet[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignAuxiliaryAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Auxiliary)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (auxiliary[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(auxiliary[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                auxiliary[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                auxiliary[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (auxiliary[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                auxiliary[oldIndex] = auxiliary[newIndex];
                auxiliary[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                auxiliary[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignEarringAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Earring)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (earring[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(earring[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                earring[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                earring[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (earring[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                earring[oldIndex] = earring[newIndex];
                earring[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                earring[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignNecklaceAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Necklace)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (necklace[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(necklace[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                necklace[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                necklace[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (necklace[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                necklace[oldIndex] = necklace[newIndex];
                necklace[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                necklace[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignBraceletAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Bracelet)
        {
            return false;
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (bracelet[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(bracelet[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                bracelet[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                bracelet[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (bracelet[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                bracelet[oldIndex] = bracelet[newIndex];
                bracelet[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                bracelet[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }

    public bool AssignRingAtIndex(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        if (newIndex < 0 || newIndex > 30 || equipment.equipment != EquipmentType.Ring)
        {
            return false; 
        }

        if (oldSlotType == SlotType.CurrentEquip)
        {
            if (ring[newIndex].equipment != EquipmentType.None)
            {
                // print("현재 장비 > 장비함 스왑");
                CurrentEquipped.Instance.IsEquipped(ring[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                ring[newIndex] = equipment;
            }
            else
            {
                // print("현재 장비 > 장비함 넣기");
                ring[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("장비함 > 현재 장비 스왑");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (ring[newIndex].equipment != EquipmentType.None)
            {
                // print("장비함 > 장비함 스왑");
                ring[oldIndex] = ring[newIndex];
                ring[newIndex] = equipment;
            }
            else
            {
                // print("장비함 장비 추가");
                ring[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }
    #endregion

    #region 슬롯 초기화 메서드
    public void RemoveWeapon(Equipment equipment, int index)
    {
        if (weapon[index] != null)
        {
            weapon[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveHead(Equipment equipment, int index)
    {
        if (head[index] != null)
        {
            head[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveBody(Equipment equipment, int index)
    {
        if (body[index] != null)
        {
            body[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveHands(Equipment equipment, int index)
    {
        if (hands[index] != null)
        {
            hands[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveLegs(Equipment equipment, int index)
    {
        if (legs[index] != null)
        {
            legs[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveFeet(Equipment equipment, int index)
    {
        if (feet[index] != null)
        {
            feet[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveAuxiliary(Equipment equipment, int index)
    {
        if (auxiliary[index] != null)
        {
            auxiliary[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveEarring(Equipment equipment, int index)
    {
        if (earring[index] != null)
        {
            earring[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveNecklace(Equipment equipment, int index)
    {
        if (necklace[index] != null)
        {
            necklace[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveBracelet(Equipment equipment, int index)
    {
        if (bracelet[index] != null)
        {
            bracelet[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    public void RemoveRing(Equipment equipment, int index)
    {
        if (ring[index] != null)
        {
            ring[index] = new Equipment();
            onChangeGear?.Invoke();
        }
    }

    #endregion

}
