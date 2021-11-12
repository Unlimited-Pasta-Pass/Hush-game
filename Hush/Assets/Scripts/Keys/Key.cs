using UnityEngine;
using Common;

public class Key : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(Tags.Player)) 
            return;
        
        GameManager.Instance.CollectKey(transform.parent.gameObject.GetInstanceID());
        gameObject.SetActive(false);
    }
}
