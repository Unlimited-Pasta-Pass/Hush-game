using Common;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private bool _isDoorOpen = true; // TODO: modify this value when the door opening conditionals are met

    private bool CanGoThroughDoor => !GameManager.Instance.IsPlayerInCombat && GameManager.Instance.PlayerHasRelic && _isDoorOpen;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(Tags.Player)) 
            return;
        
        if (CanGoThroughDoor)
        {
            // TODO Door opening animation
            wall.SetActive(false);
                
            // respawn back into scene
            SceneManager.Instance.LoadNextScene();
        }
    }
}
