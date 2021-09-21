using Cinemachine;
using Enums;
using UnityEngine;

namespace Helpers
{
    public static class CameraHelper
    {
        public static CinemachineFreeLook FindFreeLookCamera()
        {
            var cameras = GameObject.FindGameObjectsWithTag(Tags.FreeLookCamera);
            
            foreach (var cam in cameras)
            {
                if (cam.TryGetComponent(typeof(CinemachineFreeLook), out Component component))
                {
                    return (CinemachineFreeLook)component;
                }
            }
            
            return null;
        }
    }
}