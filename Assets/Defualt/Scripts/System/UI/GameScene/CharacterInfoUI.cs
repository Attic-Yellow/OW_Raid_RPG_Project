using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterInfoUI : MonoBehaviour
{
    #region 캐릭터 능력치 이름
    private string[] characterInfo = { "name", "level", "job" };
    private string[] abilityNames = { "maxHp", "str", "int", "dex", "spi", "vit", "luk", "crt", "dh", "det", "def", "mef", "pap", "map", "sks", "mhp", "sps", "ten", "pie" };
    #endregion

    #region 캐릭터 창 UI 오브젝트
    [SerializeField] private GameObject characterInfoUI;
    [SerializeField] private Transform jobIconTransform;
    [SerializeField] private List<GameObject> jobIconPrefabs;


    [Header("Character Info")]
    [SerializeField] private List<TextMeshProUGUI> characterInfoText;
    [SerializeField] private List<TextMeshProUGUI> abilitesText;
    [SerializeField] private List<CurrentEquippedSlot> currentEquippedSlots;
    #endregion

    [SerializeField] private CurrentEquipped currentEquipped;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.characterInfoUI = this;
    }

    #region 스타트 메서드
    private void Start()
    {
        currentEquipped = CurrentEquipped.Instance;
        currentEquipped.onChangeEquipp += ReadrawSlotUI;
        var charData = GameManager.Instance.dataManager.characterData.currentStatus;
        var charInfo = GameManager.Instance.dataManager.characterData.characterData;

        if (characterInfoUI != null)
        {
            characterInfoUI.SetActive(false);
        }

        if (characterInfoText.Count > 0)
        {
            for (int i = 0; i < characterInfo.Length; i++)
            {
                if (i == 2)
                {
                    string job = charInfo.ContainsKey(characterInfo[i]) ? charInfo[characterInfo[i]].ToString() : "null";
                    switch (job)
                    {
                        case "Warrior":
                            characterInfoText[i].text = "전사";
                            break;
                        case "Drgoon":
                            characterInfoText[i].text = "용기사";
                            break;
                        case "Bard":
                            characterInfoText[i].text = "음유시인";
                            break;
                        case "WhiteMage":
                            characterInfoText[i].text = "백마도사";
                            break;
                        case "BlackMage":
                            characterInfoText[i].text = "흑마도사";
                            break;
                    }
                }
                else
                {
                    characterInfoText[i].text = charInfo.ContainsKey(characterInfo[i]) ? charInfo[characterInfo[i]].ToString() : "null";
                }
            }
        }

        ReadrawInfoText(charData);

        if (jobIconTransform != null)
        {
            string jobName = charData.ContainsKey("job") ? charData[characterInfo[2]].ToString() : "Warrior";
            Job job = (Job)Enum.Parse(typeof(Job), jobName);
            int jobNumber = (int)job;
            Instantiate(jobIconPrefabs[jobNumber], jobIconTransform.transform);
        }

        ReadrawSlotUI();
    }
    #endregion

    #region 캐릭터 창 컨트롤러
    // 캐릭터 창 활성화/비활성화 컨트롤러
    public void CharacterInfoUIController()
    {
        if (characterInfoUI != null)
        {
            characterInfoUI.SetActive(!characterInfoUI.activeInHierarchy);

            if (characterInfoUI.activeSelf)
            {
                characterInfoUI.transform.SetAsLastSibling();
            }
        }
    }
    #endregion

    public void ReadrawInfoText(Dictionary<string, int> charData)
    {
        if (abilitesText.Count > 0)
        {
            for (int i = 0; i < abilityNames.Length; i++)
            {
                abilitesText[i].text = charData.ContainsKey(abilityNames[i]) ? charData[abilityNames[i]].ToString() : "null";
            }
        }
    }

    #region 현재 장비 UI 최신화
    // 현재 장비 UI 최신화 메서드
    public void ReadrawSlotUI()
    {
        for (int i = 1; i < currentEquippedSlots.Count; i++)
        {
            currentEquippedSlots[i].ClearSlot();

            if (i < currentEquipped.currentEquippeds.Count && currentEquipped.currentEquippeds[i].equipment != EquipmentType.None)
            {
                currentEquippedSlots[i].equipment = currentEquipped.currentEquippeds[i];
                currentEquippedSlots[i].UpdateSlotUI();
            }
            else
            {
                currentEquippedSlots[i].ClearSlot();
            }
        }
        ReadrawInfoText(GameManager.Instance.dataManager.characterData.currentStatus);
    }
    #endregion

    #region 현재 캐릭터 직업 반환
    // 캐릭터 직업 반환
    public string GetJob()
    {
        return GameManager.Instance.dataManager.characterData.characterData.ContainsKey(characterInfo[2]) ? 
            GameManager.Instance.dataManager.characterData.characterData[characterInfo[2]].ToString() : "null";
    }
    #endregion
}
