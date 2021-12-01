using System.Xml.Schema;

namespace Game
{
    public partial class GameManager
    {
        public float PermanentDamage
        {
            get => _state.baseDamagePermanent;
            private set => _state.baseDamagePermanent = value;
        }

        public void BoostPermanentDamage()
        {
            PermanentDamage += 5;
        }

        public float PermanentSpeed
        {
            get => _state.BaseSpeedPermanent;
            private set => _state.BaseSpeedPermanent = value;
        }

        public void BoostPermanentSpeed()
        {
            PermanentSpeed += 5;
        }
        
        public float PermanentVitality
        {
            get => _state.BaseVitalityPermanent;
            private set => _state.BaseVitalityPermanent = value;
        }

        public void BoostPermanentVitality()
        {
            PermanentVitality += 10;
        }
    }
}