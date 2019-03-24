using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    GameObject WarningSunOpen;
    public Animator anim1;
    private bool sunDamage; // Determining whether the sun causes damage at the moment
    public double lasttime = -3;

    void Start()
    {
        WarningSunOpen = GameObject.Find("Warning sun open");
        WarningSunOpen.GetComponent<Renderer>().enabled = false;
        WarningSunOpen.GetComponent<AudioSource>().enabled = false;
        anim1 = WarningSunOpen.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        transform.RotateAround(transform.position, Vector3.forward, 0.05f); // Rotation around its axis "Z" (point, axis, angle).
        Dangerous_area(); // The sun does damage in some area around it
    }

    private void OnTriggerEnter2D(Collider2D collision)                          // Handling a collision
    {
        if (collision.gameObject.name == "SpaceShip")                            // If it was with the ship
        {
            sunDamage = true;                                                    // Then put the flag that the sun does damage
        }

        if (collision.tag == "Asteroid")                                         // If with an asteroid
        {
            collision.transform.position = transform.TransformPoint(-100, 0, 0); // Then teleport him somewhere far away
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // We process the termination of collision
    {
        if (collision.gameObject.name == "SpaceShip")  // If with the ship
        {
            sunDamage = false;                         // Then remove the flag that the sun does damage
        }
    }

    void Dangerous_area() // Function that deals damage to an object in the area of the collider of the sun
    {
        GameObject player = GameObject.Find("SpaceShip");           // Find a player
        GameObject sun = GameObject.Find("Sun - Core");             // And the sun
        GameController gc1 = player.GetComponent<GameController>(); // We connect GameController

        if (sunDamage)                                              // If there is a flag that the sun does damage
        {
            double distance = GetInteractionDistance(sun, player); // Then we calculate the distance between the sun and the player
            gc1.HitPlayer(7 / distance);                               // After that, we remove his HP, depending on the proximity to the sun
        }

        if (GetInteractionDistance(player, gameObject) < 18)
        {
            if (lasttime + 3 < player.GetComponent<GameController>().playtime) //rise warning info
            {
                WarningSunOpen.GetComponent<Renderer>().enabled = true;
                WarningSunOpen.GetComponent<AudioSource>().enabled = true;
                anim1.Play("Entry");
                lasttime = gc1.playtime;
            }
        }
        else
        {
            if (WarningSunOpen.GetComponent<Renderer>().enabled == true)
            {
                WarningSunOpen.GetComponent<Renderer>().enabled = false;
                WarningSunOpen.GetComponent<AudioSource>().enabled = false;
            }
        }
    }

    public static double GetInteractionDistance(GameObject g1, GameObject g2) // Returns the distance between objects
    {
        double a = (g1.transform.position.x - g2.transform.position.x) * (g1.transform.position.x - g2.transform.position.x);  //distance on X-axis
        double b = (g1.transform.position.y - g2.transform.position.y) * (g1.transform.position.y - g2.transform.position.y);  //distance on Y-axis
        double c = Mathf.Sqrt((float)(a + b)); //Pythagoras

        return c;
    }
}
