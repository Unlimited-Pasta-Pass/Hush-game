using System.Collections;
using Common.Enums;
using UnityEngine;

namespace Enemies
{
   public class RelicEnemySpawner : MonoBehaviour
   {
      [SerializeField] private GameObject enemyPrefab;
      [SerializeField] private GameObject smokePrefab;

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
            StartCoroutine(CreateEnemey(position));
         }
      }

      private void CreateSmoke(Vector3 position)
      {
         var smokeClone = Instantiate(smokePrefab, position, Quaternion.identity);
         smokeClone.GetComponent<ParticleSystem>().Play();
         Destroy(smokeClone, 1.5f);
      }

      IEnumerator CreateEnemey(Vector3 position)
      {
         yield return new WaitForSeconds(.3f);
         
         var enemyClone = Instantiate(enemyPrefab, position, Quaternion.identity);
         var target = GameObject.FindWithTag(Tags.Relic);
         enemyClone.transform.LookAt(target.transform, Vector3.up);
      }
   }
}
