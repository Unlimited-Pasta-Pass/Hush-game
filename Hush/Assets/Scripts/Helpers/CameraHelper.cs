using Cinemachine;
using Enums;
using UnityEngine;

namespace Helpers
{
    public static class CameraHelper
    {
        public static bool TryFindMainCamera(out GameObject mainCamera)
        {
            mainCamera = null;
            var cameras = GameObject.FindGameObjectsWithTag(Tags.MainCamera);
            
            foreach (var c in cameras)
            {
                if (c.TryGetComponent(typeof(Camera), out var component))
                {
                    mainCamera = c;
                    return true;
                }
            }
            
            return false;
        }
        public static bool TryFindVirtualCamera(out CinemachineFreeLook virtualCamera)
        {
            virtualCamera = null;
            var cameras = GameObject.FindGameObjectsWithTag(Tags.VirtualCamera);
            
            foreach (var c in cameras)
            {
                if (c.TryGetComponent(typeof(CinemachineFreeLook), out var component))
                {
                    virtualCamera = (CinemachineFreeLook)component;
                    return true;
                }
            }
            
            return false;
        }
    }
}