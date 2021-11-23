using System;
using System.Linq;
using UnityEngine;

namespace Environment.Passage
{
    public class ObjectToggle : MonoBehaviour
    {
        private Renderer[] _renderers;
        private Collider[] _colliders;

        void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _colliders = GetComponentsInChildren<Collider>();
        }

        public void Show()
        {
            foreach (var r in _renderers)
                r.enabled = true;
            foreach (var c in _colliders)
                c.enabled = true;
        }

        public void Hide()
        {
            foreach (var r in _renderers)
                r.enabled = false;
            foreach (var c in _colliders)
                c.enabled = false;
        }
    }
}