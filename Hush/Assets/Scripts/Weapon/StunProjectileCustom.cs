using Common.Interfaces;
using DigitalRuby.PyroParticles;
using Enemies;
using UnityEngine;

namespace Weapon
{
    public class StunProjectileCustom : FireProjectileScript
    {
        
        public override void HandleCollision(GameObject obj, Collision c)
        {
            base.HandleCollision(obj, c);
            
            if(c.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Stun();
            }
        }
        
    }
}