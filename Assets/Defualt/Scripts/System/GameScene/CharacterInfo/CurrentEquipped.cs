using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEquipped : MonoBehaviour
{
    public static CurrentEquipped Instance;

    public delegate void OnEquippChanged();
    public OnEquippChanged onChangeEquipp;

    public List<Equipment> currentEquippeds = new List<Equipment>();
    public bool isStarted;

    private CharacterData CharacterData;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        isStarted = true;
        CharacterData.Instance.UpdateCurrentEquipData(CharacterData.Instance.CurrentEquip());
        CharacterData.Instance.CalculateAndSetStats();
        isStarted = false;
    }

    #region 장비 착용 및 교체 메서드
    // 장비 착용 및 교체 메서드
    public bool IsEquipped(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        int index = (int)equipment.equipment; // EquipmentType에 해당하는 인덱스를 정수로 변환

        // 해당 위치에 장비가 이미 있으면, 이전 장비를 적절한 위치에 추가
        if (currentEquippeds[index] != null && currentEquippeds[index].equipment != EquipmentType.None)
        {
            Equipped.Instance.AssignEquipAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
        }

        // 새 장비를 해당 위치에 설정
        currentEquippeds[index] = equipment;

        CharacterData.Instance.CalculateAndSetStats();
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
        CharacterData.Instance.CalculateAndSetStats();
        onChangeEquipp?.Invoke();
    }
    #endregion
}
