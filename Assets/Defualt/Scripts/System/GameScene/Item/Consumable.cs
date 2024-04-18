using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Consumable : Item
{
    public int minDropCount;
    public int maxDropCount;
    public int itemCount;

    public Consumable Clone()
    {
        return new Consumable
        {
            itemType = this.itemType,
            itemName = this.itemName,
            itemImage = this.itemImage,
            itemId = this.itemId,
            minDropCount = this.minDropCount,
            maxDropCount = this.maxDropCount,
            itemCount = this.itemCount
        };
    }
}
