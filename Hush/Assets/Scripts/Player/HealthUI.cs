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
    
    void Update()
    {
        healthValue.text = player.HitPoints.ToString();
        healthSlider.value = player.HitPoints;
    }
    
}
