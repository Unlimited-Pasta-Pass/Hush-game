using Common;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace StarterAssets
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerHitPointController : MonoBehaviour, IKillable
    {
        #region Parameters

        [Tooltip("Amount of damage the player can receive before dying")] 
        [SerializeField] private float maxHitPoints = 100f;

        [Header("Component References")] 
        [SerializeField] private Animator animator;

        #endregion

        #region Public Variables

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        private float _hitPoints;
        public float HitPoints
        {
            get => _hitPoints;
            private set
            {
                _hitPoints = value;
                if (_hitPoints <= 0)
                {
                    _hitPoints = 0;
                    Die();
                }
            }
        }

        #endregion

        private void Awake()
        {
            HitPoints = maxHitPoints;
        }
        
        public void TakeDamage(float damage)
        {
            // TODO: Add animation
            HitPoints -= damage;
        }

        public void Die()
        {
            animator.SetBool(PlayerAnimator.Dead, true);
            var playerInput = GetComponent<PlayerInputManager>();
            
            if (playerInput)
                playerInput.enabled = false;
            
            Killed.Invoke();
        }
        
    }
}
