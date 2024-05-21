using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BardSkill : Skill
{
    public Job job;

    public override void UseSkill(int id)
    {
        switch (id)
        {
            case 0:
                PoisonArrow(id);
                break;
            case 1:
                PhilosophersOde();
                break;
            case 2:
                Demonticrequiem();
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;

        }
    }

    private void PoisonArrow(int id)
    {
        bool isMonster = false;

        foreach (var obj in GameManager.Instance.GetAlives()) //마우스 커서에 감지되는 몬스터가 있는지 확인
        {
            if(obj.GetComponent<Monster>() != null)
            {
                isMonster = true;
                return;
            }
        }
       if(isMonster)
        {
            if (MovingSkillAni(id))  GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().PosionArrow(GameManager.Instance.GetAlives()[0].GetComponent<Monster>());
        }      
        else
        {
            //TODO : 해당 적이 없다고 텍스트
        }
    }

    private void PhilosophersOde() //현자의 담시곡? 몇초동안 주변 플레이어 크리티컬 증가
    {
        GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().PhilosophersOde();
    }

    private void Demonticrequiem() //마인의 진혼곡 가장 가까운 몬스터 데미지 증가량 증가
    {
        GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().DemonticRequiem(this);
    }

}
