using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    private float powerForce = 2.0F; // Impact force on an object in Unity units

    void Update()
    {
        if (gameObject.GetComponent<Asteroid>().isKilling) //If hit the ship, then destroy after 20seconds.
        {
            if (Environment.CheckEndOfView(GameObject.Find("SpaceShip"), gameObject, gameObject.GetComponent<Asteroid>().timeOfCreation, 3))
            {
                gameObject.GetComponent<Asteroid>().flowEnd = true;
                if (gameObject.GetComponent<Asteroid>().isFall == false)
                {
                    Destroy(gameObject);
                }
            }
        }

        if (GameObject.Find("SpaceShip").GetComponent<GameController>().playtime > gameObject.GetComponent<Asteroid>().timeOfCreation + 30)
        {
            gameObject.GetComponent<Asteroid>().flowEnd = true;
            if (gameObject.GetComponent<Asteroid>().isFall == false)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Collision handling
    {
        if (collision.tag == "SafeZone") // If an asteroid collides with a safety zone
        {
            /* Calculate the rebound path */
            Vector3 direction = (this.transform.position - collision.transform.position);
            /* We act on the object giving impulse of the established force (multiplied by the direction) */
            gameObject.GetComponent<Rigidbody2D>().AddForce(direction * powerForce, ForceMode2D.Impulse);
        }

        if (collision.tag == "Player") // If an asteroid collides with a player
        {
            Vector3 direction = (this.transform.position - collision.transform.position);
            gameObject.GetComponent<Rigidbody2D>().AddForce(direction * powerForce, ForceMode2D.Impulse);
        }
    }
}
