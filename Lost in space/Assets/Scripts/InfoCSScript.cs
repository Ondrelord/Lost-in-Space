using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCSScript : MonoBehaviour {

    GameObject craftingSlot;
    double speed = 0.01;
    bool clicked = false;
    int childID = 0;
    double distanceNow = 0;
    double distance = 0;
    int integer;
    int index = 0;


    // Use this for initialization
    void Start () {

        if (int.TryParse(gameObject.name[14].ToString(), out integer))
        {
            index += integer;
        }
        childID = index * 3;
        craftingSlot = GameObject.Find("CraftingPanel").transform.GetChild(childID).gameObject;
        distance = (transform.position.x - craftingSlot.transform.position.x) * 10;
    }

    // Update is called once per frame
    void Update () {
        if (clicked)
        {
            if (distanceNow < distance)
            {
                transform.position = new Vector3((float)(craftingSlot.transform.position.x + distanceNow), craftingSlot.transform.position.y, craftingSlot.transform.position.z);
                distanceNow += speed;
            }
        }
        else
        {
        }
	}

    public void OnClick()
    {
        if (clicked)
        {
            clicked = false;
        }
        else
        {
            clicked = true;
        }
    }
}
