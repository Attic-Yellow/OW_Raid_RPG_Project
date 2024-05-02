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

    #region ���� �� ����� ����Ʈ
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

    #region ��� ���� �޼���
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
            print("Ÿ�� ����ġ");
            return false;
        }

        if (ring.Count > 30)
        {
            print("���� ����");
            return false;
        }

        for (int i = 0; i < ring.Count; i++)
        {
            print(i);
            if (ring[i] != null && ring[i].equipment == EquipmentType.None)
            {
                print(ring[i] + "�� ��");
                ring[i] = equipment;
                onChangeGear?.Invoke();
                return true;
            }
        }

        return false;
    }
    #endregion

    #region ��� �̵� �� ���� �޼���
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(weapon[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                weapon[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                weapon[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (weapon[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                weapon[oldIndex] = weapon[newIndex];
                weapon[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(head[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                head[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                head[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (head[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                head[oldIndex] = head[newIndex];
                head[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(body[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                body[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                body[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (body[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                body[oldIndex] = body[newIndex];
                body[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(hands[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                hands[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                hands[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (hands[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                hands[oldIndex] = hands[newIndex];
                hands[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(legs[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                legs[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                legs[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (legs[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                legs[oldIndex] = legs[newIndex];
                legs[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(feet[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                feet[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                feet[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (feet[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                feet[oldIndex] = feet[newIndex];
                feet[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(auxiliary[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                auxiliary[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                auxiliary[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (auxiliary[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                auxiliary[oldIndex] = auxiliary[newIndex];
                auxiliary[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(earring[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                earring[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                earring[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (earring[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                earring[oldIndex] = earring[newIndex];
                earring[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(necklace[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                necklace[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                necklace[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (necklace[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                necklace[oldIndex] = necklace[newIndex];
                necklace[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(bracelet[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                bracelet[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                bracelet[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (bracelet[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                bracelet[oldIndex] = bracelet[newIndex];
                bracelet[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
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
                // print("���� ��� > ����� ����");
                CurrentEquipped.Instance.IsEquipped(ring[newIndex], oldIndex, newIndex, newSlotType, newSlotType);
                ring[newIndex] = equipment;
            }
            else
            {
                // print("���� ��� > ����� �ֱ�");
                ring[newIndex] = equipment;
            }
        }
        else if (newSlotType == SlotType.CurrentEquip)
        {
            // print("����� > ���� ��� ����");
            CurrentEquipped.Instance.IsEquipped(equipment, oldIndex, oldIndex, oldSlotType, oldSlotType);
        }
        else
        {
            if (ring[newIndex].equipment != EquipmentType.None)
            {
                // print("����� > ����� ����");
                ring[oldIndex] = ring[newIndex];
                ring[newIndex] = equipment;
            }
            else
            {
                // print("����� ��� �߰�");
                ring[newIndex] = equipment;
            }
        }

        onChangeGear?.Invoke();
        return true;
    }
    #endregion

    #region ���� �ʱ�ȭ �޼���
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
