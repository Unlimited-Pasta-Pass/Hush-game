using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PowerUpSelectionManager : MonoBehaviour
{
    public static PowerUpSelectionManager Instance;
    private int currentlySelected = 0;
    [SerializeField] private float speedBoost = 0.5f;
    [SerializeField] private float damageBoost =  10f;
    [SerializeField] private float vitalityBoost = 15f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
            
        DontDestroyOnLoad(Instance.gameObject);
    }

    public void OnSubmit()
    {
        switch(currentlySelected)
        {
            case 1:
                SetupDamageBoost(damageBoost);
                break;
            case 2:
                SetupSpeedBoost(speedBoost);
                break;
            case 3:
                SetupVitalityBoost(vitalityBoost);
                break;
            default: // select nothing
                break;
        }
    }

    public void SetCurrentlySelected(int selected)
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
        float currentHp = GameManager.Instance.PlayerCurrentHitPoints; 
        float currentMax = GameManager.Instance.PlayerMaxHitPoints;
        
        GameManager.Instance.SetPlayerHitPoints(currentHp, boost);
        GameManager.Instance.SetPlayerMaxHitPoints(currentMax, boost);
    }
}
