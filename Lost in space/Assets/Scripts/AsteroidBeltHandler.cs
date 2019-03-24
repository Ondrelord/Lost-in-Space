using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBeltHandler : MonoBehaviour
{
    private Object[] prefabs;
    GameObject belt;
    public int BELTSIZE;
    public int MinDistance;
    public int MaxDistance;

	// Use this for initialization
	void Start ()
    {
        prefabs = Resources.LoadAll("Prefabs/Asteroids");
        belt = GameObject.Find("Asteroid Belt");

        createBelt();
    }

    void createBelt()
    {
        //40-70
        float randDist;
        float randSpd;
        float randSize;
        int randPrefab;

        GameObject asteroid;

        for (int i = 0; i < BELTSIZE; i++)
        {
            randDist = Random.Range(MinDistance, MaxDistance);
            randSpd = Random.Range(0.01f, 0.3f);
            randSize = Random.Range(0.2f, 1f);
            randPrefab = Random.Range(0, prefabs.Length);


            asteroid = Instantiate(prefabs[randPrefab], new Vector2(randDist, 0), Random.rotation, belt.transform) as GameObject;
            asteroid.GetComponent<CircularMovement>().speed = randSpd;
            asteroid.GetComponent<Transform>().localScale = new Vector3(randSize, randSize, randSize);
        }

    }

}
