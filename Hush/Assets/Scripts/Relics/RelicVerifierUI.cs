using Game;
using TMPro;
using UnityEngine;

namespace Relics
{
    [RequireComponent(typeof(TMP_Text))]
    public class RelicVerifierUI : MonoBehaviour
    {
        private TMP_Text _relicVerifierText;

        private void Awake () {
            _relicVerifierText = GetComponent<TMP_Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            _relicVerifierText.text = $"Relic: {(GameManager.Instance.PlayerHasRelic ? "Yes" : "No")}";
        }
    }
}
