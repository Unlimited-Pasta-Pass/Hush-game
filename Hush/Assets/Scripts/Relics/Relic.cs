using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Relic : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag(Tags.Player)) {
            GameMaster.RelicCollect ();
            gameObject.SetActive(false);
        }
    }
}
