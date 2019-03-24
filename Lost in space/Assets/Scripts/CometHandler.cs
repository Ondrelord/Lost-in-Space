using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CometHandler : MonoBehaviour
{
    Comet c;
    GameObject player;
    int count = 5;
    int period = 10;  //for Comet
    int period2 = 0;  //for Asteroids
    int period3 = 40; //for asteroid count
    double lasttime = 0;  //timingforComet
    double lasttime2 = 0;  //timingforKillingAsteroids
    double lasttime3 = 0;  //timingforStoppedAsteroids
    double lasttime4 = 0;  //for asteroid fall
    bool timeDefined = false;
    bool timeDefined2 = false;
    public bool asteroidFallOn = false;
    List<Asteroid> killingAsteroids = new List<Asteroid>();
    List<Asteroid> stoppedAsteroids = new List<Asteroid>();
    List<Asteroid> asteroidFall = new List<Asteroid>();
    public GameObject KillingAsteroid;
    public GameObject UraniumAsteroid;
    public GameObject ShipPartAsteroid;
    public GameObject MineAsteroid;
    float probabilityOfKiling = 0.4f; //if == 1, then all asteroids are killing

    // Use this for initialization
    void Start()
    {
        c = new Comet();
        player = GameObject.Find("SpaceShip");
        AddAsteroids();
    }

    public void InitComet()
    {
        c.CreateNew();
    }

    public void InitAsteroids()
    {
        lasttime = 0;  //timingforComet
        lasttime2 = 0;  //timingforKillingAsteroids
        lasttime3 = 0;  //timingforStoppedAsteroids
        timeDefined = false;
        timeDefined2 = false;

        for (int i = killingAsteroids.Count - 1; i >= 0; i--)
        {
            Destroy(killingAsteroids[i].gameObject);
            killingAsteroids.RemoveAt(i);
        }

        /*while (GameObject.Find("KillingAsteroidPrefab(Clone)") != null)
        {
            Destroy(GameObject.Find("KillingAsteroidPrefab(Clone)"));
        }*/

        AddAsteroids();
    }

    private void AddAsteroids(bool k = true)
    {
        if (k)
        {
            for (int i = 0; i < count; i++)
            {
                killingAsteroids.Add(CreateAsteroid());
            }
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                asteroidFall.Add(CreateAsteroid());
            }
        }
    }

    void MoreAsteroids()
    {
        if (lasttime4 + period3 < player.GetComponent<GameController>().playtime)
        {
            if (count < 20)
            {
                count += 1;
                killingAsteroids.Add(CreateAsteroid());
            }
            lasttime4 = player.GetComponent<GameController>().playtime;

            //Change probability of killing asteroid each 20seconds.
            if (probabilityOfKiling == 0.4f)
            {
                probabilityOfKiling = 0.75f;
            }
            else
            {
                probabilityOfKiling = 0.4f;
            }
        }
    }

    void AsteroidFall()
    {
        if (lasttime4 + period3 < player.GetComponent<GameController>().playtime)
        {
            foreach(Asteroid a in asteroidFall)
            {
                //a.CreateNew(true, true);
            }
            lasttime4 = player.GetComponent<GameController>().playtime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Have a fun with asteroids.
        //AsteroidFall();

        if (asteroidFall.Count > 0)
        {
            foreach (Asteroid a in asteroidFall)
            {
                if (!a.flowEnd)
                {
                    a.Move();
                    //a.HitTheShip();
                }

                if (player.GetComponent<GameController>().playtime > period3)
                {
                    for (int i = asteroidFall.Count - 1; i >= 0; i--)
                    {
                        Destroy(asteroidFall[i]);
                    }
                    asteroidFall.Clear();
                }
            }
        }

        //Manage comet trajectory
        if (!c.flowEnd)
        {
            c.Move();
            c.HitTheShip();
        }
        //If comet finished, then create new...
        if (c.flowEnd && !timeDefined)
        {
            lasttime = player.GetComponent<GameController>().playtime;

            timeDefined = true;
        }

        //Use period of comet creating.

        if (timeDefined && (player.GetComponent<GameController>().playtime - lasttime) > period)
        {
            c.CreateNew();
            timeDefined = false;
        }

        //This is for mining asteroid after flow of asteroid ends.
        //Asteroid is after some time destroyed.
        List<int> removedPositions = new List<int>();
        bool lasttimeActualization = false;

        for (int j = 0; j < stoppedAsteroids.Count; j++)
        {
            if (player.GetComponent<GameController>().playtime >= lasttime3 + 1)
            {
                stoppedAsteroids[j].timeOfDestroying -= 1;
                lasttimeActualization = true;
            }

            stoppedAsteroids[j].HitTheShip();
            if (stoppedAsteroids[j].mined)
            {
                removedPositions.Add(j);
            }


            if (stoppedAsteroids[j].timeOfDestroying <= 0)
            {
                removedPositions.Add(j);
            }
        }

        if (lasttimeActualization)
        {
            lasttime3 = player.GetComponent<GameController>().playtime;
        }

        for (int i = removedPositions.Count - 1; i >=0 ; i--)
        {
            Destroy(stoppedAsteroids[i].gameObject);
            stoppedAsteroids.RemoveAt(i);
        }

        //For all not stopped asteroids in the space.
        for (int i = 0; i < killingAsteroids.Count; i++)
        {
             //When asteroids move on.
            if (!killingAsteroids[i].flowEnd)
            {
                killingAsteroids[i].Move();
                killingAsteroids[i].HitTheShip();
            }
            else
            {    //When asteroid flow is finished, then define time of finishing.
                if (killingAsteroids[i].flowEnd && !timeDefined2)
                {
                    lasttime2 = player.GetComponent<GameController>().playtime;
                    timeDefined2 = true;
                }
                else
                {
                    //Use period for creating asteroids.
                    if (timeDefined2 && (player.GetComponent<GameController>().playtime - lasttime2) >= period2) //peroid2 nefunguje.. je stále = 0
                    {
                        if (!killingAsteroids[i].mined && !killingAsteroids[i].isKilling)
                        {
                            stoppedAsteroids.Add(killingAsteroids[i]);
                        }
                        else
                        {
                            if (killingAsteroids[i].mined && !killingAsteroids[i].isKilling)
                            {
                                Destroy(killingAsteroids[i].gameObject);
                            }
                        }
                        killingAsteroids[i] = CreateAsteroid();
                        timeDefined2 = false;
                    }
                }
            }
        }
        MoreAsteroids();
    }

    //Create new asteroid. If probability of killing == 1, then all asteroids are killing.
    private Asteroid CreateAsteroid(bool isFall = false, bool k = false)
    {
        GameObject asteroid;
        if (k)
        {
            asteroid = Instantiate(KillingAsteroid, new Vector3(0, 0, 0), Quaternion.identity);
            //asteroid.GetComponent<Asteroid>().CreateNew(true, isFall);
            return asteroid.GetComponent<Asteroid>();
        }
        else
        {
            if ((Random.Range(0, 100) / 100.0f) < probabilityOfKiling)
            {
                asteroid = Instantiate(KillingAsteroid, new Vector3(0, 0, 0), Quaternion.identity);
                asteroid.GetComponent<Asteroid>().CreateNew(true);
                return asteroid.GetComponent<Asteroid>();
            }
            else
            {
                if (Random.Range(0, 100) < 20)
                {
                    if (Random.Range(0, 2) == 0) //uranium
                    {
                        asteroid = Instantiate(UraniumAsteroid, new Vector3(0, 0, 0), Quaternion.identity);
                        asteroid.GetComponent<Asteroid>().CreateNew(false, 5);
                    }
                    else                         //shippart
                    {
                        asteroid = Instantiate(ShipPartAsteroid, new Vector3(0, 0, 0), Quaternion.identity);
                        asteroid.GetComponent<Asteroid>().CreateNew(false, 6);
                    }
                }
                else    //basic asteroid
                {
                    asteroid = Instantiate(MineAsteroid, new Vector3(0, 0, 0), Quaternion.identity);
                    asteroid.GetComponent<Asteroid>().CreateNew(false);
                }
                return asteroid.GetComponent<Asteroid>();
            }
        }
    }
}

public class Comet
{
    double distance = 1.5;
    double positionX;
    double positionY;
    Direction direction;
    GameObject player;
    GameObject comet;
    double timeOfCreation;
    public bool flowEnd;
    Item material;
    int speed;


    public Comet()
    {
        CreateNew();
    }

    public void CreateNew() //Create new comet behind the view of player. 
    {
        flowEnd = false;
        speed = Random.Range(320, 380);     //bigger = slower comet
        int amount = Random.Range(1, 6);
        material = new Item("Water", amount);
        player = GameObject.Find("SpaceShip");
        comet = GameObject.Find("Comet");
        comet.GetComponent<Renderer>().enabled = true;
        if (!GameObject.Find("SpaceShip").GetComponent<GameController>().gameIsRunning)
        {
            comet.transform.position = new Vector3(43, 15, 0);
        }
        else
        {
            comet.transform.position = Environment.GetRandomPositions(2);
        }
        direction = Environment.GenerateDirection(comet, player, 1500);
        timeOfCreation = player.GetComponent<GameController>().playtime;
    }

    public void Move() //Move towards ship.
    {
        comet.transform.Translate((float)direction.directionX / speed, (float)direction.directionY / speed, 0.0f);

        if (Environment.CheckEndOfView(player, comet, timeOfCreation, 5))
        {
            flowEnd = true;
            comet.GetComponent<Renderer>().enabled = false;
        }
    }

    public void HitTheShip()
    {
        if (Input.GetKeyUp("f"))
        {
            if (Environment.GetInteraction(player, comet, distance, comet.transform.localScale.x) && !flowEnd)
            {
                if (GameObject.Find("Inventory").GetComponent<Inventory>().AddItem(material.Name, material.Amount))
                {
                    AudioClip pickupSound = Resources.Load<AudioClip>("Sounds/comet pickup");
                    comet.GetComponent<AudioSource>().PlayOneShot(pickupSound);

                    comet.GetComponent<Renderer>().enabled = false;
                    flowEnd = true;
                }
               
            }
        }
    }
}

public static class Environment
{
    public static bool CheckEndOfView(GameObject player, GameObject asteroidy, double timeOfCreation, int elongation) //Check, if object is behind the player view or not.
    {
        if (player.GetComponent<GameController>().playtime > timeOfCreation + elongation)
        {
            GameObject camera = GameObject.Find("Main Camera");
            double distance = Mathf.Sqrt(Mathf.Pow((float)(camera.GetComponent<Camera>().orthographicSize * 2.4), 2) + Mathf.Pow(camera.GetComponent<Camera>().orthographicSize, 2));
            if (!Environment.GetInteraction(player, asteroidy, distance))
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetInteraction(GameObject g1, GameObject g2, double distance) //Return true, if two objects are in given distance.
    {
        double a = (g1.transform.position.x - g2.transform.position.x) * (g1.transform.position.x - g2.transform.position.x); //Pythagoras
        double b = (g1.transform.position.y - g2.transform.position.y) * (g1.transform.position.y - g2.transform.position.y);
        double c = Mathf.Sqrt((float)(a + b));

        if (c <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool GetInteraction(GameObject g1, GameObject g2, double distance, double size) //Return true, if two objects are in given distance (+ size of object).
    {
        double a = (g1.transform.position.x - g2.transform.position.x) * (g1.transform.position.x - g2.transform.position.x); //Pythagoras
        double b = (g1.transform.position.y - g2.transform.position.y) * (g1.transform.position.y - g2.transform.position.y);
        double c = Mathf.Sqrt((float)(a + b));

        if (c <= distance + size)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Direction GenerateDirection(GameObject g1, GameObject g2)     //Get direction FROM first gameObject towards second gameObject
    {
        double directionX = -(g1.transform.position.x - g2.transform.position.x); //Pythagoras
        double directionY = -(g1.transform.position.y - g2.transform.position.y);
        return new Direction(directionX, directionY);
    }

    public static Direction GenerateDirection(GameObject g1, GameObject g2, float distance)     //Get direction FROM first gameObject towards second gameObject (his position changed by distance)
    {
        double distanceX = Random.Range(-distance, distance) / 100.0f;
        double distanceY = Random.Range(-distance, distance) / 100.0f;
        double directionX = -(g1.transform.position.x - g2.transform.position.x + distanceX); //Pythagoras
        double directionY = -(g1.transform.position.y - g2.transform.position.y + distanceY);
        return new Direction(directionX, directionY);
    }

    /// <summary>
    /// Generating random positions behind all borders of player view. 
    /// </summary>
    /// <param name="distance">Distance behind the scene.</param>
    /// <returns></returns>
    public static Vector3 GetRandomPositions(int distance, bool asteroidFall = false)
    {
        double positionX; double positionY;
        double sizeX = 1; double sizeY = 1;
        double rotation = GameObject.Find("SpaceShip").transform.eulerAngles.z;

        if (rotation < 90)
        {
            sizeX += 1.4 * ((90 - rotation) / 90); // 2.4 -> 0
            sizeY += 1.4 * ((rotation) / 90);      // 0 -> 2.4
        }
        else
        {
            if (rotation < 180)
            {
                rotation -= 90;
                sizeX += 1.4 * ((rotation) / 90);
                sizeY += 1.4 * ((90 - rotation) / 90);
            }
            else
            {
                if (rotation < 270)
                {
                    rotation -= 180;
                    sizeX += 1.4 * ((90 - rotation) / 90);
                    sizeY += 1.4 * ((rotation) / 90);
                }
                else
                {
                    rotation -= 270;
                    sizeX += 1.4 * ((rotation) / 90);
                    sizeY += 1.4 * ((90 - rotation) / 90);
                }
            }
        }


        GameObject camera = GameObject.Find("Main Camera");
        if (Random.Range(0, 2) == 1)   //generate on Y axis
        {
            if (Random.Range(0, 2) == 0 || asteroidFall)
            {
                positionX = camera.transform.position.x + distance + camera.GetComponent<Camera>().orthographicSize * sizeX;
                positionY = camera.transform.position.y + Random.Range(-camera.GetComponent<Camera>().orthographicSize * 100, camera.GetComponent<Camera>().orthographicSize * 100) / 100 * sizeY;
            }
            else
            {
                positionX = camera.transform.position.x - distance - camera.GetComponent<Camera>().orthographicSize * sizeX;
                positionY = camera.transform.position.y + Random.Range(-camera.GetComponent<Camera>().orthographicSize * 100, camera.GetComponent<Camera>().orthographicSize * 100) / 100 * sizeY;
            }
        }
        else    //generate on X axis
        {
            if (Random.Range(0, 2) == 1 || asteroidFall)
            {
                positionY = camera.transform.position.y + distance + camera.GetComponent<Camera>().orthographicSize * sizeY;
                positionX = camera.transform.position.x + Random.Range(-camera.GetComponent<Camera>().orthographicSize * 100, camera.GetComponent<Camera>().orthographicSize * 100) / 100 * sizeX;
            }
            else
            {
                positionY = camera.transform.position.y - distance - camera.GetComponent<Camera>().orthographicSize * sizeY;
                positionX = camera.transform.position.x + Random.Range(-camera.GetComponent<Camera>().orthographicSize * 100, camera.GetComponent<Camera>().orthographicSize * 100) / 100 * sizeX;
            }
        }

        return new Vector3((float)positionX, (float)positionY, 0.0f);
    }
}

public class Direction
{
    public double directionX;
    public double directionY;

    public Direction(double x, double y)
    {
        directionX = x;
        directionY = y;
    }
}