using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        public bool CanReveal => Time.time - EcholocationActivationTime > EcholocationCooldownTime;
        
        public float EcholocationActivationTime
        {
            get
            {
                return _state.echolocationActivationTime;
            }
            set
            {
                _state.echolocationActivationTime = value;
            }
        }
        
        public float EcholocationCooldownTime
        {
            get
            {
                return _state.echolocationCooldownTime;
            }
            set
            {
                _state.echolocationCooldownTime = value;
            }
        }
    }
}