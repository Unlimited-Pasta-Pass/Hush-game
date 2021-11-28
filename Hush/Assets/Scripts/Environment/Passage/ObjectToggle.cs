using Common.Interfaces;
using UnityEngine;

namespace Environment.Passage
{
    public abstract class ObjectToggle : MonoBehaviour, ITogglelable
    {
        public abstract bool RevealOnEcholocate { get; }
        public abstract void Show(bool force = false);
        public abstract void Hide(bool force = false);
    }
}
