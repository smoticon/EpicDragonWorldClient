/**
* @author Pantelis Andrianakis
*/
public class CharacterDataHolder
{
    private string name = "";
    private byte slot = 0;
    private bool selected = false;
    private byte classId = 0;
    private string locationName = "";
    private float x = 0;
    private float y = 0;
    private float z = 0;
    private int heading = 0;
    private long experience = 0;
    private long hp = 0;
    private long mp = 0;
    private byte accessLevel = 0;
    private int itemHead = 0;
    private int itemChest = 0;
    private int itemGloves = 0;
    private int itemLegs = 0;
    private int itemBoots = 0;
    private int itemRightHand = 0;
    private int itemLeftHand = 0;

    public string GetName()
    {
        return name;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public byte GetSlot()
    {
        return slot;
    }

    public void SetSlot(byte slot)
    {
        this.slot = slot;
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void SetSelected(bool selected)
    {
        this.selected = selected;
    }

    public byte GetClassId()
    {
        return classId;
    }

    public void SetClassId(byte classId)
    {
        this.classId = classId;
    }

    public string GetLocationName()
    {
        return locationName;
    }

    public void SetLocationName(string locationName)
    {
        this.locationName = locationName;
    }

    public float GetX()
    {
        return x;
    }

    public void SetX(float x)
    {
        this.x = x;
    }

    public float GetY()
    {
        return y;
    }

    public void SetY(float y)
    {
        this.y = y;
    }

    public float GetZ()
    {
        return z;
    }

    public void SetZ(float z)
    {
        this.z = z;
    }

    public int GetHeading()
    {
        return heading;
    }

    public void SetHeading(int heading)
    {
        this.heading = heading;
    }

    public long GetExperience()
    {
        return experience;
    }

    public void SetExperience(long experience)
    {
        this.experience = experience;
    }

    public long GetHp()
    {
        return hp;
    }

    public void SetHp(long hp)
    {
        this.hp = hp;
    }

    public long GetMp()
    {
        return mp;
    }

    public void SetMp(long mp)
    {
        this.mp = mp;
    }

    public byte GetAccessLevel()
    {
        return accessLevel;
    }

    public void SetAccessLevel(byte accessLevel)
    {
        this.accessLevel = accessLevel;
    }

    public int GetItemHead()
    {
        return itemHead;
    }

    public void SetItemHead(int itemHead)
    {
        this.itemHead = itemHead;
    }

    public int GetItemChest()
    {
        return itemChest;
    }

    public void SetItemChest(int itemChest)
    {
        this.itemChest = itemChest;
    }

    public int GetItemGloves()
    {
        return itemGloves;
    }

    public void SetItemGloves(int itemGloves)
    {
        this.itemGloves = itemGloves;
    }

    public int GetItemLegs()
    {
        return itemLegs;
    }

    public void SetItemLegs(int itemLegs)
    {
        this.itemLegs = itemLegs;
    }

    public int GetItemBoots()
    {
        return itemBoots;
    }

    public void SetItemBoots(int itemBoots)
    {
        this.itemBoots = itemBoots;
    }

    public int GetItemRightHand()
    {
        return itemRightHand;
    }

    public void SetItemRightHand(int itemRightHand)
    {
        this.itemRightHand = itemRightHand;
    }

    public int GetItemLeftHand()
    {
        return itemLeftHand;
    }

    public void SetItemLeftHand(int itemLeftHand)
    {
        this.itemLeftHand = itemLeftHand;
    }
}
