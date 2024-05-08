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
    [SerializeField] protected float currentHP; //����ü��
    [SerializeField] protected float moveSpeed = 1; //�̵��ӵ�
    [SerializeField] protected float basePDamage; //�⺻���ݷ�
    [SerializeField] protected float baseMDamage; //�⺻�ֹ���
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


    #region ������ ���� �޼���

    public virtual void TakeDamage(GameObject obj, float pDamage, float physicalP /*���������*/, float mDamage, float physicalM)
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

         
        // ���� ��������ŭ ü�� ����
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
            // ��Ʈ��ũ ����� ��쿡�� ����ȭ
            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
        }

        // ���� ü���� 0 ���Ϸ� �������� �� ó��
        if (currentHP <= 0)
        {
            Die(); // ���� ó��
            return;
        }

        // ü�� ���Ҹ� ����ȭ

    }

    protected bool IsCritical()  //ũ��Ƽ�� üũ �޼���
    {
        float randomNum = Random.Range(0, 100);
        if(randomNum <= CriticalRate)
        {
            return true;
        }
        return false;
    }

    protected float ReturnDamage(float pDamage,float physicalP /*���������*/,float mDamage,float physicalM)
    {
        float pd = PDef - physicalP;
        pd = Mathf.Max(pd, 0f); //������ �����ʵ���
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
        // ����ȭ�� ü�� �� ����
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

    protected virtual void ReplenishHPAndMana() //ü�� ���� ��
    {

    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      
    }
    protected GameObject HighestAggroLevel(Dictionary<int, AggroLevel> aggroDic) //�� ������� �߿� ���� ��׷μ�ġ�� ����
    {
        print($"{aggroDic.Count}��");
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
