using UnityEngine;

public class Inventory : ItemContainer
{
    public static Inventory Instance { get; set; }

    [SerializeField] Transform itemsParent;

    public int inventorySlots = 25;
    public Transform slotPrefab;

    protected override void OnValidate()
    {
        if (itemsParent != null)
            itemsParent.GetComponentsInChildren(includeInactive: true, result: ItemSlots);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Instance = this;
        Slots();
    }

    public void Slots()
    {
        for (int i = 0; i < inventorySlots; i++)
        {
            Transform slot = Instantiate(slotPrefab);
            //slot.transform.SetParent(GameObject.FindObjectOfType<Inventory>().transform);
            slot.transform.SetParent(itemsParent, false);
            ItemSlots.Add(slot.gameObject.GetComponent<ItemSlot>());
        }

    }
}
