using Common.Enums;
using Game;
using UnityEngine;

namespace Keys
{
    public class Key : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) 
                return;
        
            GameManager.Instance.CollectKey(transform.parent.GetComponent<GuidComponent>().GetGuid());
            Destroy(gameObject);
        }
    }
}
