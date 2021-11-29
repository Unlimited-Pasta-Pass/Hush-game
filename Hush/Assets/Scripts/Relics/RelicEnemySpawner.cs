using Common.Enums;
using UnityEngine;

namespace Enemies
{
   public class RelicEnemySpawner : MonoBehaviour
   {
      [SerializeField] private GameObject enemyPrefab;
      [SerializeField] private GameObject healthUI;
   
      public void OnRelicAttacked()
      {
         SpawnEnemies();
         ShowUI();
      }

      void SpawnEnemies()
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

      void ShowUI()
      {
         healthUI.SetActive(true);
      }
   
   }
}
