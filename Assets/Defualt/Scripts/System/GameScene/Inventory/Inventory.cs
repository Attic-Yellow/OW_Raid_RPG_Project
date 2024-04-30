using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public delegate void OnItemChanged();
    public OnItemChanged onChangeItem;

    public List<Consumable> items = new List<Consumable>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #region ������ ���� �޼���
    // ������ ���� �޼���
    public bool AddItem(Consumable consumable, int itemCount)
    {
        if (items.Count > 120)
        {
            return false;
        }

        Consumable newItem = consumable.Clone();
        newItem.itemCount = itemCount;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == newItem.itemName)
            {
                items[i].itemCount += newItem.itemCount;
                onChangeItem?.Invoke();
                return true;
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].itemType == ItemType.Consumable)
            {
                items[i] = newItem;
                items[i].itemCount = newItem.itemCount;
                onChangeItem?.Invoke();
                return true;
            }
        }

        return false;
    }
    #endregion

    #region ������ �̵� �� ���� �޼���
    // ������ �̵� �� ���� �޼���
    public bool AssignItemAtIndex(Consumable consumable, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType, int itemCount)
    {
        if (newIndex < 0 || newIndex > 120 || consumable.itemType != ItemType.Consumable)
        {
            return false;
        }

        if (items[newIndex].itemImage != null)
        {
            if (items[newIndex].itemName == consumable.itemName) // print("���� ������ ��ħ");
            {
                items[newIndex].itemCount += itemCount; 
            }
            else // print("������ > ������ ����");
            {
                items[oldIndex] = items[newIndex];
                items[newIndex] = consumable;
            }
        }
        else // print("������ ��� �߰�");
        {
            items[newIndex] = consumable;
            items[newIndex].itemCount = itemCount;
        }

        onChangeItem?.Invoke();
        return true;
    }
    #endregion

    #region ������ ���� �� ������ �޼���
    // ������ ���� �� ������ �޼���
    public void RemoveItem(Consumable consumable, int index)
    {
        if (items[index] != null)
        {
            items[index] = new Consumable();
            onChangeItem?.Invoke();
        }
    }
    #endregion

    #region ������ ��� ���� ���� ��ȯ
    public int GetItemCount()
    {
        return Random.Range(ItemData.Instance.items[0].minDropCount, ItemData.Instance.items[0].maxDropCount);
    }
    #endregion
}
