using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SkillType
{
    Active,
    Passive
}

[Serializable]
public class Skill
{
    public SkillType skillType;
    public string skillName;
    public string description;
    public Sprite skillIcon;
    public int useLevel;
    public int useMana;
    public int Damage;
    public int heal;
    public int range;
    public float coolTime;
    public float duration;
    public float spellTime;


    public virtual void UseSkill()
    {
        Debug.Log("Use Skill");
    }
}
