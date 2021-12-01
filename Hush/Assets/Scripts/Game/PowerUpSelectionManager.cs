using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PowerUpSelectionManager : MonoBehaviour
{
    public static PowerUpSelectionManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
            
        DontDestroyOnLoad(Instance.gameObject);
    }

    public void SetupDamageBoost(float boost)
    {
        GameManager.Instance.AddToPlayerDamageBoost(boost);
    }

    public void SetupSpeedBoost(float boost)
    {
        GameManager.Instance.AddToPlayerSpeedBoost(boost);
    }

    public void SetupVitalityBoost(float boost)
    {
        float currentHp = GameManager.Instance.PlayerMaxHitPoints;
        float currentMax = GameManager.Instance.PlayerCurrentHitPoints;
        
        GameManager.Instance.SetPlayerHitPoints(currentHp, boost);
        GameManager.Instance.SetPlayerMaxHitPoints(currentMax, boost);
    }
}
