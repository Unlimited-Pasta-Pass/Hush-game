using Common.Enums;
using UnityEngine;

namespace Enemies
{
   public class EnemySpawner : MonoBehaviour
   {
      [SerializeField] private GameObject enemyPrefab;
   
      public void OnRelicDestroy()
      {
         SpawnEnemy();
      }

      void SpawnEnemy()
      {
         GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(Tags.EnemySpawnPoint);

         foreach (var spawn in spawnPoints)
         {
            var enemyClone = Instantiate(enemyPrefab, spawn.transform.position , Quaternion.identity);
         
            var target = GameObject.FindWithTag(Tags.Relic);
            enemyClone.transform.LookAt(target.transform, Vector3.up);
         
            //TODO change clone vision
         }
      }
   
   }
}
