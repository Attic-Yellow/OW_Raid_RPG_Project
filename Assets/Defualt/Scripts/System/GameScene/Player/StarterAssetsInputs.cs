using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        public void OnUp(InputValue value)
        {
			MoveYInput(value.isPressed ? 1 : 0); 
        }

        public void OnDown(InputValue value)
        {
            MoveYInput(-value.Get<float>());
        }

        public void OnRight(InputValue value)
        {
            MoveXInput(value.Get<float>()); 
        }

        public void OnLeft(InputValue value)
        {
            MoveXInput(- value.Get<float>()); 
        }
        public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void MoveXInput(float newX)
		{
			move.x = newX;
		}

		public void MoveYInput(float newY)
		{
			move.y = newY;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        /*private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}*/

        #region ´ÜÃàÅ°
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
        #endregion

        #region Äü½½·Ô
        public void OnQuickSlot11(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 1");
        }

        public void OnQuickSlot12(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 2");
        }

        public void OnQuickSlot13(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 3");
        }

        public void OnQuickSlot14(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 4");
        }

        public void OnQuickSlot15(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 5");
        }

        public void OnQuickSlot16(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 6");
        }

        public void OnQuickSlot17(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 7");
        }

        public void OnQuickSlot18(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 8");
        }

        public void OnQuickSlot19(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 9");
        }

        public void OnQuickSlot110(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 10");
        }

        public void OnQuickSlot111(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 11");
        }

        public void OnQuickSlot112(InputValue value)
        {
            QuickSlotName("Quick Slot 1 - 12");
        }


        public void OnQuickSlot21(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 1");
        }

        public void OnQuickSlot22(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 2");
        }

        public void OnQuickSlot23(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 3");
        }

        public void OnQuickSlot24(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 4");
        }

        public void OnQuickSlot25(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 5");
        }

        public void OnQuickSlot26(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 6");
        }

        public void OnQuickSlot27(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 7");
        }

        public void OnQuickSlot28(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 8");
        }

        public void OnQuickSlot29(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 9");
        }

        public void OnQuickSlot210(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 10");
        }

        public void OnQuickSlot211(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 11");
        }

        public void OnQuickSlot212(InputValue value)
        {
            QuickSlotName("Quick Slot 2 - 12");
        }


        public void OnQuickSlot31(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 1");
        }

        public void OnQuickSlot32(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 2");
        }

        public void OnQuickSlot33(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 3");
        }

        public void OnQuickSlot34(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 4");
        }

        public void OnQuickSlot35(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 5");
        }

        public void OnQuickSlot36(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 6");
        }

        public void OnQuickSlot37(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 7");
        }

        public void OnQuickSlot38(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 8");
        }

        public void OnQuickSlot39(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 9");
        }

        public void OnQuickSlot310(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 10");
        }

        public void OnQuickSlot311(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 11");
        }

        public void OnQuickSlot312(InputValue value)
        {
            QuickSlotName("Quick Slot 3 - 12");
        }


        public void OnQuickSlot41(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 1");
        }

        public void OnQuickSlot42(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 2");
        }

        public void OnQuickSlot43(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 3");
        }

        public void OnQuickSlot44(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 4");
        }

        public void OnQuickSlot45(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 5");
        }

        public void OnQuickSlot46(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 6");
        }

        public void OnQuickSlot47(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 7");
        }

        public void OnQuickSlot48(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 8");
        }

        public void OnQuickSlot49(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 9");
        }

        public void OnQuickSlot410(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 10");
        }

        public void OnQuickSlot411(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 11");
        }

        public void OnQuickSlot412(InputValue value)
        {
            QuickSlotName("Quick Slot 4 - 12");
        }


        public void OnQuickSlot51(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 1");
        }

        public void OnQuickSlot52(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 2");
        }

        public void OnQuickSlot53(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 3");
        }

        public void OnQuickSlot54(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 4");
        }

        public void OnQuickSlot55(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 5");
        }

        public void OnQuickSlot56(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 6");
        }

        public void OnQuickSlot57(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 7");
        }

        public void OnQuickSlot58(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 8");
        }

        public void OnQuickSlot59(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 9");
        }

        public void OnQuickSlot510(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 10");
        }

        public void OnQuickSlot511(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 11");
        }

        public void OnQuickSlot512(InputValue value)
        {
            QuickSlotName("Quick Slot 5 - 12");
        }

        public void OnQuickSlot61(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 1");
        }

        public void OnQuickSlot62(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 2");
        }

        public void OnQuickSlot63(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 3");
        }

        public void OnQuickSlot64(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 4");
        }

        public void OnQuickSlot65(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 5");
        }

        public void OnQuickSlot66(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 6");
        }

        public void OnQuickSlot67(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 7");
        }

        public void OnQuickSlot68(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 8");
        }

        public void OnQuickSlot69(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 9");
        }

        public void OnQuickSlot610(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 10");
        }

        public void OnQuickSlot611(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 11");
        }

        public void OnQuickSlot612(InputValue value)
        {
            QuickSlotName("Quick Slot 6 - 12");
        }

        public void OnQuickSlot71(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 1");
        }

        public void OnQuickSlot72(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 2");
        }
        public void OnQuickSlot73(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 3");
        }

        public void OnQuickSlot74(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 4");
        }

        public void OnQuickSlot75(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 5");
        }

        public void OnQuickSlot76(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 6");
        }

        public void OnQuickSlot77(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 7");
        }

        public void OnQuickSlot78(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 8");
        }

        public void OnQuickSlot79(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 9");
        }

        public void OnQuickSlot710(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 10");
        }

        public void OnQuickSlot711(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 11");
        }

        public void OnQuickSlot712(InputValue value)
        {
            QuickSlotName("Quick Slot 7 - 12");
        }

        public void OnQuickSlot81(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 1");
        }

        public void OnQuickSlot82(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 2");
        }

        public void OnQuickSlot83(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 3");
        }

        public void OnQuickSlot84(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 4");
        }

        public void OnQuickSlot85(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 5");
        }

        public void OnQuickSlot86(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 6");
        }

        public void OnQuickSlot87(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 7");
        }

        public void OnQuickSlot88(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 8");
        }

        public void OnQuickSlot89(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 9");
        }

        public void OnQuickSlot810(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 10");
        }

        public void OnQuickSlot811(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 11");
        }

        public void OnQuickSlot812(InputValue value)
        {
            QuickSlotName("Quick Slot 8 - 12");
        }

        public void OnQuickSlot91(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 1");
        }

        public void OnQuickSlot92(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 2");
        }

        public void OnQuickSlot93(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 3");
        }

        public void OnQuickSlot94(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 4");
        }

        public void OnQuickSlot95(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 5");
        }

        public void OnQuickSlot96(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 6");
        }

        public void OnQuickSlot97(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 7");
        }

        public void OnQuickSlot98(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 8");
        }

        public void OnQuickSlot99(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 9");
        }

        public void OnQuickSlot910(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 10");
        }

        public void OnQuickSlot911(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 11");
        }

        public void OnQuickSlot912(InputValue value)
        {
            QuickSlotName("Quick Slot 9 - 12");
        }

        public void OnQuickSlot101(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 1");
        }

        public void OnQuickSlot102(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 2");
        }

        public void OnQuickSlot103(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 3");
        }

        public void OnQuickSlot104(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 4");
        }

        public void OnQuickSlot105(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 5");
        }

        public void OnQuickSlot106(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 6");
        }

        public void OnQuickSlot107(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 7");
        }

        public void OnQuickSlot108(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 8");
        }


        public void OnQuickSlot109(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 9");
        }

        public void OnQuickSlot1010(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 10");
        }

        public void OnQuickSlot1011(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 11");
        }

        public void OnQuickSlot1012(InputValue value)
        {
            QuickSlotName("Quick Slot 10 - 12");
        }

        void QuickSlotName(string str)
        {
            if (GameManager.Instance.uiManager.gameSceneUI.quickSlotData.slotDataList.ContainsKey(str))
            {
                print("¿©±âµµ µé¾î¿Í?");
                SlotDataList slotData = GameManager.Instance.uiManager.gameSceneUI.quickSlotData.slotDataList[str];
                print($"½½·Ô ÀÌ¸§{slotData.slotName}");
                GameObject.Find(str).GetComponent<QuickSlot>().slot.GetComponent<Slot>().Use();
            }
        }
        #endregion
    }

}