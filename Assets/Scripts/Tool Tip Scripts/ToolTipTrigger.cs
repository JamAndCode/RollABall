using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(LoadToolTip());
    }

    IEnumerator LoadToolTip()
    {
        yield return new WaitForSeconds(1f);
        ToolTipSystem.Show(message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
    }
}
