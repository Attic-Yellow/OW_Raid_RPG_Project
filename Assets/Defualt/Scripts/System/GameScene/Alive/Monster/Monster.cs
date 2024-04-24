using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Aggressive;

public class Monster : Alive
{
    public GameObject target;
    public Dictionary<int, AggroLevel> aggroLevels = new Dictionary<int, AggroLevel>(); // ������� ��׷� �������ִ� ��ųʸ� Ű������ ����� ���̵�

    [Header("�ǰ��� ��ġ")]

    [SerializeField] protected float tiredness; // �ǰ��� ��ġ
    [SerializeField] protected float tirednessIncreaseRate;// �ǰ��� ���� �ӵ�
    [SerializeField] protected float tirednessDecreaseRate;// �ǰ��� ���� �ӵ�
    [SerializeField] protected float sleepingValue;// �ھ��ϴ� 

    protected void Update()
    {
        if (aggroLevels.Count > 0) RemoveInactiveAggroLevels();
        if (photonView.IsMine == false) return;
    }

    protected void SetAggroLevel(int bossInstanceID, float damage)
    {
        if (!aggroLevels.ContainsKey(bossInstanceID))
        {
            aggroLevels[bossInstanceID] = new AggroLevel(damage);

        }
        else
        {
            aggroLevels[bossInstanceID].IncreaseAggroLevel(damage);
        }
    }
    public void SetTarget(GameObject obj)
    {
        target = obj;
    }
    protected void RemoveInactiveAggroLevels()
    {
        List<int> keysToRemove = new List<int>();
        float passedTime = 20f; //���������� ��׷ΰ��� �ٲ���

        foreach (var kvp in aggroLevels)
        {
            if (Time.time - kvp.Value.GetLastUpdateTime() > passedTime)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (int key in keysToRemove)
        {
            aggroLevels.Remove(key);
        }
    }

    protected virtual void ManageTiredness() // �ǰ��� �޼���
    {
      
    }

    public virtual void IsFailed() //��� ���������ҽ� �ο��� �÷��̾�� ��� ���
    {
        print("���н� ��� ����");
        List<int> failsPlayersId = new List<int>();

        foreach (var kvp in aggroLevels)
        {
            failsPlayersId.Add(kvp.Key);
        }

        foreach (int playerId in failsPlayersId)
        {
            // �ش� �÷��̾��� GameObject�� �����ɴϴ�. 
            GameObject playerObject = PhotonView.Find(playerId).gameObject;

            // �ش� GameObject�� Player ��ũ��Ʈ�� ������ �ִ��� Ȯ��
            Player playerScript = playerObject.GetComponent<Player>();

            // Player ��ũ��Ʈ�� �����Ѵٸ� IsDie �޼��带 ȣ��
            if (playerScript != null)
            {
                playerScript.TakeDamage(gameObject,playerScript.MaxHP);
            }
        }
    }
}
