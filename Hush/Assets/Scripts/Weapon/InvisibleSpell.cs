using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapon;

public class InvisibleSpell : PlayerSpell
{
    [SerializeField] GameObject playerRoot;
    [SerializeField] private float alpha = 0.25f;
    [SerializeField] private float invisibleDuration = 5f;

    private bool canBecomeInvisible = true;
    private float invisibleCountdown;
    private bool isInvisible = false;

    protected void Update()
    {
        base.Update();
        
        invisibleCountdown += Time.deltaTime;
        if (invisibleCountdown >= invisibleDuration)
        { 
            canBecomeInvisible = true; 
            ToggleInvisibilty();
        }
    }

    protected override void CreateSpellAttack(float damage)
    {
        if (!canBecomeInvisible)
            return;

        canBecomeInvisible = false;
        ToggleInvisibilty();
    }

    private void ToggleTransparent()
    {
        isInvisible = !isInvisible;
        var renderers = playerRoot.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            Color newColor = r.material.color;
            newColor.a = isInvisible ? 1.0f : alpha;
            r.material.color = newColor;
        } 
    }

    private void ToggleOutline()
    {
        var outline = playerRoot.GetComponent<Outline>();
        outline.enabled = isInvisible;
    }

    private void ToggleInvisibilty()
    {
        ToggleOutline();
        ToggleTransparent();
        invisibleCountdown = 0f;
    }
}
