using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool isOpen = false; // TODO: modify this value when the door operning conditionals are met

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool engagedInCombat = other.gameObject.GetComponent<PlayerInputManager>().isInCombat;
            bool hasRelic = other.gameObject.GetComponent<PlayerPossessions>().possessesRelic;

            if (!engagedInCombat && hasRelic)
            {
                isOpen = true;
            }
        }
    }
}
