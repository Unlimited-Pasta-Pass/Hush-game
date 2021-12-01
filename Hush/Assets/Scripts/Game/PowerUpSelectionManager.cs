using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PowerUpSelectionManager : MonoBehaviour
{
    public enum Choice
    {
        Strength = 0,
        Speed = 1,
        Vitality = 2,
    }
    
    public static PowerUpSelectionManager Instance;
    private Choice currentlySelected = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
            
        DontDestroyOnLoad(Instance.gameObject);
    }

    public void OnSubmit(float boost)
    {
        switch(currentlySelected)
        {
            case Choice.Strength:
                SetupDamageBoost(boost);
                break;
            case Choice.Speed:
                SetupSpeedBoost(boost);
                break;
            case Choice.Vitality:
                SetupVitalityBoost(boost);
                break;
            default: // select nothing
                break;
        }
    }

    public void SetCurrentlySelected(Choice selected)
    {
        currentlySelected = selected;
    }

    private void SetupDamageBoost(float boost)
    {
        GameManager.Instance.AddToPlayerDamageBoost(boost);
    }

    private void SetupSpeedBoost(float boost)
    {
        GameManager.Instance.AddToPlayerSpeedBoost(boost);
    }

    private void SetupVitalityBoost(float boost)
    {
        float currentHp = GameManager.Instance.PlayerMaxHitPoints;
        float currentMax = GameManager.Instance.PlayerCurrentHitPoints;
        
        GameManager.Instance.SetPlayerHitPoints(currentHp, boost);
        GameManager.Instance.SetPlayerMaxHitPoints(currentMax, boost);
    }
}
