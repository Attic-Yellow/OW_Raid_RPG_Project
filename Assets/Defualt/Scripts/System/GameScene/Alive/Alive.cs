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
    [SerializeField] protected float baseDamage; //�⺻���ݷ�
    [SerializeField] protected float baseDefence; //�⺻����
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
        // ���� ��������ŭ ü�� ����
        currentHP -= damage;

        // ���� ü���� 0 ���Ϸ� �������� �� ó��
        if (currentHP <= 0)
        {
            Die(); // ���� ó��
        }

        // ü�� ���Ҹ� ����ȭ
        if (PhotonNetwork.IsConnected)
        {
            // ��Ʈ��ũ ����� ��쿡�� ����ȭ
            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
        } 
    }

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
            hpSlider.value = currentHP / maxHP;
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
  
    protected GameObject HighestAggroLevel(Dictionary<int,AggroLevel> aggroDic ) //�� ������� �߿� ���� ��׷μ�ġ�� ����
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

    void TankerAggroUp(float skillAmout)  //��Ŀ�� ��׷� ��ų���� ȣ�� TODO : �÷��̾����� �Ѱ��ָ�ɵ�
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
