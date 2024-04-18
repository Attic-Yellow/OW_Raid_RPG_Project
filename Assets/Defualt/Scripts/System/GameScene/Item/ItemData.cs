using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public List<Consumable> items = new List<Consumable>();
    public List<Equipment> equip = new List<Equipment>();

}
