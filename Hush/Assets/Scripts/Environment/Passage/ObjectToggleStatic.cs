using Common.Interfaces;
using UnityEngine;

namespace Environment.Passage
{
    public abstract class ObjectToggleStatic : ObjectToggle
    {
        public override void Show(bool force = false)
        {
            if (gameObject.activeSelf && !force)
                return;
            
            gameObject.SetActive(true);
        }

        public override void Hide(bool force = false)
        {
            if (!gameObject.activeSelf && !force)
                return;
            
            gameObject.SetActive(false);
        }
    }
}
