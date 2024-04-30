using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{
    private void Start()
    {
        QuickSlot[] quickSlotArray = FindObjectsOfType<QuickSlot>(true);

        foreach (var quick in quickSlotArray)
        {
            if (quick.slot != null)
            {
                quick.AddSlotData(quick.slot);
            }
        }

        quickSlotArray[0].SaveSlotData();
    }
}
