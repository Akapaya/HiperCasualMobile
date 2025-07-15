using UnityEngine;

public struct StackItemStruct
{
    public IStackable Stackable;
    public ISellItem SellItem;
    public GameObject Item;

    public StackItemStruct(IStackable stackable, ISellItem sellItem, GameObject item)
    {
        this.Stackable = stackable;
        this.SellItem = sellItem;
        this.Item = item;
    }
}