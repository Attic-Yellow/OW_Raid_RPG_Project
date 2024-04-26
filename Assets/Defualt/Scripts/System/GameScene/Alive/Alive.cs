using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AggroLevel
{
     private float aggroLevel;
    private float lastUpdateTime;
    public AggroLevel(float initialAggroLevel)
    {
        aggroLevel = initialAggroLevel;
        lastUpdateTime = Time.time;
    }
   
    public float GetAggroLevel()
    {
        return aggroLevel;
    }

    public float GetLastUpdateTime()
    {
        return lastUpdateTime;
    }
   

    public void SetAggroLevel(float newAggroLevel)
    {
        aggroLevel = newAggroLevel;
        lastUpdateTime = Time.time;
    }


    public void IncreaseAggroLevel(float amount)
    {
        aggroLevel += amount;
        lastUpdateTime = Time.time;
    }

    public void DecreaseAggroLevel(float amount)
    {
        aggroLevel -= amount;
        if (aggroLevel < 0)
        {
            aggroLevel = 0;
        }
        lastUpdateTime = Time.time;
    }

  
}

public class Alive : DefalutState, IPunObservable
{
    [SerializeField] protected float currentHP; //현재체력
    [SerializeField] protected float moveSpeed = 1; //이동속도
    [SerializeField] protected float baseDamage; //기본공격력
    [SerializeField] protected float baseDefence; //기본방어력
    [SerializeField] protected Slider hpSlider;

    public List<Skill> haveSkills = new();
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        UpdateHPBar();
    }

  

    public virtual void Die()
    {
        Destroy(gameObject);
    }
  
    public virtual void TakeDamage(GameObject obj, float damage)
    {
        // 받은 데미지만큼 체력 감소
        currentHP -= damage;

        // 현재 체력이 0 이하로 떨어졌을 때 처리
        if (currentHP <= 0)
        {
            Die(); // 죽음 처리
        }

        // 체력 감소를 동기화
        if (PhotonNetwork.IsConnected)
        {
            // 네트워크 연결된 경우에만 동기화
            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
        } 
    }

    [PunRPC]
    protected void SyncHealth(float health)
    {
        // 동기화된 체력 값 설정
        currentHP = health;
        UpdateHPBar();
    }
  

    protected virtual void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP / maxHP;
        }
    }
    protected virtual void Move()
    {
        
    }

    protected virtual void ReplenishHPAndMana() //체력 마나 젠
    {

    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      
    }
  
    protected GameObject HighestAggroLevel(Dictionary<int,AggroLevel> aggroDic ) //날 때린놈들 중에 제일 어그로수치가 높은
    {
        print($"{aggroDic.Count}명");
        if (aggroDic.Count == 0)
        { 
            return null;
        }

        float maxAggroLevel = float.MinValue;
        GameObject target = null;

        foreach (var kvp in aggroDic)
        {
            if(target == null && kvp.Value.GetAggroLevel() == maxAggroLevel)
            {
                maxAggroLevel = kvp.Value.GetAggroLevel();
                target = PhotonView.Find(kvp.Key)?.gameObject;
            }
            else if (kvp.Value.GetAggroLevel() > maxAggroLevel)
            {
                maxAggroLevel = kvp.Value.GetAggroLevel();
                target = PhotonView.Find(kvp.Key)?.gameObject;
            }
        }

        return target;
    }

    void TankerAggroUp(float skillAmout)  //탱커가 어그로 스킬쓰면 호출 TODO : 플레이어한테 넘겨주면될듯
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30, 1<<3);
        foreach (Collider collider in colliders)
        {
             Monster monster = collider.GetComponent<Monster>();
            if (monster.aggroLevels.ContainsKey(photonView.ViewID))
            {
                monster.aggroLevels[photonView.ViewID].IncreaseAggroLevel(skillAmout);
                monster.SetTarget(HighestAggroLevel(monster.aggroLevels));
            }
        }
    }
}
