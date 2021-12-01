using System.Xml.Schema;

namespace Game
{
    public partial class GameManager
    {
        public float PermanentDamage
        {
            get => _state.baseDamagePermanent;
            set => _state.baseDamagePermanent = value;
        }
        
        public float PermanentSpeed
        {
            get => _state.BaseSpeedPermanent;
            set => _state.BaseSpeedPermanent = value;
        }
        
        public float PermanentVitality
        {
            get => _state.BaseVitalityPermanent;
            set => _state.BaseVitalityPermanent = value;
        }
        
    }
}