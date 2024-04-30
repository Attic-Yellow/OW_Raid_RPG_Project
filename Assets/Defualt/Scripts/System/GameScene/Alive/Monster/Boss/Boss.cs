using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Boss : Monster
{
   
    List<Action> attackList = new List<Action>();
    List<Action> midAttackList = new List<Action>();

    Queue<int> basicQ = new();
    Queue<int> midQ = new();
    Queue<Action> deadlyQ = new();
    Dictionary<int, Action> attackDic = new();

    [SerializeField] List<Color> colorList = new List<Color>();
    Queue<Color> colorQueue = new Queue<Color>();
    Coroutine coroutine = null;

    public TextMeshProUGUI testText;
    

    [Header("Need Drag")]

    public SkinnedMeshRenderer swordCollider; // Į�� �ݶ��̴�
    public SkinnedMeshRenderer legCollider; //�������Ҷ� �浹 üũ�� �ݶ��̴�
    public Image attackRangeImg;
    public NavMeshAgent agent;
    public Transform shotPoint; //�������� �����°�
    public SkillReceiver skillReciver;

    bool underHalfHp = false; //50�ۼ�Ʈ �������� ü����
    bool isFighting = false;
    bool attacking; //��ų �����
    bool isDie = false;
    float lastTime;
    float idleTime = 4f; //�������ִ½ð�
    float firstDeadlyAmount;
    float lastDeadlyAmount;

    [SerializeField] float floorSkillAniTime;
    [SerializeField] float radius; // ���� ������
    [SerializeField] float attackRange;
    [SerializeField] Material weaponMaterial;
    enum State
    {
        Idle,
        Walk,
        Sleep,
        Attack
    }
  
    [SerializeField] State currentState;

  


    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            aggroLevels.Clear();
        }

        firstDeadlyAmount = Random.Range(0.4f, 0.6f);
        lastDeadlyAmount = Random.Range(0.15f, 0.3f);

        AddMidList();
        AddAttackList();

        AddBasicQ(); 
        AddMidQ();

        AddAttackDic();

        deadlyQ.Enqueue(Deadly1);
        deadlyQ.Enqueue(Deadly2);

        currentState = State.Idle;
   /*     agent.areaMask |= 1 << 1;*/
        lastTime = Time.time;
 
    }



    protected new void Update()
    {
        var firstKvp = aggroLevels.FirstOrDefault();
        var secondKvp = aggroLevels.Skip(1).FirstOrDefault();

        if (aggroLevels.Count > 1)
        {
            testText.text = $"First Key: {firstKvp.Key}, First Value: {firstKvp.Value}\n" +
                            $"Second Key: {secondKvp.Key}, Second Value: {secondKvp.Value}";
        }
        else
        {
            // ��ųʸ��� ������� �� ó��
            testText.text = "Dictionary is nujl.";
        }

        if (isDie) return;
        base.Update();
        ManageTiredness();

        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Walk:
                WalkState();
                break;
            case State.Sleep:
                SleepingState();
                break;
            case State.Attack:
                if (attacking) return;
                Attack();
                break;
        }
        
       animator.SetInteger("State",(int)currentState);
        animator.SetBool("IsFighting", isFighting);
   
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // ���� ������ ǥ���� ���� ����
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    #region OTHER
    void PerformMidAttack(Color newColor)
    {
        while (colorQueue.Contains(newColor))
        {
            newColor = RandomColor();
        }

        weaponMaterial.color = newColor;
        colorQueue.Enqueue(newColor);

        if (colorQueue.Count > 3)
        {
            colorQueue.Dequeue();
        }
    }

    Color RandomColor()
    {
        int randomNum = Random.Range(0, colorList.Count);
        
        return colorList[randomNum];
    }
    public bool IsOkColorCheck(Color color)
    {
       if(colorQueue.Contains(color))
        {
            IsFailed();
        }
        print("IsOkColorCheck");
        return true;
        
    }

    public bool ColorQCheck(Color color)
    {
        print("ColorQCheck");
        if (colorQueue.Count == 0)
        {
            print("���� qī��Ʈ 0");
        }
        if (color != colorQueue.Dequeue())
        {
            IsFailed();
        }
        return true;
    }

    void AddAttackDic()
    {
        for (int i = 0; i < attackList.Count; i++)
        {
            attackDic.Add(i, attackList[i]);
        }
        for (int j = attackList.Count; j < midAttackList.Count + attackList.Count; j++)
        {
            attackDic.Add(j, midAttackList[j - attackList.Count]);
        }
    }


    void AddAttackList()
    {
        attackList.Add(BasicAttack0);
        attackList.Add(BasicAttack1);
        attackList.Add(BasicAttack2);
        attackList.Add(BasicAttack3);
        attackList.Add(BasicAttack4);
        attackList.Add(BasicAttack5);
    }

    void AddBasicQ()
    {
        print("�⺻���� �߰�");
        List<int> numList = new();
        for (int i = 0; i < attackList.Count; i++)
        {
            numList.Add(i);
        }
        while (numList.Count > 0)
        {
            int randomIndex = Random.Range(0, numList.Count);
            basicQ.Enqueue(numList[randomIndex]);
            numList.RemoveAt(randomIndex);
        }
    }

    void AddMidList()
    {
        midAttackList.Add(LaserAttack);
        midAttackList.Add(FloorAttack);
         midAttackList.Add(Thunder);
          midAttackList.Add(MagicBall);
        
    }

    void AddMidQ()
    {
        List<int> numList = new();
        for (int i = attackList.Count; i < midAttackList.Count + attackList.Count; i++)
        {
            numList.Add(i);
        }
        while (numList.Count > 0)
        {
            int randomIndex = Random.Range(0, numList.Count);
            midQ.Enqueue(numList[randomIndex]);
            numList.RemoveAt(randomIndex);
        }
    }

    protected override void ManageTiredness() // �ǰ��� �޼���
    {
        if (currentState == State.Sleep) return;

        if (isFighting) // ���� �߿��� �ǰ����� �� ���� ����
        {
            tiredness += tirednessIncreaseRate * Time.deltaTime * 2f;
        }
        else
        {
            tiredness += tirednessIncreaseRate * Time.deltaTime;
        }
        // �ǰ��� ��ġ�� ���� ��ġ �̻��� �� ������ ���·� ����
        if (tiredness >= sleepingValue && isFighting == false && currentState != State.Idle)
        {
            currentState = State.Idle;
        }

    }
    #endregion

    #region STATE

    void Attack()
    {
        if (agent.hasPath) agent.ResetPath();

        if (underHalfHp == false)
        {
            if (basicQ.Count > 3)
            {
                print("�⺻����");
                attacking = true;
                animator.SetInteger("AttackState", basicQ.Peek());
                print($"basicQ{attackDic[basicQ.Peek()]}");
                attackDic[basicQ.Dequeue()].Invoke();
                return;
            }
            else
            {

                if (midQ.Count == 0)
                {
                    print("�߰����� �߰�");
                    AddMidQ();
                    return;
                    
                }

                attacking = true;

                if (deadlyQ.Count == 2 && currentHP/maxHP < firstDeadlyAmount)
                {
                    print("����");
                    deadlyQ.Dequeue().Invoke();
                    animator.SetInteger("AttackState", 11);
                    return;
                }
                if (deadlyQ.Count == 1 && currentHP < lastDeadlyAmount)
                {
                   deadlyQ.Dequeue().Invoke();
                    animator.SetInteger("AttackState", 12);
                    return;
                }

                PerformMidAttack(RandomColor());
                
                print("�߰�����");
                int midQNum = midQ.Dequeue();
                animator.SetInteger("AttackState", midQNum);
                print($"midPeek{midQNum}");
                attackDic[midQNum].Invoke();

                basicQ.Clear();
                AddBasicQ();
            }
        }
        else
        {

        }


    }

    void IdleState()
    {
        if (agent.hasPath) agent.ResetPath();

        if (tiredness >= sleepingValue && attacking == false)
        {
            currentState = State.Sleep;
            return;
        }

        if (lastTime + idleTime <= Time.time ) currentState = State.Walk;
            
    }

    void WalkState()
    {
        if(agent.pathPending  && agent.remainingDistance < agent.stoppingDistance && isFighting == false)
        {
            currentState = State.Idle;
            return;
        }

        if (isFighting && target != null)
        {
            agent.SetDestination(target.transform.position);
            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                agent.ResetPath();
                currentState = State.Attack;
                print("��������");
                return;
            }
           
        }

        while (!agent.hasPath)
        {
            float canGoingDistance = 50f;
            Vector3 randomDirection = Random.insideUnitSphere * canGoingDistance;
            randomDirection.y = 0;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, canGoingDistance, agent.areaMask))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }
    }

    void SleepingState()
    {
        if (tiredness <= 0)
        {
            tiredness = 0;
            currentState = State.Idle;
            return;
        }
        tiredness -= tirednessIncreaseRate * Time.deltaTime; //�Ƿΰ��� ������
        
    }

    public override void TakeDamage(GameObject obj, float damage)
    {
        // ���� ��������ŭ ü�� ����
        currentHP -= damage;

        // ü�� ���Ҹ� ����ȭ
        if (PhotonNetwork.IsConnected)
        {
            // ��Ʈ��ũ ����� ��쿡�� ����ȭ
            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
        }

        // ���� ü���� 0 ���Ϸ� �������� �� ó��
        if (currentHP <= 0 && isDie == false)
        {
            Die(); // ���� ó��
            return;
        }

       if(obj.GetComponent<Player>() != null)
        {
            obj.GetComponent<Player>().AddAggroMonster(this);
        }

        SetAggroLevel(obj.GetComponent<PhotonView>().ViewID, damage);
        target = HighestAggroLevel(aggroLevels);
        isFighting = true;

        if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            if (currentState != State.Walk) currentState = State.Walk;
        }
        else
        {
            print("�ٷ� ����");
            if(agent.hasPath) agent.ResetPath();

            currentState = State.Attack;
        }
        
        
    }


    public override void Die()
    {
        isDie = true;
        if(agent.hasPath) agent.path = null;
        animator.SetTrigger("Die");
    }
    public void AttackFInish() //���ݾִϸ��̼ǳ����� �̺�Ʈ
    {
        attacking = false;
        print("���ݳ�");
        if(attackRangeImg.gameObject.activeSelf) attackRangeImg.gameObject.SetActive(false);

        if(target == null)
        {
            currentState = State.Walk;
            animator.SetInteger("State", (int)currentState);
            return;
        }
        if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            currentState = State.Walk;
            animator.SetInteger("State", (int)currentState);
            print($"����{Vector3.Distance(transform.position, target.transform.position)}");
        }
    }

    public void FinishDie()
    {
        //TODO ���� ���������� ������ �Ѱ��ְ� �����۶���
        Destroy(gameObject,1f);
    }
    #endregion

    #region BASIC ATTACKS

    void BasicAttack0() //�ֵθ���
    {
        print("�ֵθ���");
        SetApplyRootMotion(false);
    }
    void BasicAttack1()//���̾
    {
        print("���̾");
        SetApplyRootMotion(false);
    }

    void BasicAttack2() //������
    {
        print("������");
        SetApplyRootMotion(true);
    }

   

    void BasicAttack3() //���� ����
    {
        print("��������");
        SetApplyRootMotion(true );
    }

    void BasicAttack4() //�޺�
    {
        print("�޺�");
        SetApplyRootMotion(true ) ;
    }

    void BasicAttack5() //���� ����
    {
        print("���� ����");
        SetApplyRootMotion(true) ;
    }
    void SetApplyRootMotion(bool isTrue)
    {
       animator.applyRootMotion = isTrue;
    }

    public void StartCheckAxe()
    {
      coroutine =  StartCoroutine(CheckAxeCollision());
    }

    public void StartCheckLeg()
    {
      coroutine =  StartCoroutine(CheckLegCollision());
    }

     IEnumerator CheckAxeCollision() //���⿡ ��Ҵ���
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapBox(swordCollider.bounds.center, swordCollider.bounds.extents, swordCollider.transform.rotation, 1 << 3);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == gameObject) continue;

                col.GetComponent<Alive>().TakeDamage(gameObject, power);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator CheckLegCollision() //�ٸ��� ��Ҵ���
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapBox(legCollider.bounds.center, legCollider.bounds.extents, legCollider.transform.rotation, 1 << 3);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject == gameObject) continue;

                col.GetComponent<Alive>().TakeDamage(gameObject, power);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void FinishCheckAxe()
    {
        if(coroutine != null)    StopCoroutine(coroutine);
    }

    public void FinishCheckLeg()
    {
       if(coroutine!=null) StopCoroutine(coroutine);
       
    }
    public void FireBall()
    {
        PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.Fireball].name, shotPoint.position, Quaternion.identity);
    }
  

    #endregion


    #region MIDATTACKS
    private void FloorAttack() //���ǰ���(���� �ֺ� ������ ������ ����ŭ ���ݹ����� �����ϰ� �������� ���� 4���� 1�� ������������ �����ؼ� �������� �ȸ���)
    {
        SetApplyRootMotion(false);
        attackRangeImg.gameObject.SetActive(true);


        int randomNum = Random.Range(0, 4);
        switch (attackRangeImg.fillOrigin)
        {
            case 0://����,��
               StartCoroutine(CheckAngle( -Vector3.right, -Vector3.forward));
                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.BloodFountain].name, transform.position, transform.rotation * Quaternion.Euler(0, -180, 0));
                break;
            case 1:// ������,��
               StartCoroutine(CheckAngle( Vector3.right, -Vector3.forward));
                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.BloodFountain].name, transform.position, transform.rotation * Quaternion.Euler(0, 90, 0));
                break;
            case 2://������, ��
              StartCoroutine(CheckAngle( Vector3.right, Vector3.forward));
                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.BloodFountain].name, transform.position, transform.rotation * Quaternion.identity);
                break;
            case 3://���� ��
               StartCoroutine(CheckAngle( -Vector3.right, Vector3.forward));
                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.BloodFountain].name, transform.position, transform.rotation * Quaternion.Euler(0, -90, 0));
                break;
            default:
                break;
        }
       
        attackRangeImg.fillOrigin = randomNum;
    }


    IEnumerator CheckAngle(Vector3 right, Vector3 forward)
    {
       
        float elapsedTime = 0f;

        while (elapsedTime < floorSkillAniTime)
        {
        
            elapsedTime += Time.deltaTime;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 3);

            foreach (Collider collider in colliders)
            {
                if (collider == GetComponent<Collider>())
                {
                    continue;
                }
                print(collider.gameObject.name);
                Vector3 directionToCollider = collider.transform.position - transform.position;
                bool isXBool = right.x < 0 ? directionToCollider.x < 0 : directionToCollider.x > 0;
                bool isZBool = forward.z < 0 ? directionToCollider.z < 0 : directionToCollider.z > 0;

                if (!isXBool || !isZBool)
                {
                    print("��������x");
                    collider.GetComponent<Alive>().TakeDamage(gameObject, skillReciver.skillDic[SkillEffectEnum.BloodFountain].GetComponent<Effect>().damage);
                }
                else
                {
                    print("��������o");
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

    }


    private void LaserAttack()//���������� (���� ������ ��������� ȸ������ ���� , 45�� or 90�� ��������) #�ڿ��־�� �ƿ� �ȸ°�
    {
        SetApplyRootMotion(false);


    }
    public void InitLaser()
    {
        PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.Laser].name, shotPoint.position, Quaternion.identity);
    }

    private void Thunder()//������ ����(�ڱⰡ �ٶ󺸰��ִ� �������� ���� ������ �� �������� �� ���� ���� ������)
    {
        print("��������");
        SetApplyRootMotion(false);
    }

    public void InitThunder()
    {
        
        for (int i = 0; i < skillReciver.skillDic[SkillEffectEnum.Thunder].GetComponent<Effect>().createCount; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * 40;
            randomPosition.y = 20;
            PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.Thunder].name, transform.position + randomPosition, Quaternion.identity);
        }
    }

    private void MagicBall()
    {
        print("��������");
        SetApplyRootMotion(false);
        InitMagicBall();
    }

    public void InitMagicBall()
    {
        float distance = 20f;
        for (int i = 0; i < skillReciver.skillDic[SkillEffectEnum.MagicBall].GetComponent<Effect>().createCount; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * (i* distance);
            randomPosition += transform.position;
            GameObject magicBall = PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.MagicBall].name, randomPosition, Quaternion.identity);
            magicBall.transform.parent = transform;
        }
    }

    private void InOutAttack() //�ȹ� ���� (�Ӹ����� ť�갡 �������̸� �ٱ������� ������ �Ķ����̸� ���ʿ����� ������)
    {
     
    }
    #endregion

    #region Deadly

   public void Deadly1()
    {
        print("Deadly1 ȣ��Ϸ�");

            float radius = 40f; // ������ ������Ʈ�� ������
            
            for (int i = 0; i < colorList.Count; i++)
            {
                float angle = i * Mathf.PI * 2 / colorList.Count; // 360���� ������Ʈ ������ ������ ������ ����
                Vector3 offset = new Vector3(Mathf.Sin(angle), .5f, Mathf.Cos(angle)) * radius; // ������ ���� ��ġ ���

                Vector3 randomPosition = transform.position + offset; // ���� �ֺ��� ������Ʈ ����

                float r = colorList[i].r;
                float g = colorList[i].g;
                float b = colorList[i].b;
                Vector3 color = new Vector3(r, g, b);

                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.DeadSphere].name, randomPosition, Quaternion.identity, 0, new object[] { color });
            }
        
    }

    public void Deadly2()
    {
        print("2�ε�?");
    }





    #endregion

  

}


