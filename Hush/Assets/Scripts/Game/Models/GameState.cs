using System;
using System.Collections.Generic;
using Common.Models;
using UnityEngine;
using Weapon.Enums;

namespace Game.Models
{
    /// <summary>
    /// IMPORTANT: Every field in this class should be serializable to avoid breaking the save games!
    /// If a field is not serializable, you will need to create an adapter to a serializable class.
    /// You can use SerializableTransform as an example.
    /// </summary>
    
    [Serializable]
    public class GameState
    {
        // Save
        public float saveTime;

        // Player
        public float playerCurrentHitPoints;
        public float playerMaxHitPoints;
        public SerializableTransform playerTransform;
        public bool isPlayerInvisible;
        public float damageBoost;
        public float speedBoost;
        public float vitalityBoost;
        
        // Permanent Stat
        public float damagePermanent;
        public float speedPermanent;
        public float vitalityPermanent;

        // Echolocation
        public float echolocationActivationTime;
        public float echolocationCooldownTime;
        
        // Spell
        public SpellType activeHeavySpell;
        public float heavySpellActivationTime;
        public float heavySpellCooldownTime;
        public SpellType activeLightSpell;
        public float lightSpellActivationTime;
        public float lightSpellCooldownTime;

        // Relic
        public bool playerHasRelic;
        public float relicDomeHitPoints;
        
        // Keys
        public Dictionary<Guid, bool> keySpawnersInUse;
        public HashSet<Guid> keysInPossession;

        // Enemies
        public HashSet<Guid> enemiesAttacking;
        public Dictionary<Guid, SerializableTransform> enemiesTransforms;
        public Dictionary<Guid, float> enemiesHitPoints;
        
        // Scene
        public int currentlyLoadedScene;

        public GameState()
        {
            // Save
            saveTime = float.MinValue;

            // Player
            playerCurrentHitPoints = -1f;
            playerMaxHitPoints = -1f;
            playerTransform = new SerializableTransform(Vector3.zero, Quaternion.identity);
            isPlayerInvisible = false;
            damageBoost = 0f;
            speedBoost = 0f;
            vitalityBoost = 0f;
            
            // Stat Improvements
            damagePermanent = 0f;
            speedPermanent = 0f;
            vitalityPermanent = 0f;

            // Echolocation
            echolocationActivationTime = float.MinValue;
            echolocationCooldownTime = 7f;

            // Spell
            activeHeavySpell = SpellType.None;
            heavySpellActivationTime = float.MinValue;
            heavySpellCooldownTime = 10f;
            activeLightSpell = SpellType.None;
            lightSpellActivationTime = float.MinValue;
            lightSpellCooldownTime = 5f;
            
            // Relic
            playerHasRelic = false;
            relicDomeHitPoints = -1f;
            
            // Keys
            keySpawnersInUse = new Dictionary<Guid, bool>();
            keysInPossession = new HashSet<Guid>();
            
            // Enemies
            enemiesAttacking = new HashSet<Guid>();
            enemiesTransforms = new Dictionary<Guid, SerializableTransform>();
            enemiesHitPoints = new Dictionary<Guid, float>();
            
            // Scene
            currentlyLoadedScene = 0;
        }
    }
}