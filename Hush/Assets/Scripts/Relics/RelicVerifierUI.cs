using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RelicVerifierUI : MonoBehaviour
{
    private Text RelicVerifierText;

    void Awake () {
        RelicVerifierText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        RelicVerifierText.text = "Relic: " + (GameMaster.HasRelic ? "Yes" : "No");
    }
}
