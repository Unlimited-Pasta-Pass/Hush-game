using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PowerUpSelectionPermanentManager : MonoBehaviour
{
    public static PowerUpSelectionPermanentManager Instance;
    
    [SerializeField] private float speedBoost = 0.5f;
    [SerializeField] private float damageBoost =  10f;
    [SerializeField] private float vitalityBoost = 15f;
    
    private int _currentlySelected = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
            
        DontDestroyOnLoad(Instance.gameObject);
    }

    public void OnSubmit()
    {
        switch(_currentlySelected)
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
        _currentlySelected = selected;
    }

    private void SetupDamageBoost(float boost)
    {
        GameManager.Instance.BoostPermanentDamage(boost);
    }

    private void SetupSpeedBoost(float boost)
    {
        GameManager.Instance.BoostPermanentSpeed(boost);
    }

    private void SetupVitalityBoost(float boost)
    {
        GameManager.Instance.BoostPermanentVitality(boost);
    }
}
