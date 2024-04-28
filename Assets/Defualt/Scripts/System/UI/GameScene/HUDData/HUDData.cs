using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HUDData
{
    public Dictionary<string, HUDDataList> hudDataList = new Dictionary<string, HUDDataList>();
}

[Serializable]
public class HUDDataList
{
    public float positionX;
    public float positionY;
    public float size;
    public int quickBarShape;
    public bool isActive;
}
