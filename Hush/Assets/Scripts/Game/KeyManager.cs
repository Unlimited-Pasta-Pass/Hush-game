using System.Collections.Generic;
using Keys;
using UnityEngine;

namespace Game
{
    public class KeyManager : MonoBehaviour
    {
        [Header("Parameters")] 
        [SerializeField] private int numberOfKeysToSpawn = 3;

        private KeySpawnController[] _keySpawners;
        
        private Dictionary<int, bool> _spawnersInUse;

        private void Start()
        {
            FindKeySpawners();

            for (var i = 0; i < numberOfKeysToSpawn; i++)
            {
                SpawnKey();
            }
        }

        private void FindKeySpawners()
        {
            _keySpawners = FindObjectsOfType<KeySpawnController>();

            if (_keySpawners.Length <= 0)
                throw new UnityException("No Key Spawner was found.");

            _spawnersInUse = new Dictionary<int, bool>();

            foreach (var spawner in _keySpawners)
            {
                _spawnersInUse.Add(spawner.GetInstanceID(), false);
            }
        }

        private void SpawnKey()
        {
            int index;
            
            // Make sure we select a spawn point that is not already in use
            do
            {
                index = (int)Mathf.Round(Random.Range(0, _keySpawners.Length));
            } while (_spawnersInUse[_keySpawners[index].GetInstanceID()]);
            
            // Spawn the key in the scene
            _keySpawners[index].SpawnKey();
            
            // Mark down the spawner as in use
            _spawnersInUse[_keySpawners[index].GetInstanceID()] = true;
        }
    }
}