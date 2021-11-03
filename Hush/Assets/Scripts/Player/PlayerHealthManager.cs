using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance;
    public int health;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = 100;
        HealthDisplay.instance.UpdateHealth(health); // namespaces hate us for some reason
    }

    void ModifyHealth(int modifier)
    {
        health += modifier;
        HealthDisplay.instance.UpdateHealth(modifier);
    }
}
