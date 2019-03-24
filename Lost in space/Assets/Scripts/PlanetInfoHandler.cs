using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoHandler : MonoBehaviour
{
    GameObject planet;
    GameObject ship;
    GameObject scaninfo;

    // Use this for initialization
    void Start ()
    {
        ship = GameObject.Find("SpaceShip");
        scaninfo = GameObject.Find("PlanetInfoOff");
    }

    // Update is called once per frame
    void Update()
    {
        if ((ship.transform.parent != null) && (ship.transform.parent.tag == "Planet" || ship.transform.parent.tag == "Living Planet"))
        {
            scaninfo.SetActive(true);
            planet = ship.transform.parent.parent.gameObject;
        }
        else
        {
            if (planet != null)
                planet.GetComponent<Planet>().ScaningOff();
            scaninfo.SetActive(false);
        }
    }

    public void GetFrequentResource()
    {
        planet.GetComponent<Planet>().FrequentResourceGetting();
    }

    public void GetNormalResource()
    {
        planet.GetComponent<Planet>().NormalResourceGetting();
    }

    public void GetRareResource()
    {
        planet.GetComponent<Planet>().RareResourceGetting();
    }

    public void AddGasTool()
    {
        planet.GetComponent<Planet>().AddGAT();
    }
    public void AddFarm()
    {
        planet.GetComponent<Planet>().AddFarm();
    }

    public void AddHydratation()
    {
        planet.GetComponent<Planet>().AddHydratation();
    }

    public void AddMiningDrill()
    {
        planet.GetComponent<Planet>().AddMiningDrill();
    }

    public void AddScanStation()
    {
        planet.GetComponent<Planet>().AddScaningStation();
    }

    public void AddMiningStation()
    {
        planet.GetComponent<Planet>().AddMiningStation();
    }


}
