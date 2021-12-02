using Game;

namespace UI
{
    public class PowerUpSelectionPermanentUI : PowerUpSelectionUI
    {
        public override void OnSubmit()
        {
            switch(CurrentlySelected)
            {
                case 1:
                    GameManager.Instance.BoostPermanentDamage(damageBoost);
                    break;
                case 2:
                    GameManager.Instance.BoostPermanentSpeed(speedBoost);
                    break;
                case 3:
                    GameManager.Instance.BoostPermanentVitality(vitalityBoost);
                    break;
            }
        }
    }
}
