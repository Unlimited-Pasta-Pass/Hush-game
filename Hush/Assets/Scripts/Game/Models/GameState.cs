using System;
using System.Collections.Generic;
using Common.Models;
using UnityEngine;

namespace Game.Models
{
    [Serializable]
    public class GameState
    {
        public float playerCurrentHitPoints;
        public float playerMaxHitPoints;
        public bool playerHasRelic;
        public SerializableTransform playerTransform;
        
        public Dictionary<int, bool> keySpawnersInUse;
        public HashSet<int> keysInPossession;

        public HashSet<int> enemiesAttacking;
        public Dictionary<int, SerializableTransform> enemyTransforms;
        
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
            enemyTransforms = new Dictionary<int, SerializableTransform>();
            
            currentlyLoadedScene = 0;
        }
    }
}