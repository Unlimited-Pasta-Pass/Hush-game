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
        public float baseDamagePermanent;
        public float baseSpeedPermanent;
        public float baseVitalityPermanent;

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
        public int randomSceneSeed;
        public int sceneProgress;

        public GameState()
        {
            // Save
            saveTime = float.MinValue;

            // Permanent Progress
            baseDamagePermanent = 5f;
            baseSpeedPermanent = 2f;
            baseVitalityPermanent = 100f;
            
            UnsetHP();
            UnsetBoosts();
            UnsetSpells();
            UnsetFloorStats();

            // Scene
            currentlyLoadedScene = 0;
        }

        private void UnsetHP()
        {
            // HP
            playerCurrentHitPoints = -1f;
            playerMaxHitPoints = -1f;
        }
        private void UnsetBoosts()
        {
            damageBoost = 0f;
            speedBoost = 0f;
            vitalityBoost = 0f;
        }
        private void UnsetFloorStats()
        {
            playerTransform = new SerializableTransform(Vector3.zero, Quaternion.identity);
            isPlayerInvisible = false;
            
            // Echolocation
            echolocationActivationTime = float.MinValue;
            echolocationCooldownTime = 7f;
            
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
            randomSceneSeed = 0;
            sceneProgress = -1;
        }
        private void UnsetSpells()
        {
            // Spell
            activeHeavySpell = SpellType.None;
            heavySpellActivationTime = float.MinValue;
            heavySpellCooldownTime = 10f;
            activeLightSpell = SpellType.None;
            lightSpellActivationTime = float.MinValue;
            lightSpellCooldownTime = 5f;
        }
    }
}