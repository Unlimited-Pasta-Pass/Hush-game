using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player;
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private Slider healthSlider;

    private int maxHealth;

    void Start()
    {
        maxHealth = (int) healthSlider.value;
    }
    
    void Update()
    {
        healthValue.text = player.HitPoints.ToString() + " / " + maxHealth;
        healthSlider.value = player.HitPoints;
    }
}
