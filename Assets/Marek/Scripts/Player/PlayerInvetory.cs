using UnityEngine;

public class PlayerInvetory
{
    public PlayerController player;
    public Transform itemHolder;
    public Transform item { get; protected set; }
    public Weapon weapon;
    public Collector collector { get; protected set; }

    public PlayerInvetory(PlayerController Player)
    {
        player = Player;
        itemHolder = player.itemHolder;
    }

    public void AddItem(Transform Item)
    {
        if (item != null)
            return;

        item = Item;
        item.SetParent(itemHolder);
        item.localPosition = new Vector3();
        item.localRotation = new Quaternion();
        collector = item.GetComponent<Collector>();
        weapon = item.GetComponent<Weapon>();
        if (collector != null) collector.Collected(true);
    }

    public void DropItem()
    {
        if (item != null) item.SetParent(null, true);
        if (collector != null) collector.Collected(false);
        item = null;
        collector = null;
        weapon = null;
    }

    public void Reset()
    {
        item = null;
        collector = null;
        weapon = null;
    }
}
