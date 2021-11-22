using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
	public class InputManager : MonoBehaviour
	{
		[Header("Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool lightAttack;
		public bool heavyAttack;
		public bool lightSpell;
		public bool heavySpell;
		public bool interact;
		public bool walk;
		public bool reveal;
		public bool pause;

		[Header("References")] 
		public PlayerInput reference;

		public static InputManager Instance;

		private void Awake()
		{
			if (Instance == null)
				Instance = this;
			
			DontDestroyOnLoad(Instance.gameObject);
		}

		private void Start()
		{
			// Lock the mouse cursor
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
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

		public void OnLightSpellAttack(CallbackContext context)
		{
			lightSpell = context.ReadValueAsButton();
		}

		public void OnHeavySpellAttack(CallbackContext context)
		{
			heavySpell = context.ReadValueAsButton();
		}

		public void OnInteract(CallbackContext context)
		{
			interact = context.ReadValueAsButton();
		}

		public void OnWalk(CallbackContext context)
		{
			walk = context.ReadValueAsButton();
		}

		public void OnReveal(CallbackContext context)
		{
			reveal = context.ReadValueAsButton();
		}
	
		public void OnPause(CallbackContext context)
		{
			pause = context.ReadValueAsButton();
		}
	
		#endregion
	}
}
