using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [Header("System generation settings")]
    public bool addFoodPlanet;
    public bool addNuclearFuel;
    public bool addMetal;

    [Header("Prefabs of the planets")]
    public Planet defaultPlanet;
    public Planet moltenPlanet;
    public Planet twinEarthPlanet;
    public Planet gasGiantPlanet;
    public Planet frozenPlanet;
    public Planet tombPlanet;

    private Vector3 randomStartPosition;
    private int requestedPlanets;
    private int randomNumberOfPlanets;

    // Use this for initialization
    void Start ()
    {
        requestedPlanets = 0;

        if (addFoodPlanet)
        {
            requestedPlanets++;
        }
        if (addNuclearFuel)
        {
            requestedPlanets++;
        }
        if (addMetal)
        {
            requestedPlanets++;
        }

        if (requestedPlanets > 4)
        {
            randomNumberOfPlanets = Random.Range(requestedPlanets, 11);
        }
        else
        {
            randomNumberOfPlanets = Random.Range(4, 11);
        }

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
        else if (rangeFromSun >= 20 && rangeFromSun < 45)
        {
            targetPlanet.type = Planet.planetTypes.Twin_Earth;
            
        }
        else if (rangeFromSun >= 45 && rangeFromSun < 65)
        {
            targetPlanet.type = Planet.planetTypes.Tomb;
        }
        else if (rangeFromSun >= 65 && rangeFromSun < 80)
        {
            targetPlanet.type = Planet.planetTypes.Gas_Giant;
        }
        else if (rangeFromSun >= 80)
        {
            targetPlanet.type = Planet.planetTypes.Frozen;
        }
    }

    public void SetResourcesOfPlanet(Planet targetPlanet, Planet.planetTypes planetType)
    {
        int randomFrequentResourceIndex;
        int randomNormalResourceIndex;
        int randomRareResourceIndex;

        switch (planetType)
        {
            case Planet.planetTypes.Molten:

                if (addMetal)
                {
                    randomFrequentResourceIndex = 2;
                    SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);
                    addMetal = false;
                }
                else
                {
                    randomFrequentResourceIndex = Random.Range(2, 4);
                    SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);
                }

                randomNormalResourceIndex = Random.Range(2, 5);
                SetNormalResourceType(randomNormalResourceIndex, targetPlanet);

                randomRareResourceIndex = Random.Range(4, 6);
                SetRareResourceType(randomRareResourceIndex, targetPlanet);

                break;

            case Planet.planetTypes.Twin_Earth:

                randomFrequentResourceIndex = Random.Range(7, 8);
                SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);

                randomNormalResourceIndex = Random.Range(2, 6);
                SetNormalResourceType(randomNormalResourceIndex, targetPlanet);

                randomRareResourceIndex = Random.Range(4, 6);
                SetRareResourceType(randomRareResourceIndex, targetPlanet);

                break;

            case Planet.planetTypes.Gas_Giant:

                randomFrequentResourceIndex = Random.Range(1, 2);
                SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);

                randomNormalResourceIndex = Random.Range(1, 2);
                SetNormalResourceType(randomNormalResourceIndex, targetPlanet);

                randomRareResourceIndex = Random.Range(1, 2);
                SetRareResourceType(randomRareResourceIndex, targetPlanet);

                break;

            case Planet.planetTypes.Frozen:

                randomFrequentResourceIndex = Random.Range(7, 8);
                SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);

                randomNormalResourceIndex = Random.Range(1, 3);
                SetNormalResourceType(randomNormalResourceIndex, targetPlanet);

                randomRareResourceIndex = Random.Range(3, 6);
                SetRareResourceType(randomRareResourceIndex, targetPlanet);

                break;

            case Planet.planetTypes.Tomb:

                randomFrequentResourceIndex = Random.Range(5, 6);
                SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);

                randomNormalResourceIndex = Random.Range(1, 7);
                SetNormalResourceType(randomNormalResourceIndex, targetPlanet);

                randomRareResourceIndex = Random.Range(6, 7);
                SetRareResourceType(randomRareResourceIndex, targetPlanet);

                break;

            default:

                randomFrequentResourceIndex = Random.Range(1, 7);
                SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);

                randomNormalResourceIndex = Random.Range(1, 7);
                SetNormalResourceType(randomNormalResourceIndex, targetPlanet);

                randomRareResourceIndex = Random.Range(1, 7);
                SetRareResourceType(randomRareResourceIndex, targetPlanet);

                break;
        }
    }

    public void PlacePlanet(Planet planetPrefab)
    {
        switch (planetPrefab.type)
        {
            case Planet.planetTypes.Molten:
                SetResourcesOfPlanet(moltenPlanet, Planet.planetTypes.Molten);
                moltenPlanet.type = Planet.planetTypes.Molten;
                Instantiate(moltenPlanet, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.Twin_Earth:
                SetResourcesOfPlanet(twinEarthPlanet, Planet.planetTypes.Twin_Earth);
                twinEarthPlanet.type = Planet.planetTypes.Twin_Earth;
                Instantiate(twinEarthPlanet, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.Gas_Giant:
                SetResourcesOfPlanet(gasGiantPlanet, Planet.planetTypes.Gas_Giant);
                gasGiantPlanet.type = Planet.planetTypes.Gas_Giant;
                Instantiate(gasGiantPlanet, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.Frozen:
                SetResourcesOfPlanet(frozenPlanet, Planet.planetTypes.Frozen);
                frozenPlanet.type = Planet.planetTypes.Frozen;
                Instantiate(frozenPlanet, randomStartPosition, Quaternion.identity);
                break;
            case Planet.planetTypes.Tomb:
                SetResourcesOfPlanet(tombPlanet, Planet.planetTypes.Tomb);
                tombPlanet.type = Planet.planetTypes.Tomb;
                Instantiate(tombPlanet, randomStartPosition, Quaternion.identity);
                break;
        }
    }

    public void CreatePlanet (Planet planet)
    {
        if (addFoodPlanet)
        {
            randomStartPosition = new Vector3(Random.Range(20.0f, 30.0f), Random.Range(20.0f, 30.0f), 0);
            addFoodPlanet = false;
        }
        else if (addMetal)
        {
            randomStartPosition = new Vector3(Random.Range(5.0f, 10.0f), Random.Range(5.0f, 15.0f), 0);
        }
        else if (addNuclearFuel)
        {
            randomStartPosition = new Vector3(Random.Range(45.0f, 50.0f), Random.Range(45.0f, 50.0f), 0);
            addNuclearFuel = false;
        }
        else
        {
            randomStartPosition = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), 0);
        }

        float rangeFromSun = Vector2.Distance(randomStartPosition, new Vector3(0, 0, 0));
        SetPlanetType(rangeFromSun, planet);
        PlacePlanet(planet);
    }
}