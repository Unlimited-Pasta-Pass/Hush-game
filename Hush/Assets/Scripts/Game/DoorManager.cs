using Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private bool isDoorOpen; // TODO: modify this value when the door opening conditionals are met

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (!GameMaster.IsPlayerInCombat() && GameMaster.playerHasRelic)
            {
                isDoorOpen = true;
                wall.SetActive(false);
                
                // respawn back into scene
                SceneManager.LoadScene("DemoRespawn");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (!GameMaster.IsPlayerInCombat() && GameMaster.playerHasRelic)
            {
                isDoorOpen = false;
                wall.SetActive(true);
            }
        }
    }
}
