using System.Collections;
using Common.Enums;
using UnityEngine;

namespace Relics
{
   public class RelicEnemySpawner : MonoBehaviour
   {
      [SerializeField] private GameObject enemyPrefab;
      [SerializeField] private GameObject smokePrefab;
      [SerializeField] private float enemySpawnDelay = 0.3f;
      [SerializeField] private float smokeDuration = 1.5f;
      private GameObject relic;

      private void Awake()
      {
         relic = GameObject.Find(Tags.Relic);
      }

      public void OnRelicAttacked()
      {
         SpawnEnemies();
      }

      void SpawnEnemies()
      {
         GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(Tags.EnemySpawnPoint);

         foreach (var spawn in spawnPoints)
         {
            Vector3 position = spawn.transform.position;
            
            CreateSmoke(position);
            StartCoroutine(CreateEnemy(position));
         }
      }

      private void CreateSmoke(Vector3 position)
      {
         var smokeClone = Instantiate(smokePrefab, position, Quaternion.identity);
         smokeClone.GetComponent<ParticleSystem>().Play();
         Destroy(smokeClone, smokeDuration);
      }

      IEnumerator CreateEnemy(Vector3 position)
      {
         yield return new WaitForSeconds(enemySpawnDelay);
         
         var enemyClone = Instantiate(enemyPrefab, position, Quaternion.identity);
         enemyClone.transform.LookAt(relic.transform, Vector3.up);
      }
   }
}
