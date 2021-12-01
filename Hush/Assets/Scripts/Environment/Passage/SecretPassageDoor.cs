using Common.Interfaces;
using UnityEngine;

namespace Environment.Passage
{
    public class SecretPassageDoor : ObjectToggleStatic
    {
        [SerializeField] private SecretPassage passage;

        public override bool RevealOnEcholocate => true;

        void Reset()
        {
            passage = GetComponentInParent<SecretPassage>();
        }
        
        public override void Show(bool force = false)
        {
            base.Hide(force);
            passage.Show();
        }

        public override void Hide(bool force = false)
        {
            if (!passage.Shown) 
            {
                base.Show(force);
            }
        }
    }
}
