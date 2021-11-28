using LOS;
using UnityEngine;

namespace Environment.Passage
{
    [RequireComponent(typeof(LOSObjectHider))]
    public abstract class ObjectToggleLOS : ObjectToggle
    {
        [SerializeField] private LOSObjectHider hider;
        
        public virtual void Reset()
        {
            hider = GetComponent<LOSObjectHider>();
        }
        
        public override void Hide(bool force = false)
        {
            hider.HideObject();
        }

        public override void Show(bool force = false)
        {
            hider.ResetObjectVisibility();
        }
    }
}
