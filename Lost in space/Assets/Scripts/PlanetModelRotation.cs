using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetModelRotation : MonoBehaviour
{
    public float speed;


	// Update is called once per frame
	void FixedUpdate()
    {
        transform.Rotate(Vector3.up, speed);
	}
}
