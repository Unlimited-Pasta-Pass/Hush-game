using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHitPoint player;
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private Slider healthSlider;

    private int _maxHealth;

    private void Start()
    {
        _maxHealth = (int) healthSlider.value;
    }

    private void Update()
    {
        healthValue.text = $"{player.HitPoints} / {_maxHealth}";
        healthSlider.value = player.HitPoints;
    }
}
