using UnityEngine;
using TMPro;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyCountText;
    [SerializeField] private GameObject keySprite;
    
    // TODO: take this out of Update()
    void Update()
    {
        if (GameMaster.keysInPossession > 0)
        {
            keySprite.SetActive(true);
        }
        else
        {
            keySprite.SetActive(false);
            keyCountText.text = "";
        }

        if (GameMaster.keysInPossession > 1)
        {
            keyCountText.text = "x " + GameMaster.keysInPossession;
        }
    }
}
