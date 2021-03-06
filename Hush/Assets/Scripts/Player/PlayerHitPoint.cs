using Common.Interfaces;
using Game;
using Player.Enums;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Common.Enums;

namespace Player
{
    public class PlayerHitPoint : MonoBehaviour, IKillable
    {
        #region Parameters

        [Header("Parameters")] 
        [SerializeField] private float deathSceneChangeDelay = 2f;

        [Header("Component References")] 
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private AudioSource damageSound;
        private CanvasGroup damageOverlay;

        // Used to prevent additional death sounds if the player is hit after death
        private bool deathSoundPlayed = false;

        #endregion

        #region Public Variables

        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints => GameManager.Instance.PlayerCurrentHitPoints;

        #endregion

        void Start()
        {
            damageOverlay = GameObject.FindWithTag(Tags.DamageOverlay).GetComponent<CanvasGroup>();
        }

        public void TakeDamage(float damage)
        {
            animator.SetTrigger(PlayerAnimator.TakeHit);
            damageSound.Play();
            FadeIn();

            // If the player's hp is at 0 or lower, they die
            if (!GameManager.Instance.UpdatePlayerHitPoints(-damage))
                Die();
        }

        public void FadeIn()
        {
            if (damageOverlay)
            {
                StartCoroutine(FadeCanvasGroup(damageOverlay, damageOverlay.alpha, 1, 0.5f));
                Invoke(nameof(FadeOut), 0.5f);
            }
        }

        public void FadeOut()
        {
            if (damageOverlay)
                StartCoroutine(FadeCanvasGroup(damageOverlay, damageOverlay.alpha, 0, 0.5f));
        }

        public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
        {
            float timeStartedLerping = Time.time;

            while (true)
            {
                var timeSinceStarted = Time.time - timeStartedLerping;
                var percentageComplete = timeSinceStarted / lerpTime;

                float currentValue = Mathf.Lerp(start, end, percentageComplete);

                cg.alpha = currentValue;

                if (percentageComplete >= 1)
                {
                    break;
                }

                yield return new WaitForFixedUpdate();
            }
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
            
            Invoke(nameof(GoToDeathScene), deathSceneChangeDelay);
        }

        private void GoToDeathScene()
        {
            SceneManager.Instance.LoadGameOverScene();
        }
    }
}