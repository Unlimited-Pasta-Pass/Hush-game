using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            gameObject.SetActive(false);
            //Destroy (gameObject);
            GameMaster.KeyCollect (this);
        }
    }
}