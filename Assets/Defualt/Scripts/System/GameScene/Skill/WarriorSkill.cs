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

    void Defiance() //�����¼�
    /* ���� �� �ڽſ� ���� �������� �ſ� ���� ����մϴ�.
 ���� �� �����˴ϴ�.
 ���ӽð�: ���� �ñ���*/
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
    /*��󿡰� ���� ������ ���մϴ�.
    ����: 200*/
    {
        if (SkillAni(id))
        {
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().
                AddPower();
        }
    }

    void ThrillOfBattle(int id) //������ ¥����
    {
        /*10�� ���� �ڽ��� �ִ� HP��
�޴� HP ȸ�� ȿ���� 20 % �����մϴ�.
���� ������ �ִ� HP ��� 20 % �� HP�� ȸ���մϴ�.*/
        if (SkillAni(id))
        {
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().BoostedHPRegen();
        }

    }

 
    void Revenge() //����
    {
        /*   15�� ���� �ڽ��� �޴� ���ط��� 30% �����ϰ�,
���� ������ ������ ��뿡�� �ݰ� ���ظ� �ݴϴ�.
�ݰ� ���� ����: 55*/

            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().SpinyArmor();
        
   
    }

    void OneOnOne() //1��1 ����
    {
        if(GameManager.Instance.GetAlives().Count != 0) //���� ������� ������ ��쿡�� ����� �̵� �Ұ� ���·� ����ϴ�
        {
            GameManager.Instance.GetAlives()[0].GetComponent<Monster>().Stun(true);
        }
        else //10�� ���� �Ϻθ� ������ � ������ �޾Ƶ� �ڽ��� HP�� 1 �̸����� �������� �ʽ��ϴ�.
        {
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().invincibility();
        }
        

    }

    void Onslaught(int id) //�Ͱ���
    {
        SkillAni(id); //TODO : �ִϸ��̼� �̺�Ʈ�� �ݶ��̴� ���� �ڷ�ƾ �־����

    }

    bool SkillAni(int skillNum) //�ִϸ��̼��� �����ϴ� ��ų�� ��� ���� üũ �޼���
    {
        ThirdPersonController controller = GameManager.Instance.currentPlayerObj.GetComponent<ThirdPersonController>();
        return controller.Skill(skillNum);

    }


}
