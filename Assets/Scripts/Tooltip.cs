using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public GameObject tooltip;
    public TextMeshProUGUI tooltipText;
    public Vector2 offset = new Vector2(0.1f, 0.21f);
    public float padding = 5f;
    void Start()
    {
        
    }

    void Update()
    {
        if (tooltip.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition;
            float offsetX = Screen.width * offset.x;
            float offsetY = Screen.height * offset.y;
            if (mousePos.x > Screen.width / 2) { offsetX *= -1; }
            RectTransform tooltipRectTransform = tooltip.GetComponent<RectTransform>();
            tooltipRectTransform.position = new Vector2(mousePos.x + offsetX, mousePos.y + offsetY);
        }
    }

    public void Show(string text)
    {
        tooltip.SetActive(true);
        tooltipText.text = text;
    }

    public void Hide()
    {
        tooltip.SetActive(false);
        tooltipText.text = "";
    }
}
