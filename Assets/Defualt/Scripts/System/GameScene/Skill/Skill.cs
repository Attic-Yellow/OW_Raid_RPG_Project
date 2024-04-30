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
    public Sprite skillIcon;
    public bool globalCoolDown;
    public bool isCombo;
    public bool isOn;
    public string skillName;
    public string description;
    public string addEffects;
    public int useLevel;
    public int useMana;
    public int defaultdamage;
    public int comboDamage;
    public int heal;
    public int range;
    public float globalCoolTime;
    public float coolTime;
    public float duration;
    public float spellTime;
    public SkillType skillType;
    public int skillId;
    public int animationNum;

    public virtual void UseSkill(int id)
    {
        Debug.Log("Use Skill");
    }
}
