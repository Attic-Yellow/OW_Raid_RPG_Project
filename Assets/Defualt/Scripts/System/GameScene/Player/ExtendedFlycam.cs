using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ExtendedFlycam : MonoBehaviour
{
    private void OnCharacter(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.characterInfoUI.CharacterInfoUIController();
    }

    private void OnInventory(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.inventoryUI.InventoryController();
    }

    private void OnEquipped(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.characterGearUI.GearUIController();
    }

    private void OnSkill(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.skillUI.SkillUIController();
    }
}