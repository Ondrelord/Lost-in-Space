using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject ship;
    Camera mainCamera;
    private float zoomSpeed, maxZoomOut, maxZoomIn, zoom;


    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        ship = GameObject.Find("SpaceShip");
        zoomSpeed = 4f;
        maxZoomIn = 1f;
        maxZoomOut = 8f;
    }


    // Update is called once per frame
    void Update()
    {
        zoom = (Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed);
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoom, maxZoomIn, maxZoomOut);
    }

    public float GetZoom()
    {
        return mainCamera.orthographicSize;
    }
}