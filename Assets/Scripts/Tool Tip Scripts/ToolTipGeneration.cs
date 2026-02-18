using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class ToolTipGeneration : MonoBehaviour
{
    public TextMeshProUGUI toolTipText;
    public LayoutElement layoutElement;
    private int characterWrapLimit = 50;
    public RectTransform rectTrans;
    private void Awake() {
        rectTrans = GetComponent<RectTransform>();
    }
    public void SetText(string message)
    {
        toolTipText.text = message;
        int textLength = toolTipText.text.Length;
        layoutElement.enabled = textLength > characterWrapLimit;
    }
    private void Update()
    {
        Vector2 position = Mouse.current.position.ReadValue();
        transform.position = position;
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTrans.pivot = new Vector2(pivotX, pivotY);
    }
}
