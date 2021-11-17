using UnityEngine;

namespace Environment.Passage
{
    public class SecretPassageDoor : ObjectToggle
    {

        [SerializeField] private SecretPassage passage;

        void Reset()
        {
            passage = GetComponentInParent<SecretPassage>();
        }

        public void RevealPassage()
        {
            passage.Reveal();
        }

    }
}
