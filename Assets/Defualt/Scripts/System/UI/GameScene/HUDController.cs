using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject[] SettingBoard;
    [SerializeField] private GameObject selectedDropdown;
    [SerializeField] private GameObject[] dragAreas;

    private float lastClickTime = 0f; 
    private const float doubleClickThreshold = 0.25f;

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
        if (background != null)
        {
            background.SetActive(!background.activeInHierarchy);
        }

        if (SettingBoard.Length > 0)
        {
            SettingBoardController(0);
        }

        if (dragAreas.Length > 0)
        {
            for (int i = 0; i < dragAreas.Length; i++)
            {
                dragAreas[i].SetActive(!dragAreas[i].activeInHierarchy);
            }
        }
    }

    public void DragAreaController(int index)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick <= doubleClickThreshold) // 더블 클릭으로 간주되는 경우
        {
            DragAreaActive(index);
        }
        else // 단일 클릭으로 간주되는 경우
        {
            selectedDropdown = dragAreas[index];
        }
    }

    public void DragAreaActive(int index)
    {
        if (index < 0 || index >= dragAreas.Length)
        {
            print("Index out of range: " + index);
            return;
        }

        var parent = dragAreas[index].transform.parent;

        if (parent.childCount == 0)
        {
            print("No children found for the given parent.");
            return;
        }

        var quickBar = parent.GetChild(0);
        quickBar.gameObject.SetActive(!quickBar.gameObject.activeInHierarchy);
    }

    public void SettingBoardController(int index)
    {
        if (SettingBoard.Length > 0)
        {
            for (int i = 0; i < SettingBoard.Length; i++)
            {
                SettingBoard[i].SetActive(i == index);
            }
        }
    }

}
