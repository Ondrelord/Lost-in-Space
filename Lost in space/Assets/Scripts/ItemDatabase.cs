using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();  //actual database

    void Awake()
    {
        BuildItemDatabase();        //Building database as soon as possible
    }

    //Getting item from database by name
    public ItemData GetItemData(string name)                        
    {
        return new ItemData( this.items.Find(item => item.Name == name));
        
    }


    // function for building database of items 
    /* for creating new items copy following lines into the code bellow and fill the formula
           ,
           new Item(ID, "Name", "Description",
           new Dictionary<string, int>{
                { "attrubute", value }
            })
    */
    private void BuildItemDatabase()
    {
        items = new List<ItemData>()
        {
           //Water
           new ItemData("Water", "Fluid essetial for life.",
           new Dictionary<string, int>{
                { "hydratation", 10 }
            }, ItemType.basic),

           //Basic 1
           new ItemData("Alloy", "Rafined metal. Used for crafting and repairs.",
           new Dictionary<string, int>{
            }, ItemType.basic),

           new ItemData("Helium", "Funny gas. It's used as fuel.",
           new Dictionary<string, int>{
                { "fuel", 50 }
            }, ItemType.basic),

           //Basic 2
           new ItemData("Gold", "Gold makes the ugly beautiful. Highly conductive metal.",
           new Dictionary<string, int>{
            }, ItemType.basic),

           new ItemData("Silicium", "It's necessary to craft some electronics.",
           new Dictionary<string, int>{
            }, ItemType.basic),

           //Basic 3
           new ItemData("Uranium", "It's glowing in the dark.",
           new Dictionary<string, int>{
            }, ItemType.basic),

           new ItemData("Ship part", "Debris of an alien ship.",
           new Dictionary<string, int>{
            }, ItemType.basic),

           new ItemData("Nuclear Fuel", "Have enough power for hyperspace travel.",
           new Dictionary<string, int>{
                { "fuel", 500 }
            }, ItemType.basic),

           //crafting
           new ItemData("Mining drill", "Mines metals from planets.",
           new Dictionary<string, int>{
            }, ItemType.inventory_items),

           new ItemData("Scanning station", "Automatically scans planet for resources.",
           new Dictionary<string, int>{
            }, ItemType.outside_items),

           new ItemData("Mining station", "Automatically scans and mines resources for you.",
           new Dictionary<string, int>{
            }, ItemType.outside_items),

           new ItemData("Gas absorbent tool", "Pumps gases from gas giants." + '\n' + "A real sucker.",
           new Dictionary<string, int>{
            }, ItemType.inventory_items),

           new ItemData("Upgrade fuel storage", "The bigger the better.",
           new Dictionary<string, int>{
                { "storage", 500 }
            }, ItemType.ship_upgrade),

           new ItemData("Upgrade engine", "Make ship 2x faster.",
           new Dictionary<string, int>{
                { "multiplier", 2 }
            }, ItemType.ship_upgrade),

           new ItemData("Farm", "Oink, oink. Potatoes and stuff.",
           new Dictionary<string, int>{
                { "boost", 7 }
            }, ItemType.outside_items),

           new ItemData("Upgrade food storage", "All you can eat.",
           new Dictionary<string, int>{
                { "storage", 200 }
            }, ItemType.ship_upgrade),

           new ItemData("Canned food", "Pig in can. Store your food for later.",
           new Dictionary<string, int>{
                { "food", 20 }
            }, ItemType.inventory_items),

           new ItemData("Patch I", "If you're hurt, try this!",
           new Dictionary<string, int>{
                { "health", 35 }
            }, ItemType.ship_upgrade),

           new ItemData("Patch II", "If you're very hurt, this is a must!",
           new Dictionary<string, int>{
                { "health", 75 }
            }, ItemType.ship_upgrade),

           new ItemData("Emergency fuel", "You can use your health to get fuel" + '\n' + "in emergency situation.",
           new Dictionary<string, int>{
                { "health", -10 },
                { "fuel", 50 }
            }, ItemType.ship_upgrade),

           new ItemData("Workbench I", "You can make stuff here.",
           new Dictionary<string, int>{
            }, ItemType.ship_upgrade),

           new ItemData("Workbench II", "You can make even more stuff!",
           new Dictionary<string, int>{
            }, ItemType.ship_upgrade),

           new ItemData("Navigation upgrade", "Make your radar more powerful!",
           new Dictionary<string, int>{
            }, ItemType.ship_upgrade),

           new ItemData("Asteroid Shield", "You can cross the asteroid belt.. finnaly.",
           new Dictionary<string, int>{
                { "time", 10 }
            }, ItemType.inventory_items),

           new ItemData("Electronics", "Electronics is necessary to craft machines and engines.",
           new Dictionary<string, int>{
            }, ItemType.inventory_items),

           new ItemData("Hyperspace drive", "Craft hyperspace drive ang GO gome!",
           new Dictionary<string, int>{
            }, ItemType.ship_upgrade),

           new ItemData("Hammer", "Good for nails. Also for breaking into things.",
           new Dictionary<string, int>{
            }, ItemType.inventory_items),

           new ItemData("Chest", "Stuff can be stored in here.",
           new Dictionary<string, int>{
                { "space", 10 }
            }, ItemType.outside_items),
        };
    }

}
