using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string Name;
    public string Description;
    public int maxStack;
    public Dictionary<string, int> stats;
    public Sprite sprite;
    public ItemType type;

    //for creating new item by values 
    public ItemData( string Name, string Description, Dictionary<string, int> stats, ItemType type, int maxStack = 5)
    {
        this.Name = Name;
        this.Description = Description;
        this.maxStack = maxStack;
        this.stats = stats;
        this.type = type;
        this.sprite = Resources.Load<Sprite>("items/" + Name);
    }

    //for creating new item by existing item
    public ItemData(ItemData item)
    {
        if (item == null)
        {
            Debug.Log("Item you trying to get data from is null");
            return;
        }
        this.Name = item.Name;
        this.Description = item.Description;
        this.maxStack = item.maxStack;
        this.stats = item.stats;
        this.type = item.type;
        this.sprite = item.sprite;
    }

    //creating empty item
    public ItemData()
    {
        this.Name = "";
    }
}

public enum ItemType
{
    basic, ship_upgrade, outside_items, inventory_items
}
