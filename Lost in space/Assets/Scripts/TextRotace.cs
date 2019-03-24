using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotace : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation = Quaternion.identity;   //RotateAround(transform.position, Vector3.forward, -0.023f); // Rotation around its axis "Z" (point, axis, angle).
    }
}
