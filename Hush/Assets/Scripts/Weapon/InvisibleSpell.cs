using Game;
using UnityEngine;

namespace Weapon
{
    public class InvisibleSpell : PlayerSpell
    {
        [SerializeField] GameObject playerRoot;
        [SerializeField] private float alpha = 0.25f;
        [SerializeField] private float invisibleDuration = 5f;
        
        private bool isInvisible = false;
        
        private float lastInvisibleTime => GameManager.Instance.GetSpellActivationTime();

        protected new void Update()
        {
            base.Update();

            if (Time.time - lastInvisibleTime >= invisibleDuration && isInvisible)
            {
                SetInvisibilty(false);
            }
        }

        protected override void CreateSpellAttack(float damage)
        {
            SetInvisibilty(true);
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

        private void SetInvisibilty(bool enabled)
        {
            isInvisible = enabled;
            SetOutline(enabled);
            SetTransparent(enabled);
            
            if (enabled)
            {
                GameManager.Instance.SetSpellActivationTime(Time.time);
            }
        }
    }
}
