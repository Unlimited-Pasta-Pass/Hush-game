using UnityEngine;
using TMPro;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyCountText;
    
    void Update()
    {
        keyCountText.text = "<color=\"yellow\">Keys:</color> " + GameMaster.keysInPossession;
    }
}
