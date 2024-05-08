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
    [SerializeField] protected float basePDamage; //기본공격력
    [SerializeField] protected float baseMDamage; //기본주문력
    [SerializeField] protected Slider hpSlider;

    public float CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    public float BasePDamage
    {
        get { return basePDamage; }
        set { basePDamage = value; }
    }
    public float BaseMDamage
    {
        get { return baseMDamage; }
        set { baseMDamage = value; }
    }
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


    #region 데미지 관련 메서드

    public virtual void TakeDamage(GameObject obj, float pDamage, float physicalP /*물리관통력*/, float mDamage, float physicalM)
    {
        float damage = 0;

        if (IsCritical() == false)
        {
            damage = ReturnDamage(pDamage, physicalP, mDamage, physicalM);
        }
        else
        {
            float randomNum = Random.Range(2f, 3f);
            damage = ReturnDamage(pDamage,physicalP, mDamage, physicalM)*randomNum;   
        }

         
        // 받은 데미지만큼 체력 감소
        if ((CurrentHP -= damage) <= 0)
        {
            CurrentHP = 0;
        }
        else
        {
            CurrentHP -= damage;
        }


        if (PhotonNetwork.IsConnected)
        {
            // 네트워크 연결된 경우에만 동기화
            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
        }

        // 현재 체력이 0 이하로 떨어졌을 때 처리
        if (currentHP <= 0)
        {
            Die(); // 죽음 처리
            return;
        }

        // 체력 감소를 동기화

    }

    protected bool IsCritical()  //크리티컬 체크 메서드
    {
        float randomNum = Random.Range(0, 100);
        if(randomNum <= CriticalRate)
        {
            return true;
        }
        return false;
    }

    protected float ReturnDamage(float pDamage,float physicalP /*물리관통력*/,float mDamage,float physicalM)
    {
        float pd = PDef - physicalP;
        pd = Mathf.Max(pd, 0f); //음수가 되지않도록
        float p = pDamage * pd;

        float md = mDef - physicalM;
        md = Mathf.Max(md, 0f);
        float m = mDamage * md;

        return pd + md;
          
    }

    #endregion
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
            float amount = currentHP / maxHP;
            hpSlider.value = amount;
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
    protected GameObject HighestAggroLevel(Dictionary<int, AggroLevel> aggroDic) //날 때린놈들 중에 제일 어그로수치가 높은
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
            if (target == null && kvp.Value.GetAggroLevel() == maxAggroLevel)
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


 
}
