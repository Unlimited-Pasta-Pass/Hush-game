using UnityEngine;

namespace Enums
{
    public static class PlayerAnimator
    {
        public static int Speed => Animator.StringToHash("Speed");
        public static int Sprinting => Animator.StringToHash("Sprinting");
        public static int Crouching => Animator.StringToHash("Crouching");
    }
}
