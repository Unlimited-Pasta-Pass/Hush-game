using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Text))]
public class KeyCounterUI : MonoBehaviour
{
    private TMP_Text keyCountText;

    void Awake () {
        keyCountText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        keyCountText.text = "Keys: " + GameMaster.keysInPossession.ToString();
    }
}
