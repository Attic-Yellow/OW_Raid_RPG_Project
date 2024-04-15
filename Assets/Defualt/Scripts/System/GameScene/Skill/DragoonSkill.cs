using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DragoonSkill : Skill
{
    public Job job;

    public override void UseSkill()
    {
        Debug.Log("Use Dragoon Skill");
    }
}
