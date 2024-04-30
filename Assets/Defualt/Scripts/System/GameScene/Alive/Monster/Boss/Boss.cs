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

    public SkinnedMeshRenderer swordCollider; // 칼의 콜라이더
    public SkinnedMeshRenderer legCollider; //발차기할때 충돌 체크할 콜라이더
    public Image attackRangeImg;
    public NavMeshAgent agent;
    public Transform shotPoint; //레이저가 나가는곳
    public SkillReceiver skillReciver;

    bool underHalfHp = false; //50퍼센트 이하인지 체력이
    bool isFighting = false;
    bool attacking; //스킬 사용중
    bool isDie = false;
    float lastTime;
    float idleTime = 4f; //가만히있는시간
    float firstDeadlyAmount;
    float lastDeadlyAmount;

    [SerializeField] float floorSkillAniTime;
    [SerializeField] float radius; // 원의 반지름
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
            // 딕셔너리가 비어있을 때 처리
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
        Gizmos.color = Color.red; // 공격 범위를 표시할 색상 설정
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
            print("남은 q카운트 0");
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
        print("기본공격 추가");
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

    protected override void ManageTiredness() // 피곤함 메서드
    {
        if (currentState == State.Sleep) return;

        if (isFighting) // 전투 중에는 피곤함이 더 빨리 증가
        {
            tiredness += tirednessIncreaseRate * Time.deltaTime * 2f;
        }
        else
        {
            tiredness += tirednessIncreaseRate * Time.deltaTime;
        }
        // 피곤함 수치가 일정 수치 이상일 때 슬리핑 상태로 변경
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
                print("기본공격");
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
                    print("중간공격 추가");
                    AddMidQ();
                    return;
                    
                }

                attacking = true;

                if (deadlyQ.Count == 2 && currentHP/maxHP < firstDeadlyAmount)
                {
                    print("즉사기");
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
                
                print("중간공격");
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
                print("공격으로");
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
        tiredness -= tirednessIncreaseRate * Time.deltaTime; //피로감을 덜어줌
        
    }

    public override void TakeDamage(GameObject obj, float damage)
    {
        // 받은 데미지만큼 체력 감소
        currentHP -= damage;

        // 체력 감소를 동기화
        if (PhotonNetwork.IsConnected)
        {
            // 네트워크 연결된 경우에만 동기화
            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
        }

        // 현재 체력이 0 이하로 떨어졌을 때 처리
        if (currentHP <= 0 && isDie == false)
        {
            Die(); // 죽음 처리
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
            print("바로 공격");
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
    public void AttackFInish() //공격애니메이션끝나고 이벤트
    {
        attacking = false;
        print("공격끝");
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
            print($"차이{Vector3.Distance(transform.position, target.transform.position)}");
        }
    }

    public void FinishDie()
    {
        //TODO 몬스터 스포너한테 데이터 넘겨주고 아이템떨굼
        Destroy(gameObject,1f);
    }
    #endregion

    #region BASIC ATTACKS

    void BasicAttack0() //휘두르기
    {
        print("휘두르기");
        SetApplyRootMotion(false);
    }
    void BasicAttack1()//파이어볼
    {
        print("파이어볼");
        SetApplyRootMotion(false);
    }

    void BasicAttack2() //발차기
    {
        print("발차기");
        SetApplyRootMotion(true);
    }

   

    void BasicAttack3() //점프 공격
    {
        print("점프공격");
        SetApplyRootMotion(true );
    }

    void BasicAttack4() //콤보
    {
        print("콤보");
        SetApplyRootMotion(true ) ;
    }

    void BasicAttack5() //돌고 때려
    {
        print("돌고 떄려");
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

     IEnumerator CheckAxeCollision() //무기에 닿았는지
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
    
    IEnumerator CheckLegCollision() //다리에 닿았는지
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
    private void FloorAttack() //장판공격(몬스터 주변 설정된 범위의 원만큼 공격범위를 설정하고 랜덤으로 원의 4분의 1은 안전구역으로 설정해서 데미지를 안맞음)
    {
        SetApplyRootMotion(false);
        attackRangeImg.gameObject.SetActive(true);


        int randomNum = Random.Range(0, 4);
        switch (attackRangeImg.fillOrigin)
        {
            case 0://왼쪽,뒤
               StartCoroutine(CheckAngle( -Vector3.right, -Vector3.forward));
                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.BloodFountain].name, transform.position, transform.rotation * Quaternion.Euler(0, -180, 0));
                break;
            case 1:// 오른쪽,뒤
               StartCoroutine(CheckAngle( Vector3.right, -Vector3.forward));
                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.BloodFountain].name, transform.position, transform.rotation * Quaternion.Euler(0, 90, 0));
                break;
            case 2://오른쪽, 앞
              StartCoroutine(CheckAngle( Vector3.right, Vector3.forward));
                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.BloodFountain].name, transform.position, transform.rotation * Quaternion.identity);
                break;
            case 3://왼쪽 앞
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
                    print("안전지역x");
                    collider.GetComponent<Alive>().TakeDamage(gameObject, skillReciver.skillDic[SkillEffectEnum.BloodFountain].GetComponent<Effect>().damage);
                }
                else
                {
                    print("안전지역o");
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

    }


    private void LaserAttack()//레이저공격 (몬스터 앞으로 레이저쏘고 회전방향 랜덤 , 45도 or 90도 랜덤으로) #뒤에있어야 아예 안맞게
    {
        SetApplyRootMotion(false);


    }
    public void InitLaser()
    {
        PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.Laser].name, shotPoint.position, Quaternion.identity);
    }

    private void Thunder()//뺴빼로 공격(자기가 바라보고있는 방향으로 뭐가 떨어진 뒤 떨어진곳 양 옆에 뭐가 떨어짐)
    {
        print("번개공격");
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
        print("번개공격");
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

    private void InOutAttack() //안밖 공격 (머리위에 큐브가 빨간색이면 바깥원형에 터지고 파란색이면 안쪽원형에 데미지)
    {
     
    }
    #endregion

    #region Deadly

   public void Deadly1()
    {
        print("Deadly1 호출완료");

            float radius = 40f; // 생성할 오브젝트의 반지름
            
            for (int i = 0; i < colorList.Count; i++)
            {
                float angle = i * Mathf.PI * 2 / colorList.Count; // 360도를 오브젝트 갯수로 나누어 각도를 구함
                Vector3 offset = new Vector3(Mathf.Sin(angle), .5f, Mathf.Cos(angle)) * radius; // 각도에 따른 위치 계산

                Vector3 randomPosition = transform.position + offset; // 몬스터 주변에 오브젝트 생성

                float r = colorList[i].r;
                float g = colorList[i].g;
                float b = colorList[i].b;
                Vector3 color = new Vector3(r, g, b);

                PhotonNetwork.Instantiate(skillReciver.skillDic[SkillEffectEnum.DeadSphere].name, randomPosition, Quaternion.identity, 0, new object[] { color });
            }
        
    }

    public void Deadly2()
    {
        print("2인데?");
    }





    #endregion

  

}


