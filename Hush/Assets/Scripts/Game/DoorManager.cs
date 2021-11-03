using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private bool isDoorOpen; // TODO: modify this value when the door operning conditionals are met

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool engagedInCombat = other.gameObject.GetComponent<PlayerInputManager>().isInCombat;
            bool hasRelic = other.gameObject.GetComponent<PlayerPossessions>().possessesRelic;

            if (!engagedInCombat && hasRelic)
            {
                isDoorOpen = true;
                wall.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool engagedInCombat = other.gameObject.GetComponent<PlayerInputManager>().isInCombat;
            bool hasRelic = other.gameObject.GetComponent<PlayerPossessions>().possessesRelic;

            if (!engagedInCombat && hasRelic)
            {
                isDoorOpen = false;
                wall.SetActive(true);
            }
        }
    }
}
