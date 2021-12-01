using Common.Interfaces;
using Game;
using Player.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerHitPoint : MonoBehaviour, IKillable
    {
        #region Parameters

        [Header("Component References")] 
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private AudioSource damageSound;

        private bool deathSoundPlayed = false; // Used to prevent additional death sounds if the player is hit after death

        #endregion

        #region Public Variables

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints => GameManager.Instance.PlayerCurrentHitPoints;
        
        #endregion
        
        public void TakeDamage(float damage)
        {
            // TODO: Add animation
            damageSound.Play();
            
            // If the player's hp is at 0 or lower, they die
            if (!GameManager.Instance.UpdatePlayerHitPoints(-damage))
                Die();
        }

        public void Die()
        {
            if (!deathSoundPlayed)
            {
                deathSoundPlayed = true;
                deathSound.Play();
            }
            
            animator.SetBool(PlayerAnimator.Dead, true);
            Killed.Invoke();
        }
        
    }
}
