using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialLoading : MonoBehaviour
{
    public GameObject loadingBar;
    GameObject planet;
    GameObject ship;
    float speed;
    public float MaxTime;
    float currentAmount;

    public AudioClip scanStart;
    public AudioClip scanMid;
    public AudioClip scanEnd;

    private AudioSource audioSource;

    void Start()
    {
        ship = GameObject.Find("SpaceShip");
        audioSource = GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update ()
    {
        if (ship.GetComponent<GameController>().paused)
            return;

        if ((ship.transform.parent != null) && (ship.transform.parent.tag == "Planet" || ship.transform.parent.tag == "Living Planet"))
        {
            planet = ship.transform.parent.parent.gameObject;

            transform.parent.SetParent(planet.transform, false);
            transform.parent.GetComponent<RectTransform>().sizeDelta = planet.GetComponent<RectTransform>().sizeDelta * planet.transform.lossyScale;
            transform.parent.position = planet.transform.position;

            speed = 1;
            audioSource.pitch = 0.4f;
            if (planet.GetComponent<Planet>().scaningStation)
            {
                speed += 1;
                audioSource.pitch += 0.1f;
            }
            if (planet.GetComponent<Planet>().miningStation)
            {
                speed += 1;
                audioSource.pitch += 0.1f;
            }


            if (Input.GetKey("f") && !planet.GetComponent<Planet>().scaned)
            {
                if (currentAmount == 0)
                {
                    audioSource.clip = scanStart;
                    audioSource.Play();
                }


                gameObject.GetComponent<Image>().enabled = true;
                if (currentAmount < MaxTime)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = scanMid;
                        audioSource.Play();
                    }
                    currentAmount += speed * Time.deltaTime;
                }
                else        //done
                {
                    audioSource.Stop();
                    audioSource.clip = scanEnd;
                    audioSource.Play();
                    gameObject.GetComponent<Image>().enabled = false;
                    planet.GetComponent<Planet>().scaned = true;
                    planet.GetComponent<Planet>().ResourceGeneration();
                }

            }
            else
            {
                if (audioSource.clip != scanEnd)
                    audioSource.Stop();
                    
                currentAmount = 0;
            }

            loadingBar.GetComponent<Image>().fillAmount = currentAmount / MaxTime;
        }
        else
            gameObject.GetComponent<Image>().enabled = false;
    }
}
