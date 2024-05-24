using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Etc
}

public enum WeaponType
{
    Short,
    Long
}

[Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public string itemImage;
    public int itemId;

    public virtual bool Use()
    {
        return false;
    }
}
