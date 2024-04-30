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

    void Defiance() //수비태세
    /* 전투 중 자신에 대한 적개심이 매우 높게 상승합니다.
 재사용 시 해제됩니다.
 지속시간: 해제 시까지*/
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
    /*대상에게 물리 공격을 가합니다.
    위력: 200*/
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
