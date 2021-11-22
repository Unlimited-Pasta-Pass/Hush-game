﻿using Common;
using DigitalRuby.PyroParticles;
using UnityEngine;

namespace Plugins
{
    public class CustomFireCollision : FireCollisionForwardScript
    {
        protected override void HandleCollision(Collision other)
        {
            if (other.gameObject.CompareTag(Tags.Player) || other.gameObject.CompareTag(Tags.Ground) || other.gameObject.CompareTag(Tags.Spell))
            {
                return;
            }
            
            base.HandleCollision(other);
        }
    }
}