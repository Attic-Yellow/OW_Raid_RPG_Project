using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Custom/Quest")]
public class QuestData : ScriptableObject
{
    public string questName;//����Ʈ��(��ǥ)

    public string explainMsg; //����
    public string activingMsg; // �������� ����
    public string compelteMsg; //�����޼���

    public bool completed;  //��������
    public float requiredLevel; //���� �䱸����
    public int currentAmount; //���� �޼��� ��
    

    public int npcId; //�������ִ� npcId
    public int requiredAmount; //�޼��ؾ��� ��ǥġ
    public int targetID;

    public QuestManager.QuestPhase currentQuestPhase; //���� ����Ʈ �����Ȳ 
    public  List<int> rewardItems; //��������� �ѹ� ����Ʈ

}
