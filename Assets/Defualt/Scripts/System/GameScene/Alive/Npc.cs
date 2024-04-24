using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Npc : Alive
{
    public string greetingMessage; //일반 인사말
    public List<GameObject> marks; //물음표, 느낌표 등
    private GameObject currentMark; //현재 활성화된 마크
    public Quest quest;
    public NavMeshAgent agent;
    private new void Awake()
    {
        base.Awake();
        ChangeMark(quest.GetCurrentPhase());
    }

    private void Update()
    {
        
    }

    public void ChangeMark(QuestManager.QuestPhase phase) //머리위에 마크 변경해주는
    {
        int phaseNum = (int)phase;  
        switch (phaseNum)
        {
            case 0:
            case 4:
                if(currentMark != null)
                {
                    currentMark.SetActive(false);
                }
                break;
            case 1:
            case 2:
            case 3:
                if(currentMark != null)
                currentMark.SetActive(false);

                currentMark = marks[phaseNum-1];
                currentMark.SetActive(true);
                break;
                
        }
    }
}
