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
                Defiance(id);
                break;
            case 1:
                HeavySwing(id);
                break;
            case 2:
                ThrillOfBattle(id);
                break;
            case 3:
                Revenge(id);
                break;
            case 4:
                OneOnOne(id);
                break;
            case 5:
                break;

        }
    }

    void Defiance(int id) //�����¼�
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

 
    void Revenge(int id) //����
    {
        /*   15�� ���� �ڽ��� �޴� ���ط��� 30% �����ϰ�,
���� ������ ������ ��뿡�� �ݰ� ���ظ� �ݴϴ�.
�ݰ� ���� ����: 55*/

            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().SpinyArmor();
        
        //TODO : TakeDamage�޼��忡�� �������� , ���� ���� �Ű������� �޾Ƽ� �޴� �� ����ϰ�
    }

    void OneOnOne(int id) //1��1 ����
    {
        /* 10�� ���� �Ϻθ� ������ � ������ �޾Ƶ�
 �ڽ��� HP�� 1 �̸����� �������� �ʽ��ϴ�.
 ���� ������� ������ ��쿡��
 ����� �̵� �Ұ� ���·� ����ϴ�.*/

            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().invincibility();
        
        //TODO : ���� ������� ������ ��� ����� �̵� �Ұ� ���·�

    }

    bool SkillAni(int skillNum) //�ִϸ��̼��� �����ϴ� ��ų�� ��� ���� üũ �޼���
    {
        ThirdPersonController controller = GameManager.Instance.currentPlayerObj.GetComponent<ThirdPersonController>();
        return controller.Skill(skillNum);

    }


}
