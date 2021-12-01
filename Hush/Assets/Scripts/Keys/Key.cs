using Common.Enums;
using Game;
using UnityEngine;

namespace Keys
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private AudioSource collectKeyAudio;

        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(Tags.Player)) 
                return;
            
            AudioSource.PlayClipAtPoint(collectKeyAudio.clip, transform.position);
            GameManager.Instance.CollectKey(transform.parent.GetComponent<GuidComponent>().GetGuid());
            Destroy(gameObject);
        }
    }
}
