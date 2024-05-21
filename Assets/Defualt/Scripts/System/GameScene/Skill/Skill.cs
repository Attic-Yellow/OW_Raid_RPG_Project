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

    protected bool MovingSkillAni(int skillNum) //애니메이션을 동반하는 스킬의 사용 가능 체크 메서드
    {
        ThirdPersonController controller = GameManager.Instance.currentPlayerObj.GetComponent<ThirdPersonController>();
        return controller.MovingSkill(skillNum);

    }

    protected bool IdleSkillAni(int skillNum)
    {
        ThirdPersonController controller = GameManager.Instance.currentPlayerObj.GetComponent<ThirdPersonController>();
        return controller.IdleSkill(skillNum);
    }

    public void ToggleSkill()
    {
        skillActive = !skillActive; // 스킬 활성화 상태를 반전시킴
    }

    public void ActivateSkill()
    {
        skillActive = true; // 스킬을 활성화함
    }

    public void DeactivateSkill()
    {
        skillActive = false; // 스킬을 비활성화함
    }

}
