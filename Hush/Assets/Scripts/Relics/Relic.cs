using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Relic : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == Tags.Player && GameMaster.HasRelic == false) {
            gameObject.SetActive(false);
            //Destroy (gameObject);
            GameMaster.RelicCollect (this);
        }
    }
}
