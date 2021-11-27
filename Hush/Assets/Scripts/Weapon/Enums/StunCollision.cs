using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common.Enums;
using Enemies;

namespace Weapon.Enums
{
    public class StunCollision : MonoBehaviour
    {
        public float duration = 1f;
        void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag(Tags.Enemy))
            {
                other.GetComponent<Enemy>().Stun(duration);
            }
        }
    }
}