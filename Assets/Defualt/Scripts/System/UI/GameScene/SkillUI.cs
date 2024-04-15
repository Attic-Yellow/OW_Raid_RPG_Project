using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject skillUI;
    [SerializeField] private List<GameObject> contentAreas;
    [SerializeField] private List<Skill> skills;
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform skillSlotParent;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.skillUI = this;
    }

    private void Start()
    {
        if (skillUI != null)
        {
            skillUI.SetActive(false);
        }

        ContentAreaController(0);
        CreateSkills();
    }

    // 스킬 UI 활성화/비활성화 컨트롤러
    public void SkillUIController()
    {
        if (skillUI != null)
        {
            skillUI.SetActive(!skillUI.activeInHierarchy);
        }
    }

    // 스킬 UI 목록 컨트롤러
    public void ContentAreaController(int index)
    {
        if (contentAreas.Count > 0)
        {
            for (int i = 0; i < contentAreas.Count; i++)
            {
                contentAreas[i].SetActive(i == index);
            }
        }
    }

    public void CreateSkills()
    {
        var job = GameManager.Instance.uiManager.gameSceneUI.characterInfoUI.GetJob();

        switch (job)
        {
            case "Warrior" :
                foreach (var skill in SkillData.Instance.warriorSkills)
                {
                    var instance = Instantiate(skillSlotPrefab, skillSlotParent);

                    var skillSlot = instance.GetComponent<SkillSlot>();
                    
                    if (skillSlot != null)
                    {
                        skillSlot.SkillInfo(skill);
                    }
                }
                break;
            case "Drgoon":
                foreach (var skill in SkillData.Instance.dragoonSkills)
                {
                    var instance = Instantiate(skillSlotPrefab, skillSlotParent);

                    var skillSlot = instance.GetComponent<SkillSlot>();

                    if (skillSlot != null)
                    {
                        skillSlot.SkillInfo(skill);
                    }
                }
                break;
            case "Bard":
                foreach (var skill in SkillData.Instance.bardSkills)
                {
                    var instance = Instantiate(skillSlotPrefab, skillSlotParent);

                    var skillSlot = instance.GetComponent<SkillSlot>();

                    if (skillSlot != null)
                    {
                        skillSlot.SkillInfo(skill);
                    }
                }
                break;
            case "WhiteMage":
                foreach (var skill in SkillData.Instance.whiteMageSkills)
                {
                    var instance = Instantiate(skillSlotPrefab, skillSlotParent);

                    var skillSlot = instance.GetComponent<SkillSlot>();

                    if (skillSlot != null)
                    {
                        skillSlot.SkillInfo(skill);
                    }
                }
                break;
            case "BlackMage":
                foreach (var skill in SkillData.Instance.blackMageSkills)
                {
                    var instance = Instantiate(skillSlotPrefab, skillSlotParent);

                    var skillSlot = instance.GetComponent<SkillSlot>();

                    if (skillSlot != null)
                    {
                        skillSlot.SkillInfo(skill);
                    }
                }
                break;
        }
    }
}
