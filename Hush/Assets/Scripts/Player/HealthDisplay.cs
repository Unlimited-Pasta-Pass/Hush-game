using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay instance;
    [SerializeField]private TextMeshProUGUI health;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health.text = "ah"; // change this

    }

    // void Update()
    // {
    //     Debug.Log(PlayerHealthManager.instance.health.ToString());
    //     Debug.Log("health: " + health);
    // }
    //
    
    public void UpdateHealth(int newHealth)
    {
        health.SetText(newHealth.ToString());
    }
}
