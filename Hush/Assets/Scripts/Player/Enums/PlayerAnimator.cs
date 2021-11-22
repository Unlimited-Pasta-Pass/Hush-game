using UnityEngine;

namespace Player.Enums
{
    public static class PlayerAnimator
    {
        public static int Speed => Animator.StringToHash("Speed");
        public static int LightAttack => Animator.StringToHash("LightAttack");
        public static int HeavyAttack => Animator.StringToHash("HeavyAttack");
        public static int SpellAttack => Animator.StringToHash("SpellAttack");
        public static int SpellSpecialAttack => Animator.StringToHash("SpellSpecialAttack");
        public static int Dead => Animator.StringToHash("Dead");
        public static class Layer
        {
            public static int Base => 0;
            public static int UpperBody => 1;
        }
        public static class State
        {
            public static string LightAttack => "Light Attack";
            public static string HeavyAttack => "Heavy Attack";
        }
    }
}
