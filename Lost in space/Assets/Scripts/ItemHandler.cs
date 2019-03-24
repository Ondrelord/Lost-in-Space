using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject startParent;
    private GameObject dragged;
    private GameObject ship;
    private int src = -1;
    private int dest = -1;
    private Inventory inv;
    private GameObject tooltip;

    private AudioSource audioSource;
    private AudioClip fuelUse;
    private AudioClip foodCanUse;
    private AudioClip shieldUse;
    private AudioClip dragSound;
    private AudioClip dropSound;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>(); //getting inventory
        ship = GameObject.Find("SpaceShip");
        tooltip = GameObject.Find("Canvas").transform.Find("Tooltip").gameObject;

        audioSource = inv.GetComponent<AudioSource>();
        fuelUse = Resources.Load<AudioClip>("Sounds/fuel use");
        foodCanUse = Resources.Load<AudioClip>("Sounds/food can use");
        shieldUse = Resources.Load<AudioClip>("Sounds/shield use");
        dragSound = Resources.Load<AudioClip>("Sounds/drag sound");
        dropSound = Resources.Load<AudioClip>("Sounds/drop sound");
    }

    //event when start draging Item from slot
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent.GetSiblingIndex() == inv.INVSIZE)
            return;

        if (inv.inventory[transform.parent.GetSiblingIndex()].Name == "")
            return;

        audioSource.PlayOneShot(dragSound);
        startParent = transform.parent.gameObject;                                      //original slot of Item
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;                  //dragged object is ray transparent
        gameObject.transform.SetParent(GameObject.Find("Dragged Item").transform);      //changing parent so item will be on top (visible)
        
    }

    //during drag of item
    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.GetSiblingIndex() == inv.INVSIZE)
            return;
        
        if (startParent == null)
            return;

        transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);     //locking item to cursor position
    }

    //when stop draging
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.GetSiblingIndex() == inv.INVSIZE)
            return;

        if (startParent == null)
            return;

        audioSource.PlayOneShot(dropSound);
        transform.SetParent(startParent.transform);                     //set parent slot to original slot (if moved its set to destination parent)
        transform.localPosition = Vector2.zero;                         //swapping image - item icon is set to center position (of a slot)
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;   //returning item ray transparency
        if (src != dest)
            inv.moveItem(src, dest);
        src = -1;
        dest = -1;

        startParent = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        dragged = GameObject.Find("Dragged Item").transform.GetChild(0).gameObject;
        dragged.GetComponent<ItemHandler>().src = dragged.GetComponent<ItemHandler>().startParent.transform.GetSiblingIndex();
        dragged.GetComponent<ItemHandler>().dest = transform.parent.GetSiblingIndex();
    }

    public void OnClick()
    {
        switch (inv.inventory[transform.parent.GetSiblingIndex()].Name)
        {
            case ("Helium"):
                if (ship.GetComponent<GameController>().fuelCount < ship.GetComponent<GameController>().maxFuelCount)
                {
                    inv.GetComponent<Inventory>().RemoveItem("Helium");
                    ship.GetComponent<GameController>().AddFuel(inv.database.GetItemData("Helium").stats["fuel"]);

                    audioSource.PlayOneShot(fuelUse);
                }
                break;
            case ("Nuclear Fuel"):
                if (ship.GetComponent<GameController>().fuelCount < ship.GetComponent<GameController>().maxFuelCount)
                {
                    inv.GetComponent<Inventory>().RemoveItem("Nuclear Fuel");
                    ship.GetComponent<GameController>().AddFuel(inv.database.GetItemData("Nuclear Fuel").stats["fuel"]);

                    audioSource.PlayOneShot(fuelUse);
                }
                break;
            case ("Canned food"):
                if (ship.GetComponent<GameController>().foodCount < ship.GetComponent<GameController>().maxFoodCount)
                {
                    inv.GetComponent<Inventory>().RemoveItem("Canned food");
                    ship.GetComponent<GameController>().foodCount += inv.database.GetItemData("Canned food").stats["food"];

                    audioSource.PlayOneShot(foodCanUse);
                }
                break;
            case ("Asteroid Shield"):
                inv.GetComponent<Inventory>().RemoveItem("Asteroid Shield");
                ship.GetComponent<GameController>().RunAsteroidShield(inv.database.GetItemData("Asteroid Shield").stats["time"]);

                audioSource.PlayOneShot(shieldUse);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.parent.GetSiblingIndex() == inv.INVSIZE)
            return;

        Item item = inv.inventory[transform.parent.GetSiblingIndex()];

        if (item.Name == "")
            return;

        tooltip.GetComponent<TooltipHandler>().showTooltip(transform, item.Name, item.GetItemData().Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.GetComponent<TooltipHandler>().hideTooltip();
    }
}
