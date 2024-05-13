using StarterAssets;
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
    public bool skillActive;
    public string skillName;
    public string description;
    public string addEffects;
    public int useLevel;
    public int useMana;
    public int defaultdamage;
    public int comboDamage;
    public int heal;
    public int range;
    public int skillID;
    public float globalCoolTime;
    public float coolTime;
    public float duration;
    public float spellTime;
    public SkillType skillType;

    public virtual void UseSkill(int id)
    {
        Debug.Log("Use Skill");
    }

    public int GetSkillID()
    {
        return skillID;
    }

    protected bool SkillAni(int skillNum) //�ִϸ��̼��� �����ϴ� ��ų�� ��� ���� üũ �޼���
    {
        ThirdPersonController controller = GameManager.Instance.currentPlayerObj.GetComponent<ThirdPersonController>();
        return controller.Skill(skillNum);

    }

    public void ToggleSkill()
    {
        skillActive = !skillActive; // ��ų Ȱ��ȭ ���¸� ������Ŵ
    }

    public void ActivateSkill()
    {
        skillActive = true; // ��ų�� Ȱ��ȭ��
    }

    public void DeactivateSkill()
    {
        skillActive = false; // ��ų�� ��Ȱ��ȭ��
    }

}
