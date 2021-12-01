using System.Xml.Schema;

namespace Game
{
    public partial class GameManager
    {
        public float PermanentDamage
        {
            get => _state.damagePermanent;
            set => _state.damagePermanent = value;
        }
        
        public float PermanentSpeed
        {
            get => _state.speedPermanent;
            set => _state.speedPermanent = value;
        }
        
        public float PermanentVitality
        {
            get => _state.vitalityPermanent;
            set => _state.vitalityPermanent = value;
        }
        
    }
}