using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool isDoorOpen; // TODO: modify this value when the door operning conditionals are met
    [SerializeField] private GameObject TextUI;
    private Animator TextDoorAnimator;

    void Start()
    {
        TextDoorAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool engagedInCombat = other.gameObject.GetComponent<PlayerInputManager>().isInCombat;
            bool hasRelic = other.gameObject.GetComponent<PlayerPossessions>().possessesRelic;

            if (!engagedInCombat && hasRelic)
            {
                isDoorOpen = true;
                TextDoorAnimator.SetBool("DoorIsOpen", isDoorOpen);
                TextDoorAnimator.SetFloat("direction", 1.0f);
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
                TextDoorAnimator.SetBool("DoorIsOpen", isDoorOpen);
                TextDoorAnimator.SetFloat("direction", -1.0f); // reverse animation
            }
        }
    }
}
