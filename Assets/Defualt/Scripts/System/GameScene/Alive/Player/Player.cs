using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System;
/*using Cinemachine;*/
using TMPro;

public class Player : Alive
{

      private float money;
      public List<GameObject> skillEffectList = new();
      [SerializeField] public float Money { get => money; set => money = value; }
      private Queue<Skill> canLearnSkills = new();
      private Monster monster;
      public enum State
          {
          Idle,
          Walk,
          MovementSKill, //�̵���
          Skill0,
          Skill1,
          Skill2,
          Skill3,
          Skill4,
          ReceivedDamage, //������ �Ծ�����
          Dead
      }

      private State currentState;


      #region UintyMethod
      private new void Awake()
      {
          base.Awake();
          monster = FindObjectOfType<Monster>();
        if(monster == null)
        {
            print("������ ����");
        }
      

      }
      private void Start()
      {
        if (photonView.IsMine)
        {
            currentState = State.Idle;
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

          if (photonView.IsMine) //�� ĳ���� �� ���
          {
             if(Input.GetKeyDown(KeyCode.Escape))
              {
                  monster.TakeDamage(gameObject,this.Power);
              }

              switch(currentState)
              {
                  case State.Idle:
                      case State.Walk:
                      case State.MovementSKill:
                      case State.Skill0:
                      case State.Skill1:
                      case State.Skill2:
                      case State.Skill3:
                      case State.Skill4:
                      break;
              }
            

          }

      }


      #endregion
      #region overridMethod
  

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


      private void StatusUp()
      {
          float increaseRate = 0.1f; //������
          float levelMultiplier = 1.0f + (level * increaseRate);

          Power *= levelMultiplier;
          Luck *= levelMultiplier;
          Agility *= levelMultiplier;
          Intellect *= levelMultiplier;
          Mentality *= levelMultiplier;
          CriticalRate *= levelMultiplier;
          HitRate *= levelMultiplier;
          PDef *= levelMultiplier;
          MDef *= levelMultiplier;
          CastingSpeed *= levelMultiplier;
          MagicCastingSpeed *= levelMultiplier;
          ManaRegen *= levelMultiplier;
          HPRegen *= levelMultiplier;
          DamageReduc *= levelMultiplier;
          MaxHP *= levelMultiplier;
      }
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
    /*  public void SetJob(User.Job job)
      {
          // ������ ������ ���� ������ ����
          switch (job)
          {
              case User.Job.��Ŀ:
                  SetStatus(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                  break;
              case User.Job.��������:
                  // �������� ������ �´� ���� ����
                  SetStatus(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
                  break;
              case User.Job.���Ÿ�����:
                  // ���Ÿ����� ������ �´� ���� ����
                  SetStatus(3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3);
                  break;
              case User.Job.����:
                  // ���� ������ �´� ���� ����
                  SetStatus(4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
                  break;
          }
      }
*/




      #endregion*/

}
