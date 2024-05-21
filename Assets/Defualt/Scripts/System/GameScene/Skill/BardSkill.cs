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

        foreach (var obj in GameManager.Instance.GetAlives()) //���콺 Ŀ���� �����Ǵ� ���Ͱ� �ִ��� Ȯ��
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
            //TODO : �ش� ���� ���ٰ� �ؽ�Ʈ
        }
    }

    private void PhilosophersOde() //������ ��ð�? ���ʵ��� �ֺ� �÷��̾� ũ��Ƽ�� ����
    {
        GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().PhilosophersOde();
    }

    private void Demonticrequiem() //������ ��ȥ�� ���� ����� ���� ������ ������ ����
    {
        GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().DemonticRequiem(this);
    }

}
