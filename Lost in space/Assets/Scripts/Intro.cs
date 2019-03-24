using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Sprite intro0;
    public Sprite intro1;
    public Sprite intro2;
    public Sprite intro3;

    public float introTime1;
    public float introTime2;
    public float introTime3;
    public float introTime4;

	// Use this for initialization
	void Start ()
    {
        Invoke("Intro0", 0);
        Invoke("Intro1", introTime1);
        Invoke("Intro2", introTime1 + introTime2);
        Invoke("Intro3", introTime1 + introTime2 + introTime3);
        Invoke("IntroEnd", introTime1 + introTime2 + introTime3 + introTime4);
	}

    void Intro0()
    {
        gameObject.GetComponent<Image>().sprite = intro0;
    }

    void Intro1()
    {
        gameObject.GetComponent<Image>().sprite = intro1;
    }

    void Intro2()
    {
        gameObject.GetComponent<Image>().sprite = intro2;
    }

    void Intro3()
    {
        gameObject.GetComponent<Image>().sprite = intro3;
    }

    void IntroEnd()
    {
        GameObject.Find("Intro Camera").SetActive(false);
        GameObject.Find("Menu Camera").GetComponent<Camera>().enabled = true;
    }
}
