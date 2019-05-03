using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public Planet planetPrefab;
    public Planet moltenPlanetPrefab;
    public Planet twinEarthPlanetPrefab;
    public Planet gasGiantPlanetPrefab;
    public Planet frozenPlanetPrefab;
    public Planet tombPlanetPrefab;

    private Vector3 randomStartPosition;

    // Use this for initialization
    void Start ()
    {
        int randomNumberOfPlanets = Random.Range(4, 10);

        for (int i = 0; i < randomNumberOfPlanets; i++)
        {
            Planet newPlanet = new Planet();
            CreatePlanet(newPlanet);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetFrequentResourceType(int resourceTypeIndex, Planet targetPlanet)
    {
        switch (resourceTypeIndex)
        {
            case 1:
                targetPlanet.frequentResource = Planet.resourceTypes.Helium;
                break;
            case 2:
                targetPlanet.frequentResource = Planet.resourceTypes.Iron;
                break;
            case 3:
                targetPlanet.frequentResource = Planet.resourceTypes.Silicium;
                break;
            case 4:
                targetPlanet.frequentResource = Planet.resourceTypes.Gold;
                break;
            case 5:
                targetPlanet.frequentResource = Planet.resourceTypes.Uranium;
                break;
            case 6:
                targetPlanet.frequentResource = Planet.resourceTypes.Nuclear_Fuel;
                break;
            case 7:
                targetPlanet.frequentResource = Planet.resourceTypes.Water;
                break;
        }
    }

    public void SetNormalResourceType(int resourceTypeIndex, Planet targetPlanet)
    {
        switch (resourceTypeIndex)
        {
            case 1:
                targetPlanet.normalResource = Planet.resourceTypes.Helium;
                break;
            case 2:
                targetPlanet.normalResource = Planet.resourceTypes.Iron;
                break;
            case 3:
                targetPlanet.normalResource = Planet.resourceTypes.Silicium;
                break;
            case 4:
                targetPlanet.normalResource = Planet.resourceTypes.Gold;
                break;
            case 5:
                targetPlanet.normalResource = Planet.resourceTypes.Uranium;
                break;
            case 6:
                targetPlanet.normalResource = Planet.resourceTypes.Nuclear_Fuel;
                break;
            case 7:
                targetPlanet.normalResource = Planet.resourceTypes.Water;
                break;
        }
    }

    public void SetRareResourceType(int resourceTypeIndex, Planet targetPlanet)
    {
        switch (resourceTypeIndex)
        {
            case 1:
                targetPlanet.rareResource = Planet.resourceTypes.Helium;
                break;
            case 2:
                targetPlanet.rareResource = Planet.resourceTypes.Iron;
                break;
            case 3:
                targetPlanet.rareResource = Planet.resourceTypes.Silicium;
                break;
            case 4:
                targetPlanet.rareResource = Planet.resourceTypes.Gold;
                break;
            case 5:
                targetPlanet.rareResource = Planet.resourceTypes.Uranium;
                break;
            case 6:
                targetPlanet.rareResource = Planet.resourceTypes.Nuclear_Fuel;
                break;
            case 7:
                targetPlanet.rareResource = Planet.resourceTypes.Water;
                break;
        }
    }

    public void SetPlanetType(float rangeFromSun, Planet targetPlanet)
    {
        if (rangeFromSun < 20)
        {
            targetPlanet.type = Planet.planetTypes.Molten;
        }
        else if (rangeFromSun >= 20 && rangeFromSun < 60)
        {
            targetPlanet.type = Planet.planetTypes.TwinEarth;
        }
        else if (rangeFromSun >= 60 && rangeFromSun < 80)
        {
            targetPlanet.type = Planet.planetTypes.GasGiant;
        }
        else if (rangeFromSun >= 80)
        {
            targetPlanet.type = Planet.planetTypes.Frozen;
        }
    }

    public void SetResourcesOfPlanet(Planet targetPlanet)
    {
        int randomFrequentResourceIndex = Random.Range(1, 7);
        SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);

        int randomNormalResourceIndex = Random.Range(1, 7);
        SetNormalResourceType(randomNormalResourceIndex, targetPlanet);

        int randomRareResourceIndex = Random.Range(1, 7);
        SetRareResourceType(randomRareResourceIndex, targetPlanet);
    }

    public void PlacePlanet(Planet planetPrefab)
    {
        switch (planetPrefab.type)
        {
            case Planet.planetTypes.Molten:
                SetResourcesOfPlanet(moltenPlanetPrefab);
                moltenPlanetPrefab.type = Planet.planetTypes.Molten;
                Instantiate(moltenPlanetPrefab, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.TwinEarth:
                SetResourcesOfPlanet(twinEarthPlanetPrefab);
                twinEarthPlanetPrefab.type = Planet.planetTypes.TwinEarth;
                Instantiate(twinEarthPlanetPrefab, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.GasGiant:
                SetResourcesOfPlanet(gasGiantPlanetPrefab);
                gasGiantPlanetPrefab.type = Planet.planetTypes.GasGiant;
                Instantiate(gasGiantPlanetPrefab, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.Frozen:
                SetResourcesOfPlanet(frozenPlanetPrefab);
                frozenPlanetPrefab.type = Planet.planetTypes.Frozen;
                Instantiate(frozenPlanetPrefab, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.Tomb:
                SetResourcesOfPlanet(tombPlanetPrefab);
                tombPlanetPrefab.type = Planet.planetTypes.Tomb;
                Instantiate(tombPlanetPrefab, randomStartPosition, Quaternion.identity);
                break;
        }
    }

    public void CreatePlanet (Planet planet)
    {
        randomStartPosition = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), 0);

        float rangeFromSun = Vector2.Distance(randomStartPosition, new Vector3(0, 0, 0));
        SetPlanetType(rangeFromSun, planet);
        PlacePlanet(planet);
    }
}