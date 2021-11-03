using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
   [SerializeField] private GameObject RelicArea;
   [SerializeField] private GameObject EnemyPrefab;
   UnityEvent RelicDestroyEvent = new UnityEvent();
   
   void Start()
   {
      //RelicDestroyEvent.AddListener(OnRelicDestroy);
      SpawnEnemy();
   }
   void OnRelicDestroy()
   {
      SpawnEnemy();
   }

   void Update()
   {
      // if (RelicArea.GetComponent<RelicScript>().health < 100)
      // {
      //    RelicDestroyEvent.Invoke();
      // }
   }
   
   void SpawnEnemy()
   {
      GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(Tags.EnemySpawnPoint);
      for (int i = 0; i < spawnPoints.Length; ++i)
      {
         GameObject enemyClone = Instantiate(EnemyPrefab, spawnPoints[i].transform.position ,Quaternion.identity);
         
         GameObject target = GameObject.FindWithTag(Tags.Relic);
         enemyClone.transform.LookAt(target.transform, Vector3.up);
         
         //TODO change clone vision
      }
   }
   
}
