using Common.Enums;
using Game;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.Enums;

namespace Weapon
{
    public class InvisibleSpell : MonoBehaviour, ISpell
    {
        [Header("Parameters")]
        [SerializeField] GameObject playerRoot;
        [SerializeField] private float alpha = 0.25f;
        
        [SerializeField] private float lightDuration = 3f;
        [SerializeField] private float heavyDuration = 6f;
        [SerializeField] private float speedBoost = 1f;
        
        [SerializeField] private float heavyCooldown = 10f;
        [SerializeField] private float lightCooldown = 6f;

        [SerializeField] private AudioSource invisibleAudio;
        
        private static InputManager Input => InputManager.Instance;
        private PlayerMovement _player;
        
        public SpellType SpellType => SpellType.InvisibleSpell;
        
        public float HeavyCooldown => heavyCooldown;
        public float LightCooldown => lightCooldown;

        private void OnEnable()
        {
            if (Input != null && Input.reference != null)
            {
                // only bind which one is useful move to public method
                Input.reference.actions[Actions.LightSpell].performed += PerformLightSpell;
                Input.reference.actions[Actions.HeavySpell].performed += PerformHeavySpell;
            }

            _player = FindObjectOfType<PlayerMovement>();

            if (_player == null)
                throw new MissingComponentException("Missing PlayerMovement Component in the Scene");
        }

        private void OnDisable()
        {
            if (Input != null && Input.reference != null)
            {
                Input.reference.actions[Actions.LightSpell].performed -= PerformLightSpell;
                Input.reference.actions[Actions.HeavySpell].performed -= PerformHeavySpell;
            }
        }
        
        public void PerformLightSpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveLightSpell() != SpellType || !GameManager.Instance.CanCastLight)
                return;

            SetInvisibilty(true, true);
            GameManager.Instance.SetLightSpellActivationTime(Time.time);
        }

        public void PerformHeavySpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveHeavySpell() != SpellType || !GameManager.Instance.CanCastHeavy)
                return;
            
            SetInvisibilty(true, true);
            _player.SetSpeedModifier(speedBoost, true);
            GameManager.Instance.SetHeavySpellActivationTime(Time.time);
        }

        private void EndLight()
        {
            SetInvisibilty(false, false);
        }

        private void EndHeavy()
        {
            SetInvisibilty(false, false);
            _player.SetSpeedModifier(speedBoost, false);
        }

        private void SetTransparent(bool enabled)
        {
            var renderers = playerRoot.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                Material material = r.material;
                Color newColor = material.color;
                newColor.a = enabled ? alpha : 1.0f;
                material.color = newColor;
            } 
        }

        private void SetOutline(bool isEnabled)
        {
            var outline = playerRoot.GetComponent<Outline>();
            outline.enabled = isEnabled;
        }

        private void SetInvisibilty(bool isEnabled, bool isHeavy)
        {
            SetOutline(isEnabled);
            SetTransparent(isEnabled);
            GameManager.Instance.SetIsPlayerInvisible(isEnabled);

            if (isEnabled)
            {
                invisibleAudio.Play();
                if (isHeavy)
                    Invoke(nameof(EndHeavy), heavyDuration);
                else
                    Invoke(nameof(EndLight), lightDuration);
            }
        }
    }
}
