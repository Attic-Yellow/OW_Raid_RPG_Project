using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Aggressive;

public class Monster : Alive
{
    public GameObject target;
    public Dictionary<int, AggroLevel> aggroLevels = new Dictionary<int, AggroLevel>(); // ������� ��׷� �������ִ� ��ųʸ� Ű������ ����� ���̵�
    public bool isStun = false;
    public bool isMouseTrriger = false; 
    [Header("�ǰ��� ��ġ")]

    [SerializeField] protected float tiredness; // �ǰ��� ��ġ
    [SerializeField] protected float tirednessIncreaseRate;// �ǰ��� ���� �ӵ�
    [SerializeField] protected float tirednessDecreaseRate;// �ǰ��� ���� �ӵ�
    [SerializeField] protected float sleepingValue;// �ھ��ϴ� 

    protected void Update()
    {
        if (aggroLevels.Count > 1) RemoveInactiveAggroLevels();
        if (photonView.IsMine == false) return;
    }

 
    protected void SetAggroLevel(int bossInstanceID, float damage)
    {
        if (!aggroLevels.ContainsKey(bossInstanceID))
        {
            photonView.RPC("AddAggroLevels", RpcTarget.All, bossInstanceID, damage);

        }
        else
        {
            photonView.RPC("PlusAggroLevel", RpcTarget.All, bossInstanceID, damage);
        }
    }
    [PunRPC]
    public void AddAggroLevels(int ptId, float _value)
    {
        print($"�߰���{aggroLevels.Count}");
        aggroLevels[ptId] = new AggroLevel(_value);

        print($"�߰� ��{aggroLevels.Count}");
    }
    [PunRPC]
    public void RemoveAggroLevels(int ptId)
    {
        aggroLevels.Remove(ptId);
    }

    [PunRPC]
    public void PlusAggroLevel(int ptId, float _value)
    {
        aggroLevels[ptId].IncreaseAggroLevel(_value);
    }

    public void RPCPluseAggroLevel(int ptId, float _value)
    {
        photonView.RPC("PlusAggroLevel", RpcTarget.All, ptId, _value);
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
           photonView.RPC("RemoveAggroLevels",RpcTarget.All, key);
            GameObject thisObj = PhotonView.Find(key).gameObject;
            if (thisObj != null)
                thisObj.GetComponent<Player>().RemoveAggroMonster(this);
            else
            {
                print("���⼭ ����");
            }
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
                playerScript.TakeDamage(gameObject, float.MaxValue,100f,float.MaxValue,100f);
            }
        }
        aggroLevels.Clear();
    }
   
    public void Stun(bool isTrue)
    {
        if (isTrue)
        {
            isStun = true;
            animator.SetTrigger("Stun");
        }
        else
        {
            isStun = false;
        }

    }

    public virtual void AniStunFinish()
    {
        isStun = false;
        
    }

    private void OnMouseEnter()
    {
        isMouseTrriger = true;
    }

    private void OnMouseExit()
    {
        isMouseTrriger = false;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
       if(PhotonNetwork.IsMasterClient)
        {
            foreach (var kvp in aggroLevels)
            {
                photonView.RPC("AddAggroLevels", newPlayer, kvp.Key, kvp.Value.GetAggroLevel());
            }
        }
    }

}
