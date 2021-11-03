using Common;
using UnityEngine;

public class Relic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (!GameMaster.IsPlayerInCombat() && !GameMaster.playerHasRelic)
            {
                GameMaster.HasRelic(true);
                gameObject.SetActive(false);
            }
        }
    }
}
