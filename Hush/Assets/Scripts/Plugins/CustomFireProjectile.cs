using Common;
using Common.Enums;
using Common.Interfaces;
using DigitalRuby.PyroParticles;
using Player;
using UnityEngine;

namespace Plugins
{
    public class CustomFireProjectile : FireProjectileScript
    {
        public override void HandleCollision(GameObject obj, Collision c)
        {
            base.HandleCollision(obj, c);
            
            if (c.gameObject.TryGetComponent<IKillable>(out var killable))
            {
                if (!gameObject.CompareTag(Tags.Spell) || gameObject.layer != Layers.Enemy || !c.gameObject.CompareTag(Tags.Dome))
                {
                    killable.TakeDamage(Damage);
                }
            }
        }
    }
}
