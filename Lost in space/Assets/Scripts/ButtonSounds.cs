using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public AudioClip onHighlight;
    public AudioClip onClick;

    AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        audioSource = GameObject.Find("Menu Camera").GetComponent<AudioSource>();

        onHighlight = Resources.Load<AudioClip>("Sounds/button highlight");
        onClick = Resources.Load<AudioClip>("Sounds/button click");
	}
	

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Button>().enabled)
            audioSource.PlayOneShot(onClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Button>().enabled)
            audioSource.PlayOneShot(onHighlight);
    }
}
