using System;
using System.Linq;
using UnityEngine;

namespace Environment.Passage
{
    public class ObjectToggle : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}
