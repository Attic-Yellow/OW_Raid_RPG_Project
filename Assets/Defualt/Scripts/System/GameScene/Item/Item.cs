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
