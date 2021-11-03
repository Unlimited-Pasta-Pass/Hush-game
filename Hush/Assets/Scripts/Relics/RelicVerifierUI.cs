using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Text))]
public class RelicVerifierUI : MonoBehaviour
{
    private TMP_Text RelicVerifierText;

    void Awake () {
        RelicVerifierText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        RelicVerifierText.text = "Relic: " + (GameMaster.playerHasRelic ? "Yes" : "No");
    }
}
