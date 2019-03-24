using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    float timeCounter;

    public GameObject cosmicBody;

    public float speed;
    float rangeFromSun;
    public bool randomStart;
    public float startPosition;
    float rand;

    GameObject sun;

    // Use this for initialization
    void Start ()
    {
        sun = GameObject.Find("Sun");
        rangeFromSun =  Vector2.Distance(cosmicBody.transform.position, sun.transform.position);

        if (randomStart)
            rand = Random.Range(0f, 2f * Mathf.PI);
        else
            rand = startPosition;
	}

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * speed;

        float x = Mathf.Cos(timeCounter + rand) * rangeFromSun;
        float y = Mathf.Sin(timeCounter + rand) * rangeFromSun;
        float z = 0;

        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(y, x)*Mathf.Rad2Deg +15);
    }
}
