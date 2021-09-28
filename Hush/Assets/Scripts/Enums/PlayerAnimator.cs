namespace Enums
{
    public static class PlayerAnimator
    {
        // These variables are the ones used in the blend tree in real-time
        public static string ForwardSpeed => "ForwardSpeed";
        public static string LateralSpeed => "LateralSpeed";
        
        // These variables are synced over the network and not updated very frequently
        // We need to lerp these to get the real-time values
        public static string ForwardSpeedSync => "ForwardSpeedSync";
        public static string LateralSpeedSync => "LateralSpeedSync";
    }
}
