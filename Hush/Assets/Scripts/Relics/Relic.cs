using Common;
using UnityEngine;

public class Relic : MonoBehaviour
{
    private bool canPickupRelic => !GameManager.Instance.IsPlayerInCombat && !GameManager.Instance.PlayerHasRelic;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(Tags.Player)) 
            return;
        
        if (canPickupRelic)
        {
            GameManager.Instance.CollectRelic();
            gameObject.SetActive(false);
        }
    }
}
