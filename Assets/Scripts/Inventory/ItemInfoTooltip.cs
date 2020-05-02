using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemInfoTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Image itemImage;

    [SerializeField] Text ItemStatsText;

    [Header("Buttons")]
    [SerializeField] Button enhanceButton;
    [SerializeField] Button jewelsButton;
    [SerializeField] Button traitsButton;
    [SerializeField] Button extractButton;

    [SerializeField] Button sellButton;
    [SerializeField] Button equipButton;

    void Start()
    {
        gameObject.SetActive(false);


    }


    public void ShowTooltip(Character character, Item item)
    {
        ItemNameText.text = item.ItemName;
        itemImage.sprite = item.Icon;
        ItemStatsText.text = item.GetStatsDescription();
        gameObject.SetActive(true);
        //        equipButton.GetComponent<Button>().onClick.AddListener(new UnityAction(() => { Equip(item); }));
        enhanceButton.onClick.AddListener(() => { Debug.Log("TODO: Enhance Method"); });
        jewelsButton.onClick.AddListener(() => { Debug.Log("TODO: Jewels Method"); });
        traitsButton.onClick.AddListener(() => { Debug.Log("TODO: Traits Method"); });
        extractButton.onClick.AddListener(() => { Debug.Log("TODO: Extract Method"); });

        sellButton.onClick.AddListener(() => { Debug.Log("TODO: Sell Method"); });
        equipButton.onClick.AddListener(() => {
            character.Equip(item);
            HideTooltip(); });

    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
