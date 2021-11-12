using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Events;

namespace Game
{
    public partial class GameManager
    {
        public UnityEvent<int> keyCountChanged;
        
        public HashSet<int> KeysInPossession => _state.keysInPossession;

        private ReadOnlyDictionary<int, bool> _readOnlyDictionary;
        public ReadOnlyDictionary<int, bool> KeySpawnersInUse => _readOnlyDictionary ??= new ReadOnlyDictionary<int, bool>(_state.keySpawnersInUse);
        
        public void CollectKey(int instanceId)
        {
            _state.keysInPossession.Add(instanceId);
            keyCountChanged.Invoke(_state.keysInPossession.Count);
        }

        public void ResetKeys()
        {
            _state.keysInPossession.Clear();
        }
        
        public void UpdateKeySpawnerList(IEnumerable<int> spawners)
        {
            foreach (var spawner in spawners)
            {
                _state.keySpawnersInUse.Add(spawner, false);
            }
        }
    
        public void UseKeySpawner(int instanceId)
        {
            _state.keySpawnersInUse[instanceId] = true;
        }
    }
}