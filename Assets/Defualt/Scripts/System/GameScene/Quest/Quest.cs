using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestData qData;

    public void SetPhase(QuestManager.QuestPhase phase)
    {
        qData.currentQuestPhase = phase;    
    }
    
    public void SetCurrentAmount(int _value)
    {
        qData.currentAmount += _value;

        if (qData.currentQuestPhase == QuestManager.QuestPhase.Activing)
        {
            if (qData.currentAmount < qData.requiredAmount)
            {
                return;
            }
            SetPhase(QuestManager.QuestPhase.CanFinish);
        }
        else if (qData.currentQuestPhase == QuestManager.QuestPhase.CanFinish)
        {
            if (qData.currentAmount < qData.requiredAmount)
            {
                SetPhase(QuestManager.QuestPhase.Activing);
                return;
            }
        }
    }
    public int GetNpcId()
    {
        return qData.npcId;
    }

   public void SetComplete()
    {
        qData.completed = true;
    }
    public QuestManager.QuestPhase GetCurrentPhase()
    {
        return qData.currentQuestPhase;
    }

    public float GetRequiredLevel()
    {
        return qData.requiredLevel;
    }
    public string GetQuestName()
    {
        return qData.questName;
    }
}
