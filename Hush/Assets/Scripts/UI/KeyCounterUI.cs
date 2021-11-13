using System;
using Game;
using Relics;
using TMPro;
using UnityEngine;

namespace UI
{
    public class KeyCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI keyCountText;
        [SerializeField] private GameObject keySprite;

        private int KeyCount => GameManager.Instance.KeysInPossession.Count;

        private RelicDome Dome => FindObjectOfType<RelicDome>(true);

        private void OnEnable()
        {
            GameManager.Instance.keyCountChanged.AddListener(OnKeyCountChanged);
            
            if (Dome != null)
            {
                Dome.Killed.AddListener(OnRelicDomeDestroyed); 
            }
        }

        private void OnDisable()
        {
            GameManager.Instance.keyCountChanged.RemoveListener(OnKeyCountChanged);
            
            if (Dome != null)
            {
                Dome.Killed.RemoveListener(OnRelicDomeDestroyed);
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        private void OnKeyCountChanged()
        {
            UpdateUI();
        }

        private void OnRelicDomeDestroyed()
        {
            keySprite.SetActive(false);
        }

        private void UpdateUI()
        {
            keySprite.SetActive(KeyCount > 0);
            keyCountText.text = KeyCount == 0 ? "" : $"x {KeyCount}";
        }
    }
}
