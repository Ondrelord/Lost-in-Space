using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    GameObject ship;
	// Use this for initialization
	void Start () {
        ship = GameObject.Find("SpaceShip");
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y, 0);
	}
}
