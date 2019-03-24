using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpdate : MonoBehaviour {

    GameObject lesson1Panel;
    GameObject lesson2Panel;
	// Use this for initialization
	void Start () {
        lesson1Panel = GameObject.Find("Lesson1Panel");
        lesson2Panel = GameObject.Find("Lesson2Panel");
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void OnClickUpdate()
    {
        if (transform.GetChild(0).gameObject.GetComponent<Text>().text.ToString() == "Continue")
        {
            lesson1Panel.SetActive(false);
            lesson2Panel.SetActive(true);
        }
        if (transform.GetChild(0).gameObject.GetComponent<Text>().text.ToString() == "Ok, thanks")
        {
            lesson2Panel.SetActive(false);
            GameObject.Find("SpaceShip").GetComponent<GameController>().SetUpTutorial();
        }
    }
}
