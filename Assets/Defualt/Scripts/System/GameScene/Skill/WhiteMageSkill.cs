using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WhiteMageSkill : Skill
{
    public Job job;

    public override void UseSkill()
    {
        Debug.Log("Use WhtieMage Skill");
    }
}
