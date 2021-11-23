using Common;
using Common.Interfaces;
using DigitalRuby.PyroParticles;
using UnityEngine;

namespace Plugins
{
    public class CustomFireProjectile : FireProjectileScript
    {
        public override void HandleCollision(GameObject obj, Collision c)
        {
            base.HandleCollision(obj, c);
            
            if(c.gameObject.TryGetComponent<IKillable>(out var killable))
            {
                killable.TakeDamage(Damage);
            }
        }
    }
}