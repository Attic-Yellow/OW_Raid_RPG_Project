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
                HeavySwing();
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

    void Defiance() //�����¼�
    /* ���� �� �ڽſ� ���� �������� �ſ� ���� ����մϴ�.
 ���� �� �����˴ϴ�.
 ���ӽð�: ���� �ñ���*/
    {
        /*isOn = isOn ? false : true;
        if(isOn)
           {
               Player player =   CharacterData.Instance.currentCharObj.GetComponent<Player>();
               foreach(Monster mon in player.aggroMonsters)
               {
                   mon.PlusAggroLevel(player.photonView.ViewID, 300);
               }
           }*/

    }

    void HeavySwing()
    /*��󿡰� ���� ������ ���մϴ�.
    ����: 200*/
    {

     /*   ThirdPersonController controller = CharacterData.Instance.currentCharObj.GetComponent<ThirdPersonController>();
        controller.Skill(1);*/


    }
    void Berserk()
    {
      
    
    }

     void bloodwhetting()
    {

    }

     void ChaoticCyclone()
    {

    }

    void Decimate()
    {

    }

  

    void Equilibrium()
    {

    }

    void FellCleave()
    {

    }

   
}
