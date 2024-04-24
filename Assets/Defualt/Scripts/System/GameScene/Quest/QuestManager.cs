using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> allQuests = new(); //모든퀘스트들
    public Dictionary<int,Quest> questDIc = new();
    public List<Quest> activingQList = new(); // 현재 진행중인 퀘스트
    public List<Quest> finishQList = new(); //완료한 퀘스트
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

    public enum QuestPhase//퀘스트 진행상황
    {
        None,//진행할수 없을떄
        CanStart, //진행이 가능할떄
        Activing, //진행중일떄
        CanFinish, //목표치를 달성했을때
        Finish //퀘스트 완료시
    }

    public void PhageChange(Quest q)
    {
        
    }
   
    public void ActivingQuest(Quest q) // 퀘스트 진행
    {
        q.SetPhase(QuestPhase.Activing);
        activingQList.Add(q);
        //TODO : 화면상 우측에 진행 중인 텍스트
      /*  UIManager.Instance.SetQuestText(q.GetQuestName());*/
    }
    public void CanStartQuest(Quest q)
    {
        q.SetPhase(QuestPhase.CanStart);
    
    }
    private void GiveUpQuest(Quest q)// 퀘스트 포기
    {

    }
    private void CompleteQuest(Quest q) //퀘스트 완료
    {
        // 퀘스트를 완료할 때 필요한 처리를 수행
        q.SetComplete();
        q.SetPhase(QuestPhase.Finish);

    }

   
   


    public void GiveReward( ) //보상주는 메서드
    {
       //TODO 플레이어 인벤토리에 해당퀘스트 보상들을 넘겨주는 로직 
        
    }


}
