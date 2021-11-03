using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class KeyCounterUI : MonoBehaviour
{
    private Text keyCountText;

    void Awake () {
        keyCountText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        keyCountText.text = "Keys: " + GameMaster.NumOfKeys.ToString();
    }
}
