using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneUI : MonoBehaviour
{
    public CharacterInfoUI characterInfoUI;
    public CharacterGearUI characterGearUI;
    public InventoryUI inventoryUI;
    public MenuButtonUI menuButtonsUI;
    public KeyBindingUI keyBindingUI;
    public SkillUI skillUI;
    public HUDController hudController;
    public Canvas canvas;
    public QuickSlotData quickSlotData = new QuickSlotData();
    public GameObject DieUI;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI = this;
    }

    public void SetDieUI(bool isTrue)
    {
        DieUI.SetActive(isTrue);
    }
}
