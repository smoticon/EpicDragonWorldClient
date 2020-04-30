using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemTypeText;
    [SerializeField] Text ItemDescriptionText;
    [SerializeField] Text ItemStatsText;
    [SerializeField] Camera uiCamera;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(Item item)
    {
        ItemNameText.text = item.ItemName;
        ItemTypeText.text = item.GetItemType();
        ItemDescriptionText.text = item.GetDescription();
        ItemStatsText.text = item.GetStatsDescription();
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        Vector2 localPoint;
        Vector3 mPosition = Input.mousePosition;
        mPosition.x = mPosition.x + 150;
        mPosition.y = mPosition.y + 100;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), mPosition, uiCamera, out localPoint);
        
        transform.localPosition = localPoint;

    }
}
