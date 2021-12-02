using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PowerUpSelection : MonoBehaviour
{
    private int currentlySelected = 0;
    [SerializeField] private float speedBoost = 0.5f;
    [SerializeField] private float damageBoost =  10f;
    [SerializeField] private float vitalityBoost = 15f;
    [SerializeField] private GameObject powerBorder;
    [SerializeField] private GameObject speedBorder;
    [SerializeField] private GameObject vitalityBorder;

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
