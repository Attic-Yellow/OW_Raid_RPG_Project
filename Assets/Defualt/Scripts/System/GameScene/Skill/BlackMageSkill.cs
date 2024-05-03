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

    public void Blizard(int id) // 블라자드

/* 대상에게 얼음속성 마법 공격을 가합니다.
위력: 180
자신에게 부여된 천상의 화염을 해제하고,
천상의 화염이 부여되지 않은 경우
15초 동안 저승의 냉기를 부여합니다.
*/
    {
        PhotonNetwork.Instantiate(SkillData.Instance.blackMageSkills[id].skillName, 
            GameManager.Instance.currentPlayerObj.transform.position, 
            Quaternion.Euler(GameManager.Instance.currentPlayerObj.transform.forward));
        //현재 플레이어 앞으로 스킬 생성
    }

    public void ChangeProperties(int id) //속성 전환 (저승의 냉기 <-> 천상의 화염)
    {
        //TODO : 쿨타임 돌아가게
        isOn = !isOn;

        if (isOn)//저승의 냉기
        {
        }
        else//천상의 화염
        {

        }
         
    }
}
