using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Custom/Quest")]
public class QuestData : ScriptableObject
{
    public string questName;//퀘스트명(목표)

    public string explainMsg; //설명
    public string activingMsg; // 끝났는지 묻는
    public string compelteMsg; //성공메세지

    public bool completed;  //성공여부
    public float requiredLevel; //레벨 요구사항
    public int currentAmount; //현재 달성한 양
    

    public int npcId; //가지고있는 npcId
    public int requiredAmount; //달성해야할 목표치
    public int targetID;

    public QuestManager.QuestPhase currentQuestPhase; //현재 퀘스트 진행상황 
    public  List<int> rewardItems; //보상아이템 넘버 리스트

}
