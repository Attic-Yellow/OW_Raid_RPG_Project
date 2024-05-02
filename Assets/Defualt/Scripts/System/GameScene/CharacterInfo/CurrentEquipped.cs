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

    #region ��� ���� �� ��ü �޼���
    // ��� ���� �� ��ü �޼���
    public bool IsEquipped(Equipment equipment, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType)
    {
        int index = (int)equipment.equipment; // EquipmentType�� �ش��ϴ� �ε����� ������ ��ȯ

        // �ش� ��ġ�� ��� �̹� ������, ���� ��� ������ ��ġ�� �߰�
        if (currentEquippeds[index] != null && currentEquippeds[index].equipment != EquipmentType.None)
        {
            Equipped.Instance.AssignEquipAtIndex(currentEquippeds[index], oldIndex, newIndex, oldSlotType, newSlotType);
        }

        // �� ��� �ش� ��ġ�� ����
        currentEquippeds[index] = equipment;

        CharacterData.Instance.CalculateAndSetStats();
        // ��� ���� �˸�
        onChangeEquipp?.Invoke();

        return true;
    }
    #endregion

    #region ��� ���� �޼���
    // ��� ���� �޼���
    public void RemoveEquipped(Equipment equipment)
    {
        int index = (int)equipment.equipment; // EquipmentType�� �ش��ϴ� �ε����� ������ ��ȯ

        // ��� �����ϰ�, ��� ���� �˸�
        currentEquippeds[index] = new Equipment();
        CharacterData.Instance.CalculateAndSetStats();
        onChangeEquipp?.Invoke();
    }
    #endregion
}
