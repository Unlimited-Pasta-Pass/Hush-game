namespace Game
{
    public partial class GameManager
    {
        public float PermanentDamage
        {
            get => _state.baseDamagePermanent;
            private set => _state.baseDamagePermanent = value;
        }

        public void BoostPermanentDamage(float boost)
        {
            PermanentDamage += boost;
        }

        public float PermanentSpeed
        {
            get => _state.baseSpeedPermanent;
            private set => _state.baseSpeedPermanent = value;
        }

        public void BoostPermanentSpeed(float boost)
        {
            PermanentSpeed += boost;
        }
        
        public float PermanentVitality
        {
            get => _state.baseVitalityPermanent;
            private set => _state.baseVitalityPermanent = value;
        }

        public void BoostPermanentVitality(float boost)
        {
            PermanentVitality += boost;
        }
    }
}
