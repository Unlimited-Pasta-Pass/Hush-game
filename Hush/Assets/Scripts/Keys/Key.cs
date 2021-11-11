using UnityEngine;
using Common;

public class Key : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(Tags.Player)) 
            return;
        
        GameManager.Instance.CollectKey();
        gameObject.SetActive(false);
    }
}
