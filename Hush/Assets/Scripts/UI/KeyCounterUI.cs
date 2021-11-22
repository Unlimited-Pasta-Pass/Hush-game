using UnityEngine;
using TMPro;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyCountText;
    [SerializeField] private GameObject keySprite;

    public void OnKeyCountChanged(int keyCount)
    {
        keySprite.SetActive(keyCount > 0);
        keyCountText.text = keyCount == 0 ? "" : $"x {keyCount}";
    }
}
