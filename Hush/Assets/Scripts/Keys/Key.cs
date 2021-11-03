using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Key : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag(Tags.Player)) {
            GameMaster.CollectKey();
            gameObject.SetActive(false);
        }
    }
}
