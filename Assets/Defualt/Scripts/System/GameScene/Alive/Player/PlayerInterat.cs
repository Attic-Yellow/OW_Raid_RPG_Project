using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UIElements;
public class PlayerInterat : MonoBehaviourPun,IPunObservable
{
    public Npc nearNPC;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (photonView.IsMine == false) { return; }
        
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Aggressive aggressive = FindObjectOfType<Aggressive>();
                aggressive.TakeDamage(gameObject, player.Power);
            }
          /*  Interation();*/
            InputSkillKey();
            
        
    }

    private void InputSkillKey()
    {
        for(int i = 0; i < player.haveSkills.Count; i++)
        {
           /* if (Input.GetKeyDown(player.haveSkills[i].GetKeyCode()))
            {
                UseSkill(i);
            }*/
        }

    }

   /* private void UseSkill(int skillIndex)
    {
            int skillSlotIndex = skillIndex; // ��ų ������ 0���� �����ϹǷ� �Էµ� ���ڿ��� 1�� ���ϴ�.

            Skill skill = player.haveSkills[skillIndex];

            if (skillSlotIndex <= player.haveSkills.Count-1 ) // �������� ��ų
            {
                if (UIManager.Instance.skillIcons[skillSlotIndex].value == 1)
                {
                    UIManager.Instance.skillIcons[skillSlotIndex].value = 0;
                    UIManager.Instance.ChangeSkillValue(skillSlotIndex, skill.GetCoolTime());
                    skill.Use();
                    photonView.RPC("EffectPlay",RpcTarget.All, skillSlotIndex,transform.position);
                  
                }
                else
                {
                    Debug.Log("��ų ��Ÿ���Դϴ�");
                }
            }
    }
    [PunRPC]
    private void EffectPlay(int skillIndex, Vector3 position)
    {
        GameObject effectPrefab = player.haveSkills[skillIndex].GetSkillEffect();
        print(effectPrefab.name);
        GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity,transform);

        // ��ƼŬ �ý����� ����� �Ϸ�Ǹ� �ش� ������Ʈ�� �����ϱ� ���� �ڷ�ƾ ����
        StartCoroutine(DestroyEffectAfterPlay(effectInstance));

    }

    private IEnumerator DestroyEffectAfterPlay(GameObject effectObject)
    {
        SkillEffect skillEffect = effectObject.GetComponent<SkillEffect>();
        while (skillEffect != null && skillEffect.ps.isPlaying)
        {
            yield return null;
        }

        // ��ƼŬ �ý����� �� �̻� ������� ������ ������Ʈ ����
        PhotonNetwork.Destroy(effectObject);
    }
    private void Interation()
    {
        if (UIManager.Instance.GetactiveSelf(UIManager.Instance.interactUI))
        {
        
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    UIManager.Instance.NPCTalkUIController();
                    UIManager.Instance.InteratUIController();
                    TextMeshProUGUI t = UIManager.Instance.npcTalkUI.GetComponentInChildren<TextMeshProUGUI>();
                    switch (nearNPC.quest.GetCurrentPhase())
                    {
                        case QuestManager.QuestPhase.None:
                        case QuestManager.QuestPhase.Finish:
                            t.text = nearNPC.greetingMessage;
                            break;
                        case QuestManager.QuestPhase.CanStart:
                            t.text = nearNPC.quest.qData.explainMsg;
                        UIManager.Instance.AgreeBtnController(true);
                            break;
                        case QuestManager.QuestPhase.Activing:
                            t.text = nearNPC.quest.qData.activingMsg;
                            break;
                        case QuestManager.QuestPhase.CanFinish:
                            t.text = nearNPC.quest.qData.compelteMsg;
                            break;
                    }
                }
        }
        else if(UIManager.Instance.GetactiveSelf(UIManager.Instance.npcTalkUI))
        {
            
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (nearNPC.quest.GetCurrentPhase() != QuestManager.QuestPhase.CanStart)
                    {
                        UIManager.Instance.NPCTalkUIController();
                        nearNPC.ChangeMark(nearNPC.quest.GetCurrentPhase());
                    }
                }
            
        }
    }
      
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("NPC"))
        {
            nearNPC = other.gameObject.GetComponent<Npc>();
            print("��Ҵ�");
            UIManager.Instance.InteratUIController();
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("NPC") && UIManager.Instance.GetactiveSelf(UIManager.Instance.interactUI))
        {
            nearNPC = null;
            UIManager.Instance.InteratUIController();
        }

    }*/

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      
    }
}
