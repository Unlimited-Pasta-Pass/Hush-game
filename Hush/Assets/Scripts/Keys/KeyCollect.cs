using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour
{

    private static int numOfKeys = 0;
    public static int NumOfKeys {
        get {
            return numOfKeys;
        }
    }
    public static int keys = 0;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            numOfKeys += 1;
            gameObject.SetActive(false);
            //Destroy (gameObject);
        }
    }
}
