using UnityEngine;

namespace Enums
{
    public static class PlayerAnimator
    {
        // These variables are the ones used in the blend tree in real-time
        public static int ForwardSpeed => Animator.StringToHash("ForwardSpeed");
        public static int LateralSpeed => Animator.StringToHash("LateralSpeed");
        
        // State booleans, no need to duplicate these since they aren't
        // floating point values and cannot be lerped
        public static int Sprinting => Animator.StringToHash("Sprinting");
        public static int Crouching => Animator.StringToHash("Crouching");
        public static int Jumping => Animator.StringToHash("Jumping");
        public static int Falling => Animator.StringToHash("Falling");
        public static int Grounded => Animator.StringToHash("Grounded");
        
        // These variables are synced over the network and not updated very frequently
        // We need to lerp these to get the real-time values
        public static int ForwardSpeedSync => Animator.StringToHash("ForwardSpeedSync");
        public static int LateralSpeedSync => Animator.StringToHash("LateralSpeedSync");
    }
}
