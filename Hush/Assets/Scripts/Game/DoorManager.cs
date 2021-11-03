using Common;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private bool isDoorOpen; // TODO: modify this value when the door operning conditionals are met

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (!GameState.instance.playerEngagedInCombat && GameState.instance.playerHasRelic)
            {
                isDoorOpen = true;
                wall.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (!GameState.instance.playerEngagedInCombat && GameState.instance.playerHasRelic)
            {
                isDoorOpen = false;
                wall.SetActive(true);
            }
        }
    }
}
