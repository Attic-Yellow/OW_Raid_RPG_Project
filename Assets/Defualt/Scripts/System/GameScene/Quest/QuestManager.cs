using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> allQuests = new(); //�������Ʈ��
    public Dictionary<int,Quest> questDIc = new();
    public List<Quest> activingQList = new(); // ���� �������� ����Ʈ
    public List<Quest> finishQList = new(); //�Ϸ��� ����Ʈ
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        foreach(Quest quest in allQuests)
        {
            questDIc.Add(quest.GetNpcId(), quest);
            if(quest.GetCurrentPhase() == QuestPhase.Activing)
            {
                activingQList.Add(quest);
            }
        }
    
    }

    public enum QuestPhase//����Ʈ �����Ȳ
    {
        None,//�����Ҽ� ������
        CanStart, //������ �����ҋ�
        Activing, //�������ϋ�
        CanFinish, //��ǥġ�� �޼�������
        Finish //����Ʈ �Ϸ��
    }

    public void PhageChange(Quest q)
    {
        
    }
   
    public void ActivingQuest(Quest q) // ����Ʈ ����
    {
        q.SetPhase(QuestPhase.Activing);
        activingQList.Add(q);
        //TODO : ȭ��� ������ ���� ���� �ؽ�Ʈ
      /*  UIManager.Instance.SetQuestText(q.GetQuestName());*/
    }
    public void CanStartQuest(Quest q)
    {
        q.SetPhase(QuestPhase.CanStart);
    
    }
    private void GiveUpQuest(Quest q)// ����Ʈ ����
    {

    }
    private void CompleteQuest(Quest q) //����Ʈ �Ϸ�
    {
        // ����Ʈ�� �Ϸ��� �� �ʿ��� ó���� ����
        q.SetComplete();
        q.SetPhase(QuestPhase.Finish);

    }

   
   


    public void GiveReward( ) //�����ִ� �޼���
    {
       //TODO �÷��̾� �κ��丮�� �ش�����Ʈ ������� �Ѱ��ִ� ���� 
        
    }


}
