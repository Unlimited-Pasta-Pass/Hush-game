using Common;
using DigitalRuby.PyroParticles;
using UnityEngine;

namespace Plugins
{
    public class CustomFireCollision : FireCollisionForwardScript
    {
        protected override void HandleCollision(Collision other)
        {
            if (other.gameObject.CompareTag(Tags.Player) || other.gameObject.CompareTag(Tags.Ground) )
            {
                return;
            }
            
            base.HandleCollision(other);
        }
    }
}