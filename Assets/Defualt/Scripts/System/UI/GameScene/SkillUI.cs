using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject skillUI;
    [SerializeField] private List<GameObject> contentAreas;
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
    }

    public void SkillUIController()
    {
        if (skillUI != null)
        {
            skillUI.SetActive(!skillUI.activeInHierarchy);
        }
    }

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
}
