using System;
using System.Collections.Generic;
using Common.Models;
using UnityEngine;

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
        // Player
        public float playerCurrentHitPoints;
        public float playerMaxHitPoints;
        public SerializableTransform playerTransform;
        
        // Relic
        public bool playerHasRelic;
        
        // Keys
        public Dictionary<int, bool> keySpawnersInUse;
        public HashSet<int> keysInPossession;

        // Enemies
        public HashSet<int> enemiesAttacking;
        public Dictionary<int, SerializableTransform> enemiesTransforms;
        
        // Scene
        public int currentlyLoadedScene;

        public GameState()
        {
            playerCurrentHitPoints = 0f;
            playerMaxHitPoints = 0f;
            playerHasRelic = false;
            playerTransform = new SerializableTransform(Vector3.zero, Quaternion.identity);
            
            keySpawnersInUse = new Dictionary<int, bool>();
            keysInPossession = new HashSet<int>();
            
            enemiesAttacking = new HashSet<int>();
            enemiesTransforms = new Dictionary<int, SerializableTransform>();
            
            currentlyLoadedScene = 0;
        }
    }
}