using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public static SkillData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public List<WarriorSkill> warriorSkills = new List<WarriorSkill>();
    public List<DragoonSkill> dragoonSkills = new List<DragoonSkill>();
    public List<BardSkill> bardSkills = new List<BardSkill>();
    public List<WhiteMageSkill> whiteMageSkills = new List<WhiteMageSkill>();
    public List<BlackMageSkill> blackMageSkills = new List<BlackMageSkill>();
}
