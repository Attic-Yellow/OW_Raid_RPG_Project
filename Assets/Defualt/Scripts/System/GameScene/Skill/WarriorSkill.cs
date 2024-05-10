using Photon.Pun;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.Editor;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson.PunDemos;


[Serializable]
public class WarriorSkill : Skill
{
    public Job job;
   
   

    public override void UseSkill(int id)
    {
       switch(id)
        {
            case 0:
                Defiance();
                break;
            case 1:
                HeavySwing(id);
                break;
            case 2:
                ThrillOfBattle(id);
                break;
            case 3:
                Revenge();
                break;
            case 4:
                OneOnOne();
                break;
            case 5:
                break;

        }
    }

    void Defiance() //수비태세
    /* 전투 중 자신에 대한 적개심이 매우 높게 상승합니다.
 재사용 시 해제됩니다.
 지속시간: 해제 시까지*/
    {
        isOn = !isOn; 
        float addAggroValue = 200f;

        if (!isOn)
        {
            addAggroValue *= -1f; 
        }

        Player player = GameManager.Instance.currentPlayerObj.GetComponent<Player>();
        foreach (Monster mon in player.aggroMonsters)
        {
            mon.RPCPluseAggroLevel(player.photonView.ViewID, addAggroValue);
        }

    }

    void HeavySwing(int id)
    /*대상에게 물리 공격을 가합니다.
    위력: 200*/
    {
        if (SkillAni(id))
        {
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().
                AddPower();
        }
    }

    void ThrillOfBattle(int id) //전투의 짜릿함
    {
        /*10초 동안 자신의 최대 HP와
받는 HP 회복 효과가 20 % 증가합니다.
실행 시점의 최대 HP 대비 20 % 의 HP를 회복합니다.*/
        if (SkillAni(id))
        {
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().BoostedHPRegen();
        }

    }

 
    void Revenge() //보복
    {
        /*   15초 동안 자신이 받는 피해량이 30% 감소하고,
물리 공격을 받으면 상대에게 반격 피해를 줍니다.
반격 피해 위력: 55*/

            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().SpinyArmor();
        
   
    }

    void OneOnOne() //1대1 결투
    {
        if(GameManager.Instance.GetAlives().Count != 0) //적을 대상으로 실행한 경우에는 대상을 이동 불가 상태로 만듭니다
        {
            GameManager.Instance.GetAlives()[0].GetComponent<Monster>().Stun(true);
        }
        else //10초 동안 일부를 제외한 어떤 공격을 받아도 자신의 HP가 1 미만으로 떨어지지 않습니다.
        {
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().invincibility();
        }
        

    }

    void Onslaught(int id) //맹공격
    {
        SkillAni(id); //TODO : 애니메이션 이벤트로 콜라이더 감지 코루틴 넣어야함

    }

    bool SkillAni(int skillNum) //애니메이션을 동반하는 스킬의 사용 가능 체크 메서드
    {
        ThirdPersonController controller = GameManager.Instance.currentPlayerObj.GetComponent<ThirdPersonController>();
        return controller.Skill(skillNum);

    }


}
