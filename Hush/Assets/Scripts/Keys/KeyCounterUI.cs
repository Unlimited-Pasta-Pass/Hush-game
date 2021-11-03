using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyCountText;

    void Update()
    {
        keyCountText.text = "Keys: " + GameMaster.keysInPossession.ToString();
    }
}

