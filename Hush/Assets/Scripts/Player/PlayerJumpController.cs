using System;
using Enums;
using MLAPI;
using Player.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerJumpController : MonoBehaviour
{
    [Header("Speed")] 
    [Range(0.1f, 1f)]
    public float jumpForce;

    [Header("Raycast")]
    public LayerMask groundLayers;
    
    [Header("References")] 
    public NetworkObject networkObject;
    public Rigidbody body;
    public CapsuleCollider capsule;
    public Animator animator;
    public InputActionReference action;

    private PlayerState _state;
    private bool IsPlayerGrounded => Physics.CheckCapsule(capsule.bounds.min, capsule.bounds.max, capsule.radius, groundLayers);
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (networkObject.IsLocalPlayer)
        {
            _state.Jumping = context.action.triggered && context.ReadValueAsButton();
        }
    }

    private void Start()
    {
        _state = PlayerState.Instance;
    }

    private void Update()
    {
        animator.SetBool(PlayerAnimator.Jumping, !IsPlayerGrounded);
        
        if (IsPlayerGrounded && _state.Jumping)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
