using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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

#if ENABLE_INPUT_SYSTEM
        /*public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}*/
        public void OnUp(InputValue value)
        {
			print(move);
            MoveYInput(value.Get<float>()); 
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

		
#endif

		

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
	}
	
}