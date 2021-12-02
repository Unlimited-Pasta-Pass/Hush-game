using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PowerUpSelectionUI: MonoBehaviour
{
    [SerializeField] protected float speedBoost = 0.5f;
    [SerializeField] protected float damageBoost =  10f;
    [SerializeField] protected float vitalityBoost = 15f;
    
    [SerializeField] private GameObject powerBorder;
    [SerializeField] private GameObject speedBorder;
    [SerializeField] private GameObject vitalityBorder;
    
    protected int CurrentlySelected = 0;
    
    public virtual void OnSubmit()
    {
        switch(CurrentlySelected)
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
        }
    }

    public void SetCurrentlySelected(int selected)
    {
        powerBorder.SetActive(false);
        speedBorder.SetActive(false);
        vitalityBorder.SetActive(false);

        switch(selected)
        {
            case 1:
                powerBorder.SetActive(true);
                break;
            case 2:
                speedBorder.SetActive(true);
                break;
            case 3:
                vitalityBorder.SetActive(true);
                break;
        }

        CurrentlySelected = selected;
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
