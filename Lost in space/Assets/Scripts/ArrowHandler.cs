using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowHandler : MonoBehaviour {

    List<GameObject> objectsInGame;
    List<Color> colorsOfObjects;
    List<double> positions;
    GameObject player;
    GameObject arrow;
    int position = 0;
    int maxposition = 5;
    GameObject label;
    GameObject arrowS;
    GameObject minimapP;
    GameObject planetsMarker;
    bool start = true;
    bool minimap = false;
    bool minimapOn = false;

    AudioClip radarChangeSound;

    // Use this for initialization
    void Start () {

        arrow = GameObject.Find("Arrow_pointer");
        arrow.GetComponent<Renderer>().enabled = false;
        planetsMarker = GameObject.Find("PlanetsMarker");
        minimapP = GameObject.Find("Minimap Panel");
        player = GameObject.Find("SpaceShip");
        arrowS = GameObject.Find("ArrowAB");
        label = GameObject.Find("Label");
        positions = new List<double>();

        objectsInGame = new List<GameObject>();
        //normal
        objectsInGame.Add(GameObject.Find("None"));
        objectsInGame.Add(GameObject.Find("Comet"));
        objectsInGame.Add(GameObject.Find("Earth"));
        objectsInGame.Add(GameObject.Find("LavaPlanet"));

        //first level of navigation
        objectsInGame.Add(GameObject.Find("Dark-Planet"));
        objectsInGame.Add(GameObject.Find("Purple-Planet"));
        //second level of navigation
        objectsInGame.Add(GameObject.Find("Ice-Planet"));

        radarChangeSound = Resources.Load<AudioClip>("Sounds/radar change");

        colorsOfObjects = new List<Color>();
        colorsOfObjects.Add(new Color(255, 255, 255));
        colorsOfObjects.Add(new Color(0, 226, 226));
        colorsOfObjects.Add(new Color(0, 255, 0));
        colorsOfObjects.Add(new Color(200, 0, 0));
        colorsOfObjects.Add(new Color(226, 0, 163));
        colorsOfObjects.Add(new Color32(170, 150, 230, 255));
        colorsOfObjects.Add(new Color32(115, 231, 255, 255));

    }

    // Update is called once per frame
    void Update() {

        //Determination of the positions and color of the arrow
        if (position != 0 && !minimapOn)   //position None
        {
            positions = Environment2.GetRotationAndPositionOfArrow(player, objectsInGame[position]);
        }
        //Set color of the UI
        if (!minimapOn)
        {
            label.GetComponent<Text>().color = colorsOfObjects[position];
            arrowS.GetComponent<Image>().color = colorsOfObjects[position];
        }
        if (position != 0 && !minimapOn)   //position None
        {
            //Set color of the UI
            arrow.GetComponent<SpriteRenderer>().color = colorsOfObjects[position];

            //Change position of arrow
            arrow.transform.eulerAngles = new Vector3(0, 0, (float)positions[0]);
            arrow.transform.position = new Vector3(player.transform.position.x + (float)positions[1], player.transform.position.y + (float)positions[2], -1);
        }

        if (start)
        {
            minimapP.SetActive(false);
            start = false;
        }


        //If player click R, then objects changing.
        if (Input.GetKeyDown("r"))
        {
            player.GetComponent<AudioSource>().PlayOneShot(radarChangeSound);

            position++;
            if (position == objectsInGame.Count && minimap && minimapOn == false)
            {
                minimapOn = true;
                minimapP.SetActive(true);
                planetsMarker.SetActive(false);
            }
            else
            {
                if (position >= objectsInGame.Count || position == maxposition)
                {
                    position = 0;
                    minimapOn = false;
                    minimapP.SetActive(false);
                    planetsMarker.SetActive(true);
                }
            }
            if (position == 0 || minimapOn)
            {
                arrow.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                arrow.GetComponent<Renderer>().enabled = true;
            }


            if (!minimapOn)
            {
            GameObject.Find("PlanetsMarker").GetComponent<Dropdown>().value = position;
            }
        }


        //When focused object is in the player view.
        if (position != 0 && !minimap)
        {
            if (objectsInGame[position].GetComponent<Renderer>().isVisible)
            {
                arrow.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                arrow.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void UpgradeRadar()
    {
        if (maxposition == 5)
            maxposition = 7;

        else if (maxposition == 7)
        {
            //maxposition = 7;
            minimap = true;
        }
    }
}

public static class Environment2
{
    public static Direction GenerateDirection(GameObject g1, GameObject g2)     //Get direction FROM first gameObject towards second gameObject
    {
        double directionX = -(g1.transform.position.x - g2.transform.position.x); //Pythagoras
        double directionY = -(g1.transform.position.y - g2.transform.position.y);
        return new Direction(directionX, directionY);
    }

    public static bool CheckEndOfView(GameObject player, GameObject comet) //Check, if object is behind the player view or not.
    {
        GameObject camera = GameObject.Find("Main Camera");
        double distance = Mathf.Sqrt(Mathf.Pow((float)(camera.GetComponent<Camera>().orthographicSize * 2.4), 2) + Mathf.Pow(camera.GetComponent<Camera>().orthographicSize, 2));
        if (!Environment.GetInteraction(player, comet, distance))
        {
            return true;
        }
        return false;
    }

    public static List<double> GetRotationAndPositionOfArrow(GameObject player, GameObject comet)
    {
        double x = 0; double y = 0; double rotZ;

        Direction dir = Environment2.GenerateDirection(player, comet);

        //right up corner
        if (dir.directionX > 0 && dir.directionY > 0)
        {
            if (dir.directionX < dir.directionY)
            {
                rotZ = 360 - (1 - ((dir.directionY - dir.directionX) / dir.directionY)) * 45;
            }
            else
            {
                rotZ = 315 - ((dir.directionX - dir.directionY) / dir.directionX) * 45;
            }
        }
        else //right down corner = 270degrees
        {
            if (dir.directionX > 0 && dir.directionY < 0)
            {
                if (dir.directionX > Mathf.Abs((float)dir.directionY))
                {
                    rotZ = 270 - (1 - ((dir.directionX - Mathf.Abs((float)dir.directionY)) / dir.directionX)) * 45;
                }
                else
                {
                    rotZ = 225 - ((Mathf.Abs((float)dir.directionY) - dir.directionX) / Mathf.Abs((float)dir.directionY)) * 45;
                }
            }
            else //left down corner - rotation left = 90degrees
            {
                if (dir.directionX < 0 && dir.directionY < 0)
                {
                    if (Mathf.Abs((float)dir.directionX) < Mathf.Abs((float)dir.directionY))
                    {
                        rotZ = 180 - (1 - ((Mathf.Abs((float)dir.directionY) - Mathf.Abs((float)dir.directionX)) / Mathf.Abs((float)dir.directionY))) * 45;
                    }
                    else
                    {
                        rotZ = 135 - ((Mathf.Abs((float)dir.directionX) - Mathf.Abs((float)dir.directionY)) / Mathf.Abs((float)dir.directionX)) * 45;
                    }
                }
                else //left up corner
                {
                    if (Mathf.Abs((float)dir.directionX) > dir.directionY)
                    {
                        rotZ = 90 - (1 - ((Mathf.Abs((float)dir.directionX) - Mathf.Abs((float)dir.directionY)) / Mathf.Abs((float)dir.directionX))) * 45;
                    }
                    else
                    {
                        rotZ = 45 - ((Mathf.Abs((float)dir.directionY) - Mathf.Abs((float)dir.directionX)) / Mathf.Abs((float)dir.directionY)) * 45;
                    }
                }
            }
        }

        y = Mathf.Cos((float)(rotZ * 0.0174532925));
        x = Mathf.Sin((float)(rotZ * 0.0174532925)) * -1;
        return new List<double>(new double[] { rotZ, x, y });
    }
}