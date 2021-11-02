using UnityEngine;

namespace Enums
{
    public static class PlayerAnimator
    {
        public static int Speed => Animator.StringToHash("Speed");
		public static int LightAttack => Animator.StringToHash("LightAttack");
		public static int HeavyAttack => Animator.StringToHash("HeavyAttack");
		public static int SpellAttack => Animator.StringToHash("SpellAttack");
		public static int SpellSpecialAttack => Animator.StringToHash("SpellSpecialAttack");
    }
}
