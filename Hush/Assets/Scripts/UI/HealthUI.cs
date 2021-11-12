using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        healthSlider.minValue = 0f;
        healthSlider.maxValue = GameManager.Instance.PlayerMaxHitPoints;
    }

    private void Update()
    {
        healthValue.text = $"{GameManager.Instance.PlayerCurrentHitPoints} / {GameManager.Instance.PlayerMaxHitPoints}";
        healthSlider.value = GameManager.Instance.PlayerCurrentHitPoints;
    }
}
