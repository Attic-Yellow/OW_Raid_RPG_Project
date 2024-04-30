using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public class QuickSlotData
{
    public Dictionary<string, SlotDataList> slotDataList = new Dictionary<string, SlotDataList>();
}

[Serializable]
public class SlotDataList
{
    public string slotName;
    public string slotType;
}