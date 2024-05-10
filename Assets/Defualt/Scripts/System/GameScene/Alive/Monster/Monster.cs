using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Aggressive;

public class Monster : Alive
{
    public GameObject target;
    public Dictionary<int, AggroLevel> aggroLevels = new Dictionary<int, AggroLevel>(); // 때린놈들 어그로 관리해주는 딕셔너리 키값으로 포톤뷰 아이디
    public bool isStun = false;
    public bool isMouseTrriger = false; 
    [Header("피곤함 수치")]

    [SerializeField] protected float tiredness; // 피곤함 수치
    [SerializeField] protected float tirednessIncreaseRate;// 피곤함 증가 속도
    [SerializeField] protected float tirednessDecreaseRate;// 피곤함 감소 속도
    [SerializeField] protected float sleepingValue;// 자야하는 

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
        print($"추가전{aggroLevels.Count}");
        aggroLevels[ptId] = new AggroLevel(_value);

        print($"추가 후{aggroLevels.Count}");
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
        float passedTime = 20f; //마지막으로 어그로값이 바뀐지

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
                print("여기서 오류");
            }
        }
    }


    protected virtual void ManageTiredness() // 피곤함 메서드
    {
      
    }

    public virtual void IsFailed() //즉사 피하지못할시 싸웠던 플레이어들 모두 사망
    {
        print("실패시 모두 죽음");
        List<int> failsPlayersId = new List<int>();

        foreach (var kvp in aggroLevels)
        {
            failsPlayersId.Add(kvp.Key);
        }

        foreach (int playerId in failsPlayersId)
        {
            // 해당 플레이어의 GameObject를 가져옵니다. 
            GameObject playerObject = PhotonView.Find(playerId).gameObject;

            // 해당 GameObject가 Player 스크립트를 가지고 있는지 확인
            Player playerScript = playerObject.GetComponent<Player>();

            // Player 스크립트가 존재한다면 IsDie 메서드를 호출
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
