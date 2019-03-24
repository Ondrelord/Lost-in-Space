using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string Name;
    public int Amount;
    public GameObject slot;
    ItemDatabase database;

    //contructor for Item  
    //Empty item have empty name string ""
    public Item(string Name = "", int Amount = 0)
    {
        this.Name = Name;
        this.Amount = Amount;
        database = GameObject.Find("Inventory").GetComponent<ItemDatabase>();
    }

    //constructor for Item
    //new copy of an item
    public Item(Item item)  
    {
        this.Name = item.Name;
        this.Amount = item.Amount;
        this.slot = item.slot;
        this.database = item.database;
    }
    
    //returning excess amount of item after reaching max stack or zero
    public int ChangeAmount (int amount)
    {
        amount += this.Amount;

        if (amount <= this.GetItemData().maxStack)              //if setted amount is less than max
        {
            if (amount > 0)                 //if its not negative
                this.Amount = amount;       //set amount to item 
            else
            {
                this.Amount = 0;            //if removing more than are in stacks
                return amount;              //return excess for further removing
            }
        }
        else
        {
            this.Amount = this.GetItemData().maxStack;              //if adding more than max stack
            return amount - this.GetItemData().maxStack;            //return excess for further adding
        }

        return 0;       //return 0 if theres no excess
    }





    //Set the amount of item    
    public void SetAmount(int amount)
    {
        if (amount <= this.GetItemData().maxStack && amount > 0)
            this.Amount = amount;
    }
    
    //Getting item data straight from database
    public ItemData GetItemData()
    {
        return database.GetItemData(Name);
    }
}
