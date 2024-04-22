using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEquipped : MonoBehaviour
{
    public static CurrentEquipped Instance;

    public delegate void OnEquippChanged();
    public OnEquippChanged onChangeEquipp;

    public List<Equipment> currentEquippeds = new List<Equipment>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            IsEquipped(ItemData.Instance.equip[10000], 0, 0, SlotType.Equipment, SlotType.Equipment);
        }
    }

    #region 장비 착용 및 교체 메서드
    // 장비 착용 및 교체 메서드
    public bool IsEquipped(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        print(equipment.itemName);
        print(equipment.equipment);
        int index = (int)equipment.equipment; // EquipmentType에 해당하는 인덱스를 정수로 변환

        // 해당 위치에 장비가 이미 있으면, 이전 장비를 적절한 위치에 추가
        if (currentEquippeds[index] != null && currentEquippeds[index].equipment != EquipmentType.None)
        {
            switch (equipment.equipment)
            {
                case EquipmentType.Weapon:
                    Equipped.Instance.AssignWeaponAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Head:
                    Equipped.Instance.AssignHeadAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Body:
                    Equipped.Instance.AssignBodyAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Hands:
                    Equipped.Instance.AssignHandsAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Legs:
                    Equipped.Instance.AssignLegsAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Feet:
                    Equipped.Instance.AssignFeetAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Auxiliary:
                    Equipped.Instance.AssignAuxiliaryAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Earring:
                    Equipped.Instance.AssignEarringAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Necklace:
                    Equipped.Instance.AssignNecklaceAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Bracelet:
                    Equipped.Instance.AssignBraceletAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
                case EquipmentType.Ring:
                    Equipped.Instance.AssignRingAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
                    break;
            }
        }

        // 새 장비를 해당 위치에 설정
        currentEquippeds[index] = equipment;

        // 장비 변경 알림
        onChangeEquipp?.Invoke();

        return true;
    }
    #endregion

    #region 장비 해제 메서드
    // 장비 해제 메서드
    public void RemoveEquipped(Equipment equipment)
    {
        int index = (int)equipment.equipment; // EquipmentType에 해당하는 인덱스를 정수로 변환

        // 장비를 제거하고, 장비 변경 알림
        currentEquippeds[index] = new Equipment();
        onChangeEquipp?.Invoke();
    }
    #endregion
}
