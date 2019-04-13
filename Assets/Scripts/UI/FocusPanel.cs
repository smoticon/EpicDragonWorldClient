using UnityEngine;
using UnityEngine.EventSystems;

public class FocusPanel : MonoBehaviour, IPointerDownHandler
{
    private RectTransform panel;

    private void Awake()
    {
        panel = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        panel.SetAsLastSibling();
    }
}