using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D text;
    int levelUnlocked = 1;
    public bool mousein = false;
    private Sprite sprite;
    public GameObject prefab;
    GameObject inventory;
    GameObject ship;
    GameObject canvas;
    GameObject thisSlot;
    double lasttime;
    string parentName;
    InfoTable table;
    ItemData data;
    int slotIndex;
    Recipe recipe;
    GameObject tooltip;

    // Use this for initialization
    void Start()
    {
        ship = GameObject.Find("SpaceShip");
        canvas = GameObject.Find("Canvas");
        inventory = GameObject.Find("Inventory");
        tooltip = GameObject.Find("Canvas").transform.Find("Tooltip").gameObject;
        lasttime = ship.GetComponent<GameController>().playtime;

        //Determine slot ID (for "Crafting slot (10)" determine number "10")
        parentName = transform.parent.name;
        thisSlot = GameObject.Find(parentName);
        string index = ""; int integer;

        foreach(char value in parentName)
        {
            if (int.TryParse(value.ToString(), out integer))
            {
                index += value;
            }
        }
        slotIndex = int.Parse(index); //index of actual Slot
        LoadSprite(1);
    }

    void LoadSprite(int newLevel)
    {
        levelUnlocked = newLevel;

        //Find recipes and sprites
        recipe = inventory.GetComponent<RecipeDatabase>().database[slotIndex];
        if (recipe.Level > levelUnlocked)
        {
            sprite = Resources.Load<Sprite>("items/Lock"); //locking sprite
        }
        else
        {
            sprite = Resources.Load<Sprite>("items/" + recipe.Name);
        }
        thisSlot.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    void LoadSpritesOfOtherObjects(int newLevel)
    {
        for (int i = 0; i <= 16; i++) //till max number 
        {          
            GameObject.Find("CraftingPanel").transform.GetChild(i).GetChild(0).GetComponent<CraftingHandler>().LoadSprite(newLevel);
        }
    }

    public void OnMouseClick() //Let's craft something!
    {
        bool possible = true;

        if (recipe.Level <= levelUnlocked)
        {
            foreach (Item i in recipe.resources)
            {
                if (i.Name == "Health")
                {
                    if (ship.GetComponent<GameController>().hP < i.Amount)
                    {
                        possible = false;
                    }
                }
                else
                {
                    int itemIndex = inventory.GetComponent<Inventory>().FindItem(i.Name);
                    if (itemIndex == -1)
                    {
                        possible = false;
                    }
                    else if (inventory.GetComponent<Inventory>().inventory[itemIndex].Amount < i.Amount)
                    {
                        possible = false;
                    }
                }   
            }
        }
        else
        {
            possible = false;
        }

        if (possible)
        {
            inventory.GetComponent<Inventory>().RiseInfoPrefab(recipe.Name + " unlocked!");

            foreach (Item i in recipe.resources)
            {
                inventory.GetComponent<Inventory>().RemoveItem(i.Name, i.Amount);
            }

            //Show funcionality of crafted things
            switch (inventory.GetComponent<ItemDatabase>().GetItemData(recipe.Name).type)
            {

                case (ItemType.ship_upgrade):
                    switch (recipe.Name)
                    {
                        case ("Upgrade fuel storage"):
                            ship.GetComponent<GameController>().maxFuelCount = 500;
                            break;
                        case ("Upgrade food storage"):
                            ship.GetComponent<GameController>().maxFoodCount = 200;
                            break;
                        case ("Upgrade engine"):
                            ship.GetComponent<Ship>().MakeShipFaster();
                            break;
                        case ("Navigation upgrade"):
                            GameObject.Find("Arrow_pointer").GetComponent<ArrowHandler>().UpgradeRadar();
                            break;
                        case ("Patch I"):
                            if(ship.GetComponent<GameController>().hP + 35 < ship.GetComponent<GameController>().maxHP)
                                ship.GetComponent<GameController>().hP += 35;
                            else
                                ship.GetComponent<GameController>().hP = ship.GetComponent<GameController>().maxHP;
                            break;
                        case ("Patch II"):
                            if (ship.GetComponent<GameController>().hP + 75 < ship.GetComponent<GameController>().maxHP)
                                ship.GetComponent<GameController>().hP += 75;
                            else
                                ship.GetComponent<GameController>().hP = ship.GetComponent<GameController>().maxHP;
                            break;
                        case ("Workbench"):
                            levelUnlocked = 2;
                            LoadSpritesOfOtherObjects(levelUnlocked);
                            break;
                        case ("Workbench II"):
                            levelUnlocked = 3;
                            LoadSpritesOfOtherObjects(levelUnlocked);
                            break;
                    }
                    break;

                case (ItemType.inventory_items):
                    switch (recipe.Name)
                    {
                        case ("Gas absorbent tool"):
                            break;
                        case ("Canned food"):
                            ship.GetComponent<GameController>().foodCount -= 10;
                            break;
                        case ("Mining drill"):
                            break;
                        case ("Hammer"):
                            break;
                        case ("Shield for the asteroid belt"):

                            break;
                    }
                    inventory.GetComponent<Inventory>().AddItem(recipe.Name, 1);
                    break;

                case (ItemType.outside_items):
                    inventory.GetComponent<Inventory>().AddItem(recipe.Name, 1);
                    break;
            }
        }
        else
        {
            inventory.GetComponent<Inventory>().RiseInfoPrefab("Not possible!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (recipe.Level > levelUnlocked)
            return;

        data = inventory.GetComponent<ItemDatabase>().GetItemData(recipe.Name);

        //Resources description pre-prepare.
        string resources = "Resources:";
        foreach (Item i in recipe.resources)
        {
            resources += '\n' + i.Name + ' ' + i.Amount;
        }
       
        tooltip.GetComponent<TooltipHandler>().showTooltip(transform, data.Name, data.Description, resources);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.GetComponent<TooltipHandler>().hideTooltip();
    }
}
