using Photon.Pun;
using StarterAssets;
using System;


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
                ThrillOfBattle();
                break;
            case 3:
                Revenge();
                break;
            case 4:
                OneOnOne();
                break;
            case 5:
                Onslaught(id);
                break;

        }
    }

    void Defiance(int id) //�����¼�
    /* ���� �� �ڽſ� ���� �������� �ſ� ���� ����մϴ�.
 ���� �� �����˴ϴ�.
 ���ӽð�: ���� �ñ���*/
    {
        if (MovingSkillAni(id))
        {
            skillActive = !skillActive;
            float addAggroValue = 200f;

            if (!skillActive)
            {
                addAggroValue *= -1f;
            }

            Player player = GameManager.Instance.currentPlayerObj.GetComponent<Player>();
            foreach (Monster mon in player.aggroMonsters)
            {
                mon.RPCPluseAggroLevel(player.photonView.ViewID, addAggroValue);
            }
        }

    }

    void HeavySwing(int id) //������ �ϰ�
    /*��󿡰� ���� ������ ���մϴ�.
    ����: 200*/
    {
        if (IdleSkillAni(id))
        {
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().
                AddPower();
        }
    }

    void ThrillOfBattle() //������ ¥����
    {
        /*10�� ���� �ڽ��� �ִ� HP��
�޴� HP ȸ�� ȿ���� 20 % �����մϴ�.
���� ������ �ִ� HP ��� 20 % �� HP�� ȸ���մϴ�.*/
    
            GameManager.Instance.currentPlayerObj.GetComponent<PlayerSkillMethod>().BoostedHPRegen();
        

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
       if(IdleSkillAni(id)) //TODO : �ִϸ��̼� �̺�Ʈ�� �ݶ��̴� ���� �ڷ�ƾ �־����
        {

        }
    }
}
