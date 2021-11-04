using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using Common;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class KillableHealthUI : MonoBehaviour
{
    [SerializeField] private Camera virtualCam;
    [SerializeField] private GameObject killableObject;
    [SerializeField] private Slider healthSlider;

    private int maxHealth;

    void Start()
    {
        virtualCam = Camera.main;
        maxHealth = (int) healthSlider.value;
    }
    
    void Update()
    {
        healthSlider.value = killableObject.GetComponent<IKillable>().HitPoints;
    }

    void LateUpdate()
    {
       transform.LookAt(virtualCam.transform.position, -Vector3.up);
       transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}