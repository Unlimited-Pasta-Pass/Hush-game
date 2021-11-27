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
        
        private static InputManager Input => InputManager.Instance;
        private static SpellManager SpellManager => SpellManager.Instance;

        private PlayerMovement _player;
        
        //TODO Remove
        private void Awake()
        {
            GameManager.Instance.SetActiveHeavySpell(WeaponType.InvisibleSpell);
            GameManager.Instance.SetActiveLightSpell(WeaponType.InvisibleSpell);
            GameManager.Instance.SetHeavySpellCooldownTime(4f);
            GameManager.Instance.SetLightSpellCooldownTime(4f);
            GameManager.Instance.SetHeavySpellActivationTime(0);
            GameManager.Instance.SetLightSpellActivationTime(0);
        }

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
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveLightSpell() != WeaponType.InvisibleSpell || !SpellManager.canCastLight)
                return;

            SetInvisibilty(true, true);
            GameManager.Instance.SetLightSpellActivationTime(Time.time);
        }

        public void PerformHeavySpell(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.PlayerIsAlive || GameManager.Instance.GetActiveHeavySpell() != WeaponType.InvisibleSpell || !SpellManager.canCastHeavy)
                return;
            
            SetInvisibilty(true, true);
            GameManager.Instance.SetHeavySpellActivationTime(Time.time);
        }

        private void EndLight()
        {
            SetInvisibilty(false, false);
        }

        private void EndHeavy()
        {
            SetInvisibilty(false, false);
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

        private void SetOutline(bool enabled)
        {
            var outline = playerRoot.GetComponent<Outline>();
            outline.enabled = enabled;
        }

        private void SetInvisibilty(bool enabled, bool isHeavy)
        {
            SetOutline(enabled);
            SetTransparent(enabled);

            if (enabled)
            {
                if(isHeavy)
                    Invoke(nameof(EndHeavy), heavyDuration);
                else
                    Invoke(nameof(EndLight), lightDuration);
            }
        }
    }
}
