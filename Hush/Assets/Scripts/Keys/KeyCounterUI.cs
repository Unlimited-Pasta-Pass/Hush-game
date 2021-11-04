using UnityEngine;
using TMPro;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyCountText;
    [SerializeField] private GameObject keySprite;

    public void SetKeySpriteVisibility(bool isVisible)
    {
        keySprite.SetActive(isVisible);

        if(!isVisible)
        {
            keySprite.SetActive(false);
        }
    }

    public void UpdateKeyCounter()
    {
        if(GameMaster.keysInPossession == 0)
        {
            keyCountText.text = "";
            return;
        }
        keyCountText.text = "x " + GameMaster.keysInPossession;
    }
}
