using UnityEngine;

namespace LOS
{
    /// <summary>
    /// Disables a gameobjects renderer if the object is outside the line of sight
    /// </summary>
    [RequireComponent(typeof(LOS.LOSCuller))]
    [AddComponentMenu("Line of Sight/LOS Object Hider")]
    public class LOSObjectHider : MonoBehaviour
    {
        private LOSCuller m_Culler;
        private LOSVisibilityInfo m_VisibilityInfo;

        private bool overrideCulling;
        private bool isRevealed;

        private void OnEnable()
        {
            m_Culler = GetComponent<LOSCuller>();

            enabled &= Util.Verify(m_Culler != null, "LOS culler component missing.");
            enabled &= Util.Verify(GetComponent<Renderer>() != null || GetComponentsInChildren<Renderer>().Length > 0, "No renderer attached to this GameObject! LOS Culler component must be added to a GameObject containing a MeshRenderer or Skinned Mesh Renderer!");
        }

        private void Start()
        {
            m_VisibilityInfo = GetComponent<LOSVisibilityInfo>();

            // Disable LOSCuller script and use Visibilty Info instead if both are present
            if (m_VisibilityInfo != null && m_VisibilityInfo.isActiveAndEnabled)
            {
                m_Culler.enabled = false;
            }
        }

        private void LateUpdate()
        {
            // TODO Do a fancy reveal/hide animation w/ shaders here
            if (overrideCulling)
            {
                UpdateVisibility(isRevealed);
            }
            else if (m_Culler.enabled)
            {
                UpdateVisibility(m_Culler.Visibile);
            }
            else if (m_VisibilityInfo != null && m_VisibilityInfo.isActiveAndEnabled)
            {
                UpdateVisibility(m_VisibilityInfo.Visibile);
            }
        }

        private void UpdateVisibility(bool visible)
        {
            var parentRenderer = GetComponent<Renderer>();
            if (parentRenderer != null)
                parentRenderer.enabled = visible;

            var childRenderers = GetComponentsInChildren<Renderer>();
            if (childRenderers != null && childRenderers.Length > 0)
            {
                foreach (var childRenderer in childRenderers)
                {
                    childRenderer.enabled = visible;
                }
            }
            
            var childCanvasses = GetComponentsInChildren<Canvas>();
            if (childCanvasses != null && childCanvasses.Length > 0)
            {
                foreach (var childCanvas in childCanvasses)
                {
                    childCanvas.enabled = visible;
                }
            }
        }

        public void RevealObject()
        {
            overrideCulling = true;
            isRevealed = true;
        }

        public void HideObject()
        {
            overrideCulling = true;
            isRevealed = false;
        }

        public void ResetObjectVisibility()
        {
            overrideCulling = false;
            isRevealed = false;
        }
    }
}
