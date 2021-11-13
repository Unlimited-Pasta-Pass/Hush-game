using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Keys;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public partial class GameManager
    {
        [Header("Keys")]
        public UnityEvent<int> keyCountChanged;
        
        public HashSet<Guid> KeysInPossession => _state.keysInPossession;

        private ReadOnlyDictionary<Guid, bool> _readOnlyDictionary;
        public ReadOnlyDictionary<Guid, bool> KeySpawnersInUse => _readOnlyDictionary ??= new ReadOnlyDictionary<Guid, bool>(_state.keySpawnersInUse);
        
        public void CollectKey(Guid instanceId)
        {
            _state.keysInPossession.Add(instanceId);
            keyCountChanged.Invoke(_state.keysInPossession.Count);
        }

        public void ResetKeys()
        {
            _state.keysInPossession.Clear();
        }
        
        public void UpdateKeySpawnerList(IEnumerable<Guid> spawners)
        {
            foreach (var spawner in spawners)
            {
                _state.keySpawnersInUse.Add(spawner, false);
            }
        }
    
        public void UseKeySpawner(Guid instanceId)
        {
            _state.keySpawnersInUse[instanceId] = true;
        }

        private void ApplyKeysState()
        {
            keyCountChanged.Invoke(_state.keysInPossession.Count);
            
            // Clear the spawned keys
            foreach (var key in FindObjectsOfType<Key>())
            {
                Destroy(key.gameObject);
            }
            
            // Spawn the loaded keys
            var spawner = FindObjectOfType<KeySpawner>();
            
            if (spawner == null)
                return;

            spawner.SelectKeySpawners();
        }
    }
}