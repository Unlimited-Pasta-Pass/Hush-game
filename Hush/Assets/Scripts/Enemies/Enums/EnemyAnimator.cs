using UnityEngine;

namespace Common
{
    public static class EnemyAnimator
    {
        public static int Speed => Animator.StringToHash("Speed");
        public static int BaseAttack => Animator.StringToHash("Base_Attack");

        public static class State
        {
            public static string Move => "Move";
            public static string Attack => "Attack";
        }

        public static class Layer
        {
            public static int Base => 0;
            public static int UpperBody => 1;
        }
    }
}
