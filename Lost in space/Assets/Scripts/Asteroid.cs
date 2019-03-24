using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    double distance = 0.25;
    double distanceForMining;
    double positionX;
    double positionY;
    GameObject player;
    Direction direction;
    int dangerbyHit = 12;
    int radiusforMineAsteroid = 500;
    public double timeOfCreation = 0;
    public int timeOfDestroying;
    Item material = null;
    public bool flowEnd;
    public bool mined = false;
    public bool isKilling;
    public bool isFall;
    int speed;

    public void CreateNew(bool isKilling2, int indexOfMaterial = 0, bool isFall2 = false)  //Create new asteroid behind the view of player.
    {
        flowEnd = false;
        isFall = isFall2;
        isKilling = isKilling2;
        timeOfDestroying = 30;
        player = GameObject.Find("SpaceShip");
        gameObject.transform.SetParent(GameObject.Find("Asteroids").transform);
        gameObject.transform.position = Environment.GetRandomPositions(1, isFall2);

        if (isKilling)  //Instantiate killing asteroid
        {
            direction = Environment.GenerateDirection(gameObject, player);
            speed = Random.Range(80, 160);
        }
        else            //Instantiate mining asteroid
        {
            distanceForMining = 1.5;
            material = GenerateMaterial(indexOfMaterial);
            speed = Random.Range(320, 400); //when is higher, then asteroid is slower
            direction = Environment.GenerateDirection(gameObject, player, radiusforMineAsteroid);
        }
        timeOfCreation = player.GetComponent<GameController>().playtime;
    }

    public void Move()  //Move and destroy ship.
    {

        gameObject.transform.Translate((float)direction.directionX / speed, (float)direction.directionY / speed, 0.0f);
        int elongation = 5; if (isKilling) { elongation = 20; }
        if (Environment.CheckEndOfView(player, gameObject, timeOfCreation, elongation)) //If dont hit the ship, then destroy after 20seconds.
        {
            flowEnd = true;
        }
    }

    public void HitTheShip()
    {
        if (isKilling)  //for killing Asteroids
        {
            if (Environment.GetInteraction(player, gameObject, distance, gameObject.transform.localScale.x) && !flowEnd)
            {
                float shipSpeed = player.GetComponent<Ship>().movementDirection;
                int rotation = GetRotationToShip(player, gameObject);

                player.GetComponent<GameController>().HitPlayer(ComputeDamage((float)rotation, ((float)speed / 100), (shipSpeed / 2.5f)));
                flowEnd = true;

            }
        }
        else   //for asteroids to mining
        {
            if (Environment.GetInteraction(player, gameObject, distance, gameObject.transform.localScale.x) && !flowEnd) //stop the asteroid
            {
                flowEnd = true;
            }
            if (Input.GetKeyUp("f"))
            {
                if (material != null)
                {
                    if (Environment.GetInteraction(player, gameObject, distanceForMining, gameObject.transform.localScale.x))
                    {
                        if(GameObject.Find("Inventory").GetComponent<Inventory>().AddItem(material.Name, material.Amount))
                        {
                            AudioClip pickupSound = Resources.Load<AudioClip>("Sounds/comet pickup");
                            player.GetComponent<AudioSource>().PlayOneShot(pickupSound);
                            flowEnd = true;
                            mined = true;
                        }
                    }
                }
            }
        }
    }

    public int ComputeDamage(float rotation, float normalizedAsteroidSpeed /*0.8-1.6*/, float normalizedShipSpeed /*-0.8-2*/)
    {
        float damage; int damageShipFactor;
        
        if (rotation < 90)
        {
            damage = normalizedAsteroidSpeed + (normalizedShipSpeed * ((90 - rotation) / 90));
        }
        else
        {
            if (rotation < 180)
            {
                rotation -= 90;
                damage = normalizedAsteroidSpeed - (normalizedShipSpeed * (rotation / 90));
            }
            else
            {
                if (rotation < 270)
                {
                    rotation -= 180;
                    damage = normalizedAsteroidSpeed - (normalizedShipSpeed * ((90 - rotation) / 90));
                }
                else
                {
                    rotation -= 270;
                    damage = normalizedAsteroidSpeed + (normalizedShipSpeed * (rotation / 90));
                }
            }
        }
        /*Debug.Log("damage" + damage);*/
        damage *= dangerbyHit;

        /*Debug.Log("speed" + normalizedAsteroidSpeed);
        Debug.Log("rotation" + rotation);
        Debug.Log("playerspeed" + normalizedShipSpeed);
        Debug.Log("damage" + (int)damage);*/

        return (int)damage;
    }

    public static int GetRotationToShip(GameObject player, GameObject comet)
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

        rotZ -= player.transform.eulerAngles.z;
        if (rotZ > 360)
        {
            rotZ -= 360;
        }
        else
        {
            if (rotZ < 0)
            {
                rotZ += 360;
            }
        }
        return (int)rotZ;
    }

    private Item GenerateMaterial(int indexer = 0) //Generating material for mining asteroids.
    {
        if (indexer == 0)
        {
            int rnd = Random.Range(0, 100);
            if (rnd < 37) //Basic2 materials
            {
                indexer = Random.Range(3, 5);
            }
            else //Basic1 materials
            {
                indexer = Random.Range(1, 3);
            }       
        }

        Item item = new Item(GameObject.Find("Inventory").GetComponent<ItemDatabase>().items[indexer].Name);
        item.SetAmount(1);
        return item;
    }
}

