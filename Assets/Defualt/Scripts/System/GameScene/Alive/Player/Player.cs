using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System;
/*using Cinemachine;*/
using TMPro;
using Cinemachine;

public class Player : Alive
{
      public List<Monster> aggroMonsters;
      public  List<Monster> monsters = new(); //테스트용 공격할 몬스터들
      private float healingRate;
    private bool isInvincibility = false; //무적

    public bool IsInvincibility
    {
        get { return isInvincibility; }
        set { isInvincibility = value; }
    }
      public float HealingRate
    {
        get { return healingRate; }
        set { healingRate = value; }
    }


      #region UintyMethod
      private new void Awake()
      {
          base.Awake();
        CinemachineVirtualCamera cvc = FindObjectOfType<CinemachineVirtualCamera>();
        if (cvc != null && GameManager.Instance.currentPlayerObj != null)
        {
            cvc.LookAt = GameManager.Instance.currentPlayerObj.transform;
        }

      }
      private void Start()
      {
        if (photonView.IsMine)
        {
            /*    List<Skill> list = SkillManager.instance.GetSkillList(playerJob);
                 foreach(Skill skill in list)
                 {
                    canLearnSkills.Enqueue(skill);
                 }
                 cam.Follow = transform;
                 cam.LookAt = transform;
             }*/
        }
      }

        void Update()
      {

          if (photonView.IsMine) //내 캐릭터 일 경우
          {
             if(Input.GetKeyDown(KeyCode.Escape))
              {
                foreach (Monster mon in monsters)
                {
                    mon.TakeDamage(gameObject, Power+basePDamage,pPhy,Power+baseMDamage,mPhy);
                }
              }
          }
      }


    #endregion
    #region overridMethod
    public override void TakeDamage(GameObject obj, float pDamage, float physicalP /*물리관통력*/, float mDamage, float physicalM)
    {
        if (pDamage > 0 && GetComponent<PlayerSkillMethod>().isRevenging) //반격중이면 공격한 대상 물리고정뎀 주기
        {
            print("반격데미지!");
            obj.GetComponent<Monster>().
                TakeDamage(gameObject, GetComponent<PlayerSkillMethod>().GetRevengeDamage(),
                 obj.GetComponent<Monster>().PDef, 0, 0);
         }
        float damage = 0;

        if (IsCritical() == false)
        {
            damage = ReturnDamage(pDamage, physicalP, mDamage, physicalM);
        }
        else
        {
            float randomNum = UnityEngine.Random.Range(2f, 3f);
            damage = ReturnDamage(pDamage, physicalP, mDamage, physicalM) * randomNum;
        }


        if (isInvincibility)
        {
            CurrentHP -= damage;

            if ((CurrentHP -= damage) <= 0)
            {
                CurrentHP = 1f;
            }
        }
        else
        {
            if ((CurrentHP -= damage) < 0)
            {
                CurrentHP = 0;
            }
            else
            {
                CurrentHP -= damage;
            }

            // 현재 체력이 0 이하로 떨어졌을 때 처리
            if (currentHP <= 0)
            {
                Die(); // 죽음 처리
                return;
            }
        }

        if (PhotonNetwork.IsConnected)
        {
            // 네트워크 연결된 경우에만 동기화
            photonView.RPC("SyncHealth", RpcTarget.All, currentHP);
        }
    }

    #endregion
    #region customMethod


    public void AddPlayerSkill(Skill skill)
      {
          foreach(var s in haveSkills)
          {
              if(s == null)
              {
                  skill = s;
                  break;
              }
          }
      }
    public void AddAggroMonster(Monster monster)
    {
        bool isHave = false;
        foreach (Monster mon in aggroMonsters)
        {
            if(mon == monster) isHave = true;
        }
        if(isHave == false)   aggroMonsters.Add(monster);
    }

    public void RemoveAggroMonster(Monster monster)
    {
        aggroMonsters.Remove(monster);
    }

     /* public void InitializePlayerInfo(User.Job job, string nickName)
      {
          playerJob = job;
          SetJob(job);
          Level = 1;
          money = 0f;
          charName = nickName;
      }
      public void LevelUp()
      {
          level++;

          for (int i = 0; i < QuestManager.instance.allQuests.Count; i++)
          {
            if(QuestManager.instance.questDIc[i].GetRequiredLevel() == level)
              {
                  QuestManager.instance.questDIc[i].SetPhase(QuestManager.QuestPhase.CanStart);
                  GameObject obj =  ObjManager.instance.FindObj(QuestManager.instance.questDIc[i].qData.npcId);
                  Npc npc = obj.GetComponent<Npc>();
                  npc.ChangeMark(npc.quest.GetCurrentPhase());
              }
          }
          if(level>=5 && level % 5 ==0 && canLearnSkills.Count != 0)
          {
              haveSkills.Add(canLearnSkills.Dequeue());
              UIManager.Instance.ChangeSkillImg(haveSkills[haveSkills.Count - 1].GetImg());
          }

          StatusUp();
      }*/



      public void SetStatus(float v0, float v1, float v2, float v3, float v4, float v5, float v6, float v7
          , float v8, float v9, float v10, float v11, float v12, float v13, float v14)
      {
          Power = v0;
          Luck = v1;
          Agility = v2;
          Intellect = v3;
          Mentality = v4;
          CriticalRate = v5;
          HitRate = v6;
          PDef = v7;
          MDef = v8;
          CastingSpeed = v9;
          MagicCastingSpeed = v10;
          ManaRegen = v11;
          HPRegen = v12;
          DamageReduc = v13;
          MaxHP = v14;
      }



      #endregion*/

}
