using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Aggressive : Monster
{
    #region VAR
    public enum DragonState
    {
        Grounded,
        Flying
    }

    public DragonState drangonState;

    public enum OnGroundState
    {
        Idle,
        Sleeping,
        Scream,
        Walk,
        Run,
        Attack0,
        Attack1,
        Attack2,
        TakeOff, //���
        GetHit,
        Die
    }

    public enum FlyingState
    {
        Idle = 10,
        Foward = 11,
        Attack = 12,
        Landing = 13 //����
    }

    public OnGroundState groundState;
    public FlyingState flyingState;
    private NavMeshAgent agent;

   
    [SerializeField] private float runSpeed;

    [SerializeField] private float attack0Range;
    [SerializeField] private float attack1Range;
    [SerializeField] private float attack2Range;
    [SerializeField] private float flyAttackRange;
    [SerializeField] private float minFlightHeight; //�ּ� ����

    [SerializeField] private float goingDistance;//�̵��� �ִ� �Ÿ�
    [SerializeField] private float defaultsensingRange; //�Ϲ��� �������� 
    [SerializeField] private float figthingSensingRange; //�οﶧ ��������
    [SerializeField] VisualEffect fireBreath;

    private bool isFighting;  //�ο��������
    private float timeCheck;
    private float waitTime;
    

    

    #endregion

    #region UNITY METHOD

    protected new void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        timeCheck = 0f;
        isFighting = false;
        drangonState = DragonState.Grounded;
        groundState = OnGroundState.Idle;
        flyingState = FlyingState.Idle;
        fireBreath.Stop();
    }

    protected new void Update()
    {
        base.Update();
        ManageTiredness();

        if (drangonState == DragonState.Grounded) //���϶�
        {
            agent.speed = groundState == OnGroundState.Run ? runSpeed : moveSpeed;

            switch (groundState)
            {
                case OnGroundState.Idle:
                    IdleState();
                    break;
                case OnGroundState.Sleeping:
                    SleepingState();
                    break;
                case OnGroundState.Scream:
                    ScreamState();
                    break;
                case OnGroundState.Walk:
                    WalkState();
                    break;
                case OnGroundState.Run:
                    RunState();
                    break;
                case OnGroundState.Attack0:
                    AttackState(attack0Range);
                    break;
                case OnGroundState.Attack1:
                    AttackState(attack1Range);
                    break;
                case OnGroundState.Attack2:
                    AttackState(attack2Range);
                    break;
                case OnGroundState.TakeOff:
                    TakeOffState();
                    break;
                case OnGroundState.GetHit:
                    GetHitState();
                    break;
                case OnGroundState.Die:
                    break;
            }

        }
        else //�ϴ��϶�
        {

            //TODO : ü���� 20�� ���ϰ� ��ų�, �Ƿ����� ������� �̻��̸� �ٴ�����
            switch (flyingState)
            {
                case FlyingState.Idle:
                    FlyingIdelState();
                    break;
                case FlyingState.Foward:
                    FlyingMoveState();
                    break;
                case FlyingState.Attack:
                    FlyingAttackState(flyAttackRange);
                    break;
                case FlyingState.Landing:
                    FlyingLandingState();
                    break;
            }
            animator.SetInteger("State", (int)flyingState);
        }
    }

    #endregion

    private void ChangeDragonState()  //dragon state�� ���� �ִϸ����� ���̾� ����
    {
        drangonState = (drangonState == DragonState.Flying) ? DragonState.Grounded : DragonState.Flying;
        timeCheck = 0;
    
    }

    private void ChangeAniLayer()
    {
        switch (drangonState)
        {
            case DragonState.Grounded:
                animator.SetLayerWeight(1, 0); // �ϴ� ���� ���̾� ��Ȱ��ȭ
                animator.SetLayerWeight(0, 1); // ���� ���̾� Ȱ��ȭ
                groundState = OnGroundState.Idle;
                animator.SetInteger("State", (int)groundState);
                break;
            case DragonState.Flying:
                animator.SetLayerWeight(0, 0); // ���� ���̾� ��Ȱ��ȭ
                animator.SetLayerWeight(1, 1); // �ϴ� ���� ���̾� Ȱ��ȭ
                flyingState = FlyingState.Idle;
                animator.SetInteger("State", (int)flyingState);
                break;
        }
    }


    protected override void ManageTiredness() // �ǰ��� �޼���
    {
        if (drangonState == DragonState.Grounded && groundState == OnGroundState.Sleeping) return;

        if (isFighting) // ���� �߿��� �ǰ����� �� ���� ����
        {
            if (drangonState == DragonState.Grounded)
                tiredness += tirednessIncreaseRate * Time.deltaTime * 2f;

            else tiredness += tirednessIncreaseRate * Time.deltaTime * 4f;
        }
        else
        {
            tiredness += tirednessIncreaseRate * Time.deltaTime;
        }
        // �ǰ��� ��ġ�� ���� ��ġ �̻��� �� ������ ���·� ����
        if (tiredness >= sleepingValue && drangonState == DragonState.Grounded && isFighting == false && groundState != OnGroundState.Idle)
        {
            ChangeGroundState(OnGroundState.Idle);
        }
        if (tiredness >= sleepingValue && drangonState == DragonState.Flying)
        {
            ChangeFlyingState(FlyingState.Idle);
        }

    }

    

    #region ON GROUND STATE
    void IdleState()
    {
        if(agent.hasPath)  agent.ResetPath();

        if (tiredness >= sleepingValue && drangonState == DragonState.Grounded && isFighting == false)
        {
            ChangeGroundState(OnGroundState.Sleeping);
            return;
        }

        if (isFighting)
        {
            ChangeGroundState(OnGroundState.Scream);
            return;
        }

        if (timeCheck == 0)
        {
            timeCheck = Time.time;
            waitTime = Random.Range(1, 4);
            print(waitTime);
            return;
        }

        if (timeCheck <= Time.time - waitTime)
        {
            ChangeGroundState(OnGroundState.Walk);
            timeCheck = 0f;
        }
    }
    void WalkState()
    {
        if (!agent.hasPath || agent.remainingDistance < agent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * goingDistance;
            randomDirection.y = 0;
            randomDirection += transform.position; //TODO : �ڽŸ��� �Ʒ��Ƹ� ������
            NavMeshHit hit;
            bool isFind = false;
            while (!isFind)
            {
                if (NavMesh.SamplePosition(randomDirection, out hit, goingDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    isFind = true;
                }
            }
        }

        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, transform.forward, out rayHit, defaultsensingRange, 1 << 5)) //���� ���� ������� ���߰�
        {
            print("�����־�");
            ChangeGroundState(OnGroundState.Idle);
            return;
        }
    }

    void SleepingState()
    {
        agent.ResetPath();
        tiredness -= tirednessIncreaseRate * Time.deltaTime; //�Ƿΰ��� ������
        if (tiredness <= 0) ChangeGroundState(OnGroundState.Idle);

    }

    void ScreamState()
    {

    }

    public void ScreamFinish() //��ȿ�� ��������
    {
        print("��ȿ ��");
        if (currentHP / maxHP <= 0.5) //ü���� 50�ۼ�Ʈ �����̸�
        {
            ChangeGroundState(OnGroundState.TakeOff);
            return;
        }

            ChangeGroundState(OnGroundState.Run);
  
        
    }

    void RunState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); //������
        if (!stateInfo.IsName("Run")) return;
        
        if (target != null) agent.SetDestination(target.transform.position);

        GameObject obj = HighestAggroLevel(aggroLevels);

        if (obj != null)
        {
            target = obj;
            agent.SetDestination(obj.transform.position);
        }
        else
        {
            agent.ResetPath();
            ChangeGroundState(OnGroundState.Walk);
            isFighting = false;
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < agent.stoppingDistance)
            ChangeGroundState((OnGroundState)Random.Range(5, 8));
    }

    public void FireBreath()
    {
        print("ȣ��");
        photonView.RPC("PlayFireBreathAnimation", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PlayFireBreathAnimation()
    {
        fireBreath.Play();
    }

    void AttackState(float range) //��ų ������ ���� ���̸� ���� ���� �� ������������
    {
        if (target == null) target = HighestAggroLevel(aggroLevels);
        if(target == null) LoseTarget();

        if(agent.hasPath) agent.ResetPath();

        //TODO : ���������� ������ ������ �Ұ���
        RaycastHit[] hits;
        Debug.DrawRay(transform.position, transform.forward * range, Color.blue);
        hits = Physics.RaycastAll(transform.position, transform.forward, range, 1 << 3);
        foreach (RaycastHit hit in hits)
        {
            print($"������{hit.collider.gameObject.name}");
            hit.collider.gameObject.GetComponent<Alive>().TakeDamage(gameObject, Power+basePDamage,pPhy,Power+baseMDamage,mPhy);
        }
    }

    public void AttackFinish() //���ݳ����� ������Ʈ�ٲ��ִ�
    {
        print("���ݳ�");

        if (currentHP / maxHP <= 0.5) //ü���� 50�ۼ�Ʈ �����̸�
        {
            ChangeGroundState(OnGroundState.TakeOff);
            return;
        }
        ChangeGroundState(OnGroundState.Run);
    }

    void TakeOffState()
    {
        if (animator.applyRootMotion == false) animator.applyRootMotion = true;
    }
    public void FinishTakeoff()
    {
        ChangeGroundState(OnGroundState.Idle);
        ChangeDragonState();
    }

    void GetHitState()
    {
        agent.ResetPath();
    }
    public void FinishGetHit() //�´� �ִϸ��̼� �������� �߻��ϴ� �̺�Ʈ
    {
        // ���� ü���� 0 ���Ϸ� �������� �� ó��
        if (currentHP <= 0 && groundState != OnGroundState.Die)
        {
            ChangeGroundState(OnGroundState.Die);
            return;
        }
         ChangeGroundState(OnGroundState.Scream);
    }
    [PunRPC]
    void SyncGroundState(int state)
    {
        groundState = (OnGroundState)state;
        animator.SetInteger("State", (int)state);
        timeCheck = 0f;
    }
    void ChangeGroundState(OnGroundState state)
    {
        photonView.RPC("SyncGroundState", RpcTarget.All, (int)state);
        print(state.ToString());
    }

    #endregion

    #region ON SKY STATE

 
    void FlyingIdelState()
    {
        
        if ((tiredness >= sleepingValue && drangonState == DragonState.Flying) || currentHP/maxHP <= 0.25 || HighestAggroLevel(aggroLevels) == null)
        {
           ChangeFlyingState(FlyingState.Landing);
            return;
        }
        if (timeCheck == 0)
        {
            timeCheck = Time.time;
            waitTime = Random.Range(1, 4);
            print(waitTime);
            return;
        }

        if (timeCheck <= Time.time - waitTime)
        {
            ChangeFlyingState(FlyingState.Foward);
            timeCheck = 0f;
        }

    }
    void FlyingMoveState()
    {
        target = HighestAggroLevel(aggroLevels);
        if (target != null)
        {
            transform.LookAt(target.transform.position);
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.Translate(new Vector3(direction.x, transform.position.y, direction.z) * moveSpeed * Time.deltaTime);
        }
        else
        {
            ChangeFlyingState(FlyingState.Idle);
        }
        if (Vector3.Distance(target.transform.position, transform.position) <= defaultsensingRange)
        {
            ChangeFlyingState(FlyingState.Attack);
        }
    }
    void FlyingAttackState(float range)
    {
        agent.ResetPath();
        //TODO : ���������� ������ ������ �Ұ���
        RaycastHit[] hits;
        Debug.DrawRay(transform.position, transform.forward * range, Color.blue);
        hits = Physics.RaycastAll(transform.position, transform.forward, range, 1 << 3);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null) return;
            print($"������{hit.collider.gameObject.name}");
            hit.collider.gameObject.GetComponent<Player>().TakeDamage(gameObject, 0f,0f,Power+baseMDamage,mPhy);
        }
    }

    void FlyingLandingState()
    {

    }

    public void FlyingAttackFinish()
    {
       ChangeFlyingState(FlyingState.Idle);
    }

    public void FinishLand()
    {
        ChangeFlyingState(FlyingState.Idle);
        animator.applyRootMotion = false;
        ChangeDragonState();
    }


    [PunRPC]
    void SyncFlyingState(int state)
    {
      flyingState = (FlyingState)state;
    }
    void ChangeFlyingState(FlyingState state)
    {
        photonView.RPC("SyncFlyingState", RpcTarget.All, (int)state);
        animator.SetInteger("State",(int)flyingState);
        timeCheck = 0f;
    }

    #endregion

  

    public override void TakeDamage(GameObject obj, float pDamage, float physicalP /*���������*/, float mDamage, float physicalM)
    {
         isFighting = true; //Ŀ���� ������Ƽ��
        float damage = 0;

        if (IsCritical() == false)
        {
            damage = ReturnDamage(pDamage, physicalP, mDamage, physicalM);
        }
        else
        {
            float randomNum = Random.Range(2f, 3f);
            damage = ReturnDamage(pDamage, physicalP, mDamage, physicalM) * randomNum;
        }


        currentHP -= damage;
        SetAggroLevel(obj.GetComponent<PhotonView>().ViewID, damage);
        target = HighestAggroLevel(aggroLevels);

        // ü�� ���� ����ȭ
        if (PhotonNetwork.IsConnected) photonView.RPC("SyncHealth", RpcTarget.All, currentHP);

        if (target == null)
        {
            LoseTarget();
        }
            if (drangonState == DragonState.Grounded && groundState != OnGroundState.GetHit 
            && (groundState == OnGroundState.Walk || groundState == OnGroundState.Idle))
        {
            ChangeGroundState(OnGroundState.GetHit);
        }
     
    }

    void LoseTarget()//Ÿ���� �Ҿ��������
    {
        
            if (drangonState == DragonState.Flying)
            {
                fireBreath.Stop();
                animator.SetTrigger("Change");
            }
            else
            {
                ChangeGroundState(OnGroundState.Walk);
            }
        
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)drangonState);
            stream.SendNext((int)groundState);
            stream.SendNext((int)flyingState);
            stream.SendNext(currentHP);
            stream.SendNext(isFighting);
            stream.SendNext((float)tiredness);
        }
        else
        {
            drangonState = (DragonState)stream.ReceiveNext();
            groundState = (OnGroundState)stream.ReceiveNext();
            flyingState = (FlyingState)stream.ReceiveNext();
            currentHP = (float)stream.ReceiveNext();
            isFighting = (bool)stream.ReceiveNext();
            tiredness = (float)stream.ReceiveNext();

            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
          
        }
    }
}


