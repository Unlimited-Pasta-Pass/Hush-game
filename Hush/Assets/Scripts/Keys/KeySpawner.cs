using System;
using System.Collections.Generic;
using System.Linq;
using Keys;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class KeySpawner : MonoBehaviour
    {
        [Header("Parameters")] 
        [SerializeField] private int numberOfKeysToSpawn = 3;

        private KeySpawnPoint[] _keySpawners;

        private void Start()
        {
            FindKeySpawners();

            for (var i = 0; i < Math.Min(numberOfKeysToSpawn, _keySpawners.Length); i++)
            {
                SpawnKey();
            }
        }

        private void FindKeySpawners()
        {
            _keySpawners = FindObjectsOfType<KeySpawnPoint>();

            if (_keySpawners.Length <= 0)
                throw new UnityException("No Key Spawner was found.");

            GameManager.Instance.UpdateKeySpawnerList(_keySpawners.Select(k => k.gameObject.GetInstanceID()));
        }

        private void SpawnKey()
        {
            int index;
            
            // Make sure we select a spawn point that is not already in use
            do
            {
                index = (int)Mathf.Round(Random.Range(0, _keySpawners.Length));
            } while (GameManager.Instance.KeySpawnersInUse[_keySpawners[index].gameObject.GetInstanceID()]);
            
            // Spawn the key in the scene
            _keySpawners[index].SpawnKey();
            
            // Mark down the spawner as in use
            GameManager.Instance.UseKeySpawner(_keySpawners[index].gameObject.GetInstanceID());
        }
    }
}
