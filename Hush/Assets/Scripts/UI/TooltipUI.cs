using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;
    
    [SerializeField] private RectTransform canvasRectTransform;
    private RectTransform background;
    private TextMeshProUGUI text;
    private RectTransform rectTransform;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        background = transform.Find("Background").GetComponent<RectTransform>();
        text = transform.Find("TooltipText").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();
        
        HideTooltip();
    }

    private void Update()
    {
        Vector2 anchoredPosition = InputManager.Instance.look / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + background.rect.width > canvasRectTransform.rect.width)
        {
            // Tooltip left screen on right side
            anchoredPosition.x = canvasRectTransform.rect.width - background.rect.width;
        }
        
        if (anchoredPosition.y + background.rect.height > canvasRectTransform.rect.height)
        {
            // Tooltip left screen on top side
            anchoredPosition.x = canvasRectTransform.rect.height - background.rect.height;
        }
        
        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        text.SetText(tooltipText);
        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        background.sizeDelta = textSize + paddingSize;
    }

    private void Show(string tooltipText)
    {
        gameObject.SetActive(true);
        SetText(tooltipText);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip(string tooltipText)
    {
        Instance.Show(tooltipText);
    }

    public static void HideTooltip()
    {
        Instance.Hide();
    }
}
