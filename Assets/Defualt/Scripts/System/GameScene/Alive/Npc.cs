using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Npc : Alive
{
    public string greetingMessage; //�Ϲ� �λ縻
    public List<GameObject> marks; //����ǥ, ����ǥ ��
    private GameObject currentMark; //���� Ȱ��ȭ�� ��ũ
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

    public void ChangeMark(QuestManager.QuestPhase phase) //�Ӹ����� ��ũ �������ִ�
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
