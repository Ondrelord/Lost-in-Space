using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject invPanel;
    public ItemDatabase database;
    public Text infoPrefab;

    private List<Text> infos = new List<Text>();
    public List<Item> inventory = new List<Item>();                        //list of inventory (actual invetory)
    public int INVSIZE = 5;

    private AudioClip trashSound;

    void Start()
    {
        invPanel = GameObject.Find("Inventory Panel");
        database = gameObject.GetComponent<ItemDatabase>();

        invPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((INVSIZE + 1) * 110) + 170);
        invPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 280);

        for (int i = 0; i < INVSIZE; i++)        // initializing inventory and slots(list) for every slot(gameObject) in GUI
        {
            inventory.Add(new Item());
            inventory[i].slot = Instantiate(Resources.Load("Prefabs/Inventory Slot"), invPanel.transform) as GameObject;
        }
        Instantiate(Resources.Load("Prefabs/Trash Slot"), invPanel.transform);

        trashSound = Resources.Load<AudioClip>("Sounds/trash");
    }

    void Update() //for rendering infos about getting new items into inventory
    {
        List<int> removed = new List<int>();

        for (int indexer = 0; indexer < infos.Count; indexer++)
        {
            infos[indexer].transform.Translate(0, -0.01f, 0); //I set this statically, because it works right..
            infos[indexer].GetComponent<CanvasGroup>().alpha = infos[indexer].GetComponent<CanvasGroup>().alpha - 0.01f;

            if (infos[indexer].GetComponent<CanvasGroup>().alpha == 0)
            {
                removed.Add(indexer);
            }
        }

        for (int i = removed.Count - 1; i >= 0; i--) //removing
        {

            Destroy(infos[removed[i]].gameObject);
            infos.RemoveAt(removed[i]);
        }
    }

    

    public bool AddItem(string name, int amount = 1)
    {
        if (amount == 0)
            return false;

        bool result = false;

        for (int i = 0; i < INVSIZE; i++)
        {
            if (inventory[i].Name == name)
            {
                if (inventory[i].Amount < inventory[i].GetItemData().maxStack)
                {
                    int excess = amount;
                    if ((excess = inventory[i].ChangeAmount(excess)) != 0)
                    {
                        AddItem(name, excess);
                    }
                    result = true;
                    break;
                }
            }
            else if (inventory[i].Name == "")
            {
                GameObject tmp = inventory[i].slot;
                inventory[i] = new Item(name);
                inventory[i].slot = tmp;

                if (amount < inventory[i].GetItemData().maxStack)
                    inventory[i].SetAmount(amount);
                else
                {
                    inventory[i].SetAmount(inventory[i].GetItemData().maxStack);
                    AddItem(name, amount - inventory[i].GetItemData().maxStack);
                }
                result = true;
                break;
            }
        }
        updateGUI();

        if (result)
            RiseInfoPrefab("You've got " + amount + " " + name + "!");
        else
            RiseInfoPrefab("Inventory is full!");
        return result;
    }

    public void RemoveItem(int index)
    {
        GameObject tmpSlot = inventory[index].slot;
        inventory[index] = new Item();
        inventory[index].slot = tmpSlot;
        updateGUI();
    }

    public bool RemoveItem(string name, int amount = 1)
    {
        List<int> toDel = new List<int>();
        for (int i = 0; i < INVSIZE; i++)
        {
            if (inventory[i].Name == name)
            {
                if (amount == inventory[i].Amount)
                {
                    RemoveItem(i);
                    amount = 0;
                    break;
                }
                if (amount > inventory[i].Amount)
                {
                    amount -= inventory[i].Amount;
                    toDel.Add(i);
                }
                else
                {
                    amount = inventory[i].ChangeAmount(-amount);
                    break;
                }
            }
        }

        if (amount == 0)
        {
            for (int j = 0; j < toDel.Count; j++)
               RemoveItem(toDel[j]);
            updateGUI();
            return true;
        }

        updateGUI();
        return false;
    }

    public void moveItem(int src, int dest)
    {
        if (dest == INVSIZE)    //trash
        {
            GetComponent<AudioSource>().PlayOneShot(trashSound);
            RemoveItem(src);
            updateGUI();
            return;
        }

        if (inventory[src].Name == inventory[dest].Name)    //merge
        {
            int excess = inventory[dest].ChangeAmount(inventory[src].Amount);
            if (excess != 0)
                inventory[src].SetAmount(excess);
            else
            {
                RemoveItem(src);
            }
        }
        else            //swap
        {
            Item temp = new Item(inventory[src]);

            inventory[src] = new Item(inventory[dest]);
            inventory[src].slot = temp.slot;

            temp.slot = inventory[dest].slot;
            inventory[dest] = temp;
        }
        updateGUI();
    }

    public int FindItem(string Name)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Name == Name)
            {
                return i;
            }
        }
        return -1;
    }

    public int CheckItemCount(string name)
    {
        int count = 0;

        for (int i = 0; i < INVSIZE; i++)
        {
            if (inventory[i].Name == name)
            {
                count += inventory[i].Amount;
            }
        }

        return count;
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="info">Text of information.</param>
    /// <param name="type">If true, then you create info prefab, else you create warning.</param>
    /// <param name="size">Size of prefab.</param>
    public void RiseInfoPrefab(string info, float size = 1) //rise info text
    {
        GameObject ship = GameObject.Find("SpaceShip");
        infos.Add(Instantiate(infoPrefab, new Vector3(ship.transform.position.x, ship.transform.position.y, ship.transform.position.z - 1), Quaternion.identity));
        infos[infos.Count - 1].transform.SetParent(GameObject.Find("Canvas").transform);
        infos[infos.Count - 1].text = info;
        infos[infos.Count - 1].transform.localScale = new Vector3(size, size, size);
        infos[infos.Count - 1].transform.eulerAngles = new Vector3(0, 0, 0);
        infos[infos.Count - 1].transform.localRotation = Quaternion.identity;

    }

    public void clearInventory()
    {
        for (int i = 0; i < INVSIZE; i++)
        {
            RemoveItem(i);
        }
    }

    void updateGUI()
    {
        for (int i = 0; i < INVSIZE; i++)
        {
            if (inventory[i].Name == "")
            {
                inventory[i].slot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("items/Empty");
                inventory[i].slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            }
            else
            {
                inventory[i].slot.transform.GetChild(0).GetComponent<Image>().sprite = inventory[i].GetItemData().sprite;
                inventory[i].slot.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inventory[i].Amount.ToString();
            }
        }
    }




    [ContextMenu("Add Water")]
    public void contextAddWater()
    {
        AddItem("Water", 5);
    }

    [ContextMenu("Add Alloy")]
    public void contextAddAlloy()
    {
        AddItem("Alloy", 5);
    }

    [ContextMenu("Add Gold")]
    public void contextAddGold()
    {
        AddItem("Gold", 5);
    }

    [ContextMenu("Add Helium")]
    public void contextAddHelium()
    {
        AddItem("Helium", 5);
    }

    [ContextMenu("Add Uranium")]
    public void contextAddUranium()
    {
        AddItem("Uranium", 5);
    }

    [ContextMenu("Add Ship part")]
    public void contextAddShippart()
    {
        AddItem("Ship part", 5);
    }

    [ContextMenu("Add Silicium")]
    public void contextAddSilicium()
    {
        AddItem("Silicium", 5);
    }

    [ContextMenu("Add Nuclear Fuel")]
    public void contextAddNuclearFuel()
    {
        AddItem("Nuclear Fuel", 5);
    }

    [ContextMenu("Add Food Can")]
    public void contextAddFoodCan()
    {
        AddItem("Canned food", 5);
    }

    [ContextMenu("Add Shield")]
    public void contextAddShield()
    {
        AddItem("Asteroid Shield", 5);
    }

}
