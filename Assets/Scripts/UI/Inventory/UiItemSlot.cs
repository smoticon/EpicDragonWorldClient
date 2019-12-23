using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Author: Code Monkey
 * Date: November 15th 2019
 * Video: https://www.youtube.com/watch?v=BGr-7GZJNXg
 */
public class UiItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerDrag;
        if (obj != null)
        {
            RectTransform sourceSlot = obj.GetComponent<RectTransform>();
            RectTransform targetSlot = GetComponent<RectTransform>();
            sourceSlot.anchoredPosition = targetSlot.anchoredPosition;
            sourceSlot.SetParent(targetSlot);
        }
    }
}
