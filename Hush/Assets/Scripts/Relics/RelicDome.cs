using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RelicDome : MonoBehaviour
{
    public int keysNeededToUnlock = 0;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == Tags.Player && GameMaster.NumOfKeys == keysNeededToUnlock) {
            gameObject.SetActive(false);
            GameMaster.ResetKeys();
        }
    }
}
