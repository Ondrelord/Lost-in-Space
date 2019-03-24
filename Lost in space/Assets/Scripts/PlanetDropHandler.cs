using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//every slot have item (there are empty items)
public class PlanetDropHandler : MonoBehaviour, IDropHandler
{
    GameObject draggedItem, dropParent, dragParent;
    Inventory inv;

    void Start()
    {
       // inv = GameObject.Find("Inventory").GetComponent<Inventory>(); //getting inventory
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        draggedItem = GameObject.Find("Dragged Item").transform.GetChild(0).gameObject; //item that is dragged (currently in Dragged item parent)
        dropParent = transform.gameObject;                                      //planet on which is item dropped
        draggedItem.transform.SetParent(dropParent.transform);

        //dragParent.transform.GetChild(0).localPosition = Vector2.zero;          //swapping image - repositioning icon of dropped-on item
    }

    public void OnDrop2()
    {
        Debug.Log("drop2");
    }


}