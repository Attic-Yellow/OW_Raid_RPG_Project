using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class BlackMageSkill : Skill
{
    public Job job;
    public override void UseSkill(int id)
    {
        switch (id)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;

        }
    }

    public void Blizard(int id) // ����ڵ�

/* ��󿡰� �����Ӽ� ���� ������ ���մϴ�.
����: 180
�ڽſ��� �ο��� õ���� ȭ���� �����ϰ�,
õ���� ȭ���� �ο����� ���� ���
15�� ���� ������ �ñ⸦ �ο��մϴ�.
*/
    {
        PhotonNetwork.Instantiate(SkillData.Instance.blackMageSkills[id].skillName, 
            GameManager.Instance.currentPlayerObj.transform.position, 
            Quaternion.Euler(GameManager.Instance.currentPlayerObj.transform.forward));
        //���� �÷��̾� ������ ��ų ����
    }

    public void ChangeProperties(int id) //�Ӽ� ��ȯ (������ �ñ� <-> õ���� ȭ��)
    {
        //TODO : ��Ÿ�� ���ư���
        isOn = !isOn;

        if (isOn)//������ �ñ�
        {
        }
        else//õ���� ȭ��
        {

        }
         
    }
}
