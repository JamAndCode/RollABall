using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;
    public ToolTipGeneration tooltip;

    public void Awake()
    {
        instance = this;
    }
    public static void Show(string message)
    {
        instance.tooltip.SetText(message);
        instance.tooltip.gameObject.SetActive(true);   
    }

    public static void Hide()
    {
        instance.tooltip.SetText("");
        instance.tooltip.gameObject.SetActive(false);   
    }
}
