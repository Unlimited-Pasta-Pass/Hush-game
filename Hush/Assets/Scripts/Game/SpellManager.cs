using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Weapon.Enums;

namespace Game
{
    public class SpellManager : MonoBehaviour
    {
        public static SpellManager Instance;
        
        public bool CanCastHeavy => Time.time - GameManager.Instance.GetHeavySpellActivationTime() > GameManager.Instance.GetHeavySpellCoolDownTime();
        public bool CanCastLight => Time.time - GameManager.Instance.GetLightSpellActivationTime() > GameManager.Instance.GetLightSpellCoolDownTime();
    
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        
            DontDestroyOnLoad(Instance.gameObject);
        }
    }
}
