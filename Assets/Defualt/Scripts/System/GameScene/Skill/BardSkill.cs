using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BardSkill : Skill
{
    public Job job;

    public override void UseSkill()
    {
        Debug.Log("Use Bard Skill");
    }
}
