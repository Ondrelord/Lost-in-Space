using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
	// Update is called once per frame
	void FixedUpdate ()
    {
        /*Debug.Log("pos " + transform.position.x);
        if (transform.position == GameObject.Find("Arrow").transform.position)
        {
            return;
        }*/
        transform.RotateAround(transform.position, Vector3.forward, 0.3f); // Rotation around its axis "Z" (point, axis, angle).
    }
}
