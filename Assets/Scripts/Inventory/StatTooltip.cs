using edw.CharacterStats;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] Text StatNameText;
    [SerializeField] Text StatModifiersLabelText;
    [SerializeField] Text StatModifiersText;
    [SerializeField] Camera uiCamera;

    private StringBuilder sb = new StringBuilder();

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip(CharacterStat stat, string statName)
    {
        StatNameText.text = GetStatTopText(stat, statName);
        StatModifiersText.text = GetStatModifiersText(stat);
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private string  GetStatTopText(CharacterStat stat, string statName)
    {
        sb.Length = 0;
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);

        if(stat.Value != stat.BaseValue)
        {
            sb.Append(" (");
            sb.Append(stat.BaseValue);
    
            if (stat.Value > stat.BaseValue)
                sb.Append("+");
    
            sb.Append(System.Math.Round(stat.Value - stat.BaseValue, 4));
            sb.Append(")");
        }
        return sb.ToString();
    }

    private string GetStatModifiersText(CharacterStat stat)
    {
        sb.Length = 0;

        foreach(StatModifier mod in stat.StatModifiers)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (mod.Value > 0)
                sb.Append("+");

            if(mod.Type == StatModType.Flat)
            {
                sb.Append(mod.Value);
            }
            else
            {
                sb.Append(mod.Value * 100);
                sb.Append("%");
            }

            Item item = mod.Source as Item;

            if(item != null)
            {
                sb.Append(" ");
                sb.Append(item.ItemName);
            }
            else
            {
                Debug.LogError("Modifier is not an Equippable Item!");
            }
        }

        return sb.ToString();
    }

    private void LateUpdate()
    {
        Vector2 localPoint;
        Vector3 mPosition = Input.mousePosition;
        mPosition.x = mPosition.x + 150;
        mPosition.y = mPosition.y + 100;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), mPosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
        transform.SetAsLastSibling();
    }
}
