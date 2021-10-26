using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace StarterAssets
{
	public class PlayerInputController : MonoBehaviour
	{ 
		[Header("Mouse Cursor Settings")] 
		[SerializeField] private bool cursorLocked = true;
		
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool lightAttack;
		public bool heavyAttack;
		public bool specialAttack;
		public bool interact;
		public bool walk;

		[Header("References")] 
		public PlayerInput playerInput;

		private void Start()
		{
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

		public void OnLightAttack(CallbackContext context)
		{
			lightAttack = context.ReadValueAsButton();
		}

		public void OnHeavyAttack(CallbackContext context)
		{
			heavyAttack = context.ReadValueAsButton();
		}

		public void OnSpecialAttack(CallbackContext context)
		{
			specialAttack = context.ReadValueAsButton();
		}

		public void OnInteract(CallbackContext context)
		{
			interact = context.ReadValueAsButton();
		}

		public void OnWalk(CallbackContext context)
		{
			walk = context.ReadValueAsButton();
		}
		
		#endregion
	}
}
