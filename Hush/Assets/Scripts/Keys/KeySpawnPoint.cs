using Keys.Models;
using UnityEngine;

namespace Keys
{
    public class KeySpawnPoint : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private GameObject keyPrefab;
        
        [Space(10)]
        [SerializeField] private DebugParameters debugParameters;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = debugParameters.gizmoColor;
            Gizmos.DrawWireSphere(transform.position, debugParameters.gizmoRadius);
        }

        public void SpawnKey()
        {
            Instantiate(keyPrefab, transform);
        }
    }
}