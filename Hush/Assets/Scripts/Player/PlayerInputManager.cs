using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputManager : MonoBehaviour
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
	public bool reveal;
	public bool switchWeapon;

	[Header("References")] 
	public PlayerInput reference;

	[Header("Door Interactions")]
	public bool isInCombat;

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
		isInCombat = context.ReadValueAsButton();
	}

	public void OnHeavyAttack(CallbackContext context)
	{
		heavyAttack = context.ReadValueAsButton();
		isInCombat = context.ReadValueAsButton();
	}

	public void OnSpecialAttack(CallbackContext context)
	{
		specialAttack = context.ReadValueAsButton();
		isInCombat = context.ReadValueAsButton();
	}

	public void OnInteract(CallbackContext context)
	{
		interact = context.ReadValueAsButton();
		isInCombat = !context.ReadValueAsButton();
	}

	public void OnWalk(CallbackContext context)
	{
		walk = context.ReadValueAsButton();
		isInCombat = !context.ReadValueAsButton();
	}

	public void OnReveal(CallbackContext context)
	{
		reveal = context.ReadValueAsButton();
	}
	
	public void OnSwitchWeapon(CallbackContext context)
	{
		switchWeapon = context.ReadValueAsButton();
	}
	
	#endregion
}
