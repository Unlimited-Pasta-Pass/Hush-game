using System;
using System.Collections.ObjectModel;
using System.Linq;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Keys
{
    public class KeySpawner : MonoBehaviour
    {
        [Header("Parameters")] 
        [SerializeField] private int numberOfKeysToSpawn = 3;

        private ReadOnlyDictionary<Guid, bool> Spawners => GameManager.Instance.KeySpawnersInUse;

        private void Start()
        {
            SelectKeySpawners();
        }

        public void SelectKeySpawners()
        {
            // If some spawners were already selected spawn keys at the selected spawners
            if (Spawners.Count > 0)
            {
                var spawnersInUse = Spawners.Where(k => k.Value && !GameManager.Instance.KeysInPossession.Contains(k.Key));
                foreach (var guid in spawnersInUse.Select(s => s.Key))
                {
                    SpawnKey(guid);
                }
            }
            // Otherwise spawn the required number of keys in the scene at random spawners
            else
            {
                FindKeySpawners();
                
                for (var i = 0; i < Math.Min(numberOfKeysToSpawn, Spawners.Count); i++)
                {
                    SpawnKey();
                }
            }
        }

        private void FindKeySpawners()
        {
            var keySpawners = FindObjectsOfType<KeySpawnPoint>();

            if (keySpawners.Length <= 0)
                throw new UnityException("No Key Spawner was found.");

            GameManager.Instance.UpdateKeySpawnerList(keySpawners.Select(k => k.GetComponent<GuidComponent>().GetGuid()));
        }

        private void SpawnKey()
        {
            int index;
            
            // Make sure we select a spawn point that is not already in use
            do
            {
                index = (int)Mathf.Floor(Random.Range(0f, Spawners.Count));
            } while (Spawners.ElementAt(index).Value);
            
            // Spawn the key in the scene
            SpawnKey(Spawners.ElementAt(index).Key);
            
            // Mark down the spawner as in use
            GameManager.Instance.UseKeySpawner(Spawners.ElementAt(index).Key);
        }

        private void SpawnKey(Guid guid)
        {
            var spawnGo = GuidManager.ResolveGuid(guid);

            if (spawnGo == null)
                return;
            
            var spawnPoint = spawnGo.GetComponent<KeySpawnPoint>();
            
            if (spawnPoint == null)
                return;
            
            spawnPoint.SpawnKey();
        }
    }
}
