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
    GameController gc;
    GameObject thisSlot;
    double lasttime;
    string parentName;
    ItemData data;
    int slotIndex;
    Recipe recipe;
    GameObject tooltip;
   
    AudioClip craftPossible;
    AudioClip craftNotPossible;
    AudioClip patch;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        ship = GameObject.Find("SpaceShip");
        gc = ship.GetComponent<GameController>();
        inventory = GameObject.Find("Inventory");
        tooltip = GameObject.Find("Canvas").transform.Find("Tooltip").gameObject;
        lasttime = ship.GetComponent<GameController>().playtime;
        
        audioSource = transform.parent.parent.GetComponent<AudioSource>();
        craftPossible = Resources.Load<AudioClip>("Sounds/craft possible");
        craftNotPossible = Resources.Load<AudioClip>("Sounds/warnings/not possible");
        patch = Resources.Load<AudioClip>("Sounds/patch");

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
            bool upgraded = false;
            switch (recipe.Name)
            {
                case ("Upgrade fuel storage"):
                    upgraded = gc.upgFuelStorage;
                    break;
                case ("Upgrade food storage"):
                    upgraded = gc.upgFoodStorage;
                    break;
                case ("Upgrade engine"):
                    upgraded = gc.upgEngines;
                    break;
                case ("Navigation upgrade"):
                    sprite = Resources.Load<Sprite>("items/" + recipe.Name + gc.upgNavigation);
                    break;
                case ("Workbench I"):
                    upgraded = gc.upgWorkbench1;
                    break;
                case ("Workbench II"):
                    upgraded = gc.upgWorkbench2;
                    break;
            }

            if (recipe.Name == "Navigation upgrade") { }
            else if (upgraded)
                sprite = Resources.Load<Sprite>("items/" + recipe.Name + " B");
            else
                sprite = Resources.Load<Sprite>("items/" + recipe.Name + " A");
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
        bool upgraded = false;

        switch (recipe.Name)
        {
            case ("Upgrade fuel storage"):
                upgraded = gc.upgFuelStorage;
                break;
            case ("Upgrade food storage"):
                upgraded = gc.upgFoodStorage;
                break;
            case ("Upgrade engine"):
                upgraded = gc.upgEngines;
                break;
            case ("Navigation upgrade"):
                upgraded = gc.upgNavigation == 2 ? true : false;
                break;
            case ("Workbench I"):
                upgraded = gc.upgWorkbench1;
                break;
            case ("Workbench II"):
                upgraded = gc.upgWorkbench2;
                break;
        }


        if (recipe.Level <= levelUnlocked)
        {
            foreach (Item i in recipe.resources)
            {
                if (i.Name == "Health")
                {
                    if (ship.GetComponent<GameController>().hP < i.Amount)
                        possible = false;
                }
                else
                {
                    if (inventory.GetComponent<Inventory>().CheckItemCount(i.Name) < i.Amount)
                        possible = false;
                }   
            }

            if ((recipe.Name == "Patch I" || recipe.Name == "Patch II") && ship.GetComponent<GameController>().hP == ship.GetComponent<GameController>().maxHP)
                possible = false;
        }
        else
        {
            possible = false;
        }

        if (possible && ! upgraded)
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
                            ship.GetComponent<GameController>().maxFuelCount = inventory.GetComponent<Inventory>().database.GetItemData("Upgrade fuel storage").stats["storage"];
                            gc.upgFuelStorage = true;
                            break;
                        case ("Upgrade food storage"):
                            ship.GetComponent<GameController>().maxFoodCount = inventory.GetComponent<Inventory>().database.GetItemData("Upgrade food storage").stats["storage"];
                            gc.upgFoodStorage = true;
                            break;
                        case ("Upgrade engine"):
                            ship.GetComponent<Ship>().MakeShipFaster();
                            gc.upgEngines = true;
                            break;
                        case ("Navigation upgrade"):
                            GameObject.Find("Arrow_pointer").GetComponent<ArrowHandler>().UpgradeRadar();
                            if (gc.upgNavigation < 2)
                                gc.upgNavigation++;
                            break;
                        case ("Patch I"):
                            Mathf.Clamp((float)(ship.GetComponent<GameController>().hP += inventory.GetComponent<Inventory>().database.GetItemData("Patch I").stats["health"])
                                , 0, ship.GetComponent<GameController>().maxHP);
                            audioSource.PlayOneShot(patch);
                            break;
                        case ("Patch II"):
                            Mathf.Clamp((float)(ship.GetComponent<GameController>().hP += inventory.GetComponent<Inventory>().database.GetItemData("Patch II").stats["health"])
                                , 0, ship.GetComponent<GameController>().maxHP);
                            audioSource.PlayOneShot(patch);
                            break;
                        case ("Workbench I"):
                            levelUnlocked = 2;
                            LoadSpritesOfOtherObjects(levelUnlocked);
                            gc.upgWorkbench1 = true;
                            break;
                        case ("Workbench II"):
                            levelUnlocked = 3;
                            LoadSpritesOfOtherObjects(levelUnlocked);
                            gc.upgWorkbench2 = true;
                            break;
                        case ("Hyperspace drive"):
                            ship.GetComponent<GameController>().EndGame(true);
                            break;
                    }
                    LoadSprite(levelUnlocked);
                    break;
                    
                case (ItemType.inventory_items): 
                    switch (recipe.Name)
                    {
                        case ("Gas absorbent tool"):
                            break;
                        case ("Canned food"):
                            if (gc.foodCount < 20)
                                inventory.GetComponent<Inventory>().RiseInfoPrefab("You need more food!");
                            else
                                gc.foodCount -= 20;
                            break;
                        case ("Mining drill"):
                            break;
                        case ("Electronics"):
                            break;
                        case ("Aseroid Shield"):
                            break;
                        
                    }
                    inventory.GetComponent<Inventory>().AddItem(recipe.Name, 1);
                    break;

                case (ItemType.outside_items):
                    inventory.GetComponent<Inventory>().AddItem(recipe.Name, 1);
                    break;
            }

            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(craftPossible);
        }
        else
        {
            audioSource.PlayOneShot(craftNotPossible);
            if (upgraded)
                inventory.GetComponent<Inventory>().RiseInfoPrefab("Already built!");
            else
                inventory.GetComponent<Inventory>().RiseInfoPrefab("Not possible!");
        }
    }

    public void clearCraftPanel()
    {
        levelUnlocked = 1;
        LoadSprite(levelUnlocked);
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
