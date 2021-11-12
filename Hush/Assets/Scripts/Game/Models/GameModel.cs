using System;
using System.Collections.Generic;

namespace Game.Models
{
    [Serializable]
    public class GameModel
    {
        public float playerCurrentHitPoints;
        public float playerMaxHitPoints;
    
        public bool playerHasRelic;
        public int keysInPossession;
        public HashSet<int> enemiesAttacking;

        public int currentlyLoadedScene;
        public Dictionary<int, bool> keySpawnersInUse;
    }
}