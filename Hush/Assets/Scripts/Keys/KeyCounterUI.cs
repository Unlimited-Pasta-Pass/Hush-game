using UnityEngine;
using TMPro;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyCountText;
    
    void Update()
    {
        keyCountText.text = "Keys: " + GameMaster.keysInPossession;
    }
}
