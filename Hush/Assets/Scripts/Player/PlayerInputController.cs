using Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace StarterAssets
{
	public class PlayerInputController : MonoBehaviour
	{
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool crouch;

		[Header("References")] 
		public PlayerInput playerInput;

		private void Start()
		{
			playerInput.SwitchCurrentControlScheme(ControlSchemes.KeyboardAndMouse);
			
			if (cursorLocked)
			{
				// Disable the mouse cursor
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
		
		#region Public Event Handlers
		
		public void OnMove(CallbackContext context)
		{
			move = context.ReadValue<Vector2>();
		}

		public void OnLook(CallbackContext context)
		{
			look = context.ReadValue<Vector2>();
		}

		public void OnJump(CallbackContext context)
		{
			jump = context.ReadValueAsButton();
		}

		public void OnSprint(CallbackContext context)
		{
			sprint = context.ReadValueAsButton();
		}

		public void OnCrouch(CallbackContext context)
		{
			crouch = context.ReadValueAsButton();
		}
		
		#endregion
	}
}