using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            gameObject.SetActive(false);
            GameState.instance.CollectKey();
        }
    }
}
