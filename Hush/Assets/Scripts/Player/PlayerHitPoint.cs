using Common;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace StarterAssets
{
    public class PlayerHitPoint : MonoBehaviour, IKillable
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

        public float HitPoints => GameManager.Instance.PlayerCurrentHitPoints;
        
        #endregion
        
        private static PlayerInputManager Input => PlayerInputManager.Instance;

        private void Awake()
        {
            GameManager.Instance.SetPlayerHitPoints(maxHitPoints);
            GameManager.Instance.SetPlayerMaxHitPoints(maxHitPoints);
        }
        
        public void TakeDamage(float damage)
        {
            // TODO: Add animation
            
            // If the player's hp is at 0 or lower, they die
            if (!GameManager.Instance.UpdatePlayerHitPoints(-damage))
            {
                Die();
            }
        }

        public void Die()
        {
            animator.SetBool(PlayerAnimator.Dead, true);
            
            Input.enabled = false;
            
            Killed.Invoke();
        }
        
    }
}
