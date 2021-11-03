using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class KeyCounterUI : MonoBehaviour
{
    private Text keyCountText;

    void Awake () {
        keyCountText = GetComponent<Text>();
    }

    void Update()
    {
        keyCountText.text = "Keys: " + GameMaster.keysInPossession.ToString();
    }
}
