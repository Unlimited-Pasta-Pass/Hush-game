using UnityEngine;

namespace  UI
{
    public class InteractableUI : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 cameraOffset;

        private void LateUpdate()
        {
            // rotate to ease legibility
            transform.rotation = Quaternion.Euler(cameraOffset);
        }
    }
}
