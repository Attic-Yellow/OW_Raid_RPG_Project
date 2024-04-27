using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject mainSetBoard;
    [SerializeField] private GameObject[] SettingBoard;
    [SerializeField] private TMP_Dropdown selectedDropdown;
    [SerializeField] private GameObject[] dragAreas;

    public HUDData HUDData = new HUDData();

    private float lastClickTime = 0f; 
    private const float doubleClickThreshold = 0.25f;
    private bool isHUDActive = false;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.hudController = this;
    }

    private void Start()
    {
        if (background != null)
        {
            background.SetActive(false);
        }

        if (SettingBoard.Length > 0)
        {
            SettingBoardController(-1);
        }

        if (mainSetBoard != null)
        {
            mainSetBoard.SetActive(false);
        }

        if (selectedDropdown != null)
        {
            List<string> areaName = new List<string>();
            for (int i = 0; i < dragAreas.Length; i++)
            {
                areaName.Add(dragAreas[i].name);
            }
            PopulateDropdown(areaName);
        }

        if (dragAreas.Length > 0)
        {
            for (int i = 0; i < dragAreas.Length; i++)
            {
                dragAreas[i].SetActive(false);
            }
        }
    }

    public void DragAreaContoller()
    {
        isHUDActive = !isHUDActive;

        if (background != null)
        {
            background.SetActive(!background.activeInHierarchy);
        }

        if (SettingBoard.Length > 0)
        {
            SettingBoardController(-1);
        }

        if (dragAreas.Length > 0)
        {
            for (int i = 0; i < dragAreas.Length; i++)
            {
                dragAreas[i].SetActive(!dragAreas[i].activeInHierarchy);
            }
        }
    }

    public void DragAreaActive(int index)
    {
        if (index < 0 || index >= dragAreas.Length)
        {
            return;
        }

        var parent = dragAreas[index].transform.parent;

        if (parent.childCount == 0)
        {
            return;
        }

        var quickBar = parent.GetChild(0);
        quickBar.gameObject.SetActive(!quickBar.gameObject.activeInHierarchy);
    }

    void PopulateDropdown(List<string> options)
    {
        selectedDropdown.ClearOptions(); // 기존 옵션 삭제
        selectedDropdown.AddOptions(options); // 새로운 옵션 추가
    }

    public void MainSetBoardController()
    {
        if (mainSetBoard != null)
        {
            mainSetBoard.SetActive(!mainSetBoard.activeInHierarchy);
        }
    }

    public void SettingBoardController(int index)
    {
        index = index >= 0 ? selectedDropdown.value : index;

        MainSetBoardController();

        if (SettingBoard.Length > 0)
        {
            for (int i = 0; i < SettingBoard.Length; i++)
            {
                SettingBoard[i].SetActive(i == index);
            }
        }
    }

    public void PointerClick(GameObject clickedObject)
    {
        int index = Array.IndexOf(dragAreas, clickedObject);

        if (index == -1)
        {
            return; // 드래그 영역이 아닌 경우 무시
        }

        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick <= doubleClickThreshold) // 더블 클릭 간주
        {
            DragAreaActive(index);
        }
        else // 단일 클릭 간주
        {
            if (index >= 0 && index < dragAreas.Length)
            {
                selectedDropdown.value = index;
            }
        }
    }

    public bool GetIsHUDActive()
    {
        return isHUDActive;
    }
}
