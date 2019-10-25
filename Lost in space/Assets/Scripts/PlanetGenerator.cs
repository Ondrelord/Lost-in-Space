using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Logic: 1) We determine how many planets we need and what type, 
//        2) Use a common planet pattern, generate a random place of the planet appearance (or use a fixed one when a certain type is required), 
//        3) Assign a planet type to the planet based on the distance from the sun, 
//        4) Choose the desired planet pattern 
//        5) Based on the type obtained we generate resources on it, 
//        6) We inform it of the location 
//        7) And place it in space.

public class PlanetGenerator : MonoBehaviour
{
    [Header("Sun generation settings")]
    public int minNumberOfSuns;
    public int maxNumberOfSuns;

    [Header("Planet generation settings")]
    public int minNumberOfPlanets;
    public int maxNumberOfPlanets;
    [Range(0.0f, 100.0f)]
    public int addMoltenPlanets;
    [Range(0.0f, 100.0f)]
    public int addEarthLikePlanets;
    [Range(0.0f, 100.0f)]
    public int addTombPlanets;
    [Range(0.0f, 100.0f)]
    public int addGasGiantPlanets;
    [Range(0.0f, 100.0f)]
    public int addFrozenPlanets;

    [Header("Asteroid generation settings")]
    public int minNumberOfAsteroids;
    public int maxNumberOfAsteroids;

    [Header("Resource generation settings")]
    public bool addHelium;
    public bool addIron;
    public bool addSilicium;
    public bool addGold;
    public bool addUranium;
    public bool addNuclearuel;
    public bool addWater;

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

        // We sum up and remember the number of requested planets.
        if (addEarthLikePlanets > 0)
        {
            requestedPlanets += addEarthLikePlanets;
        }
        if (addTombPlanets > 0)
        {
            requestedPlanets += addTombPlanets;
        }
        if (addMoltenPlanets > 0)
        {
            requestedPlanets += addMoltenPlanets;
        }
        if (addGasGiantPlanets > 0)
        {
            requestedPlanets += addGasGiantPlanets;
        }
        if (addFrozenPlanets > 0)
        {
            requestedPlanets += addFrozenPlanets;
        }

        // We determine the number of planets that we will generate. This is a random variable between the minimum and maximum values. 
        // At the same time, we can not generate fewer planets than was requested.
        if (requestedPlanets > minNumberOfPlanets)
        {
            randomNumberOfPlanets = Random.Range(requestedPlanets, maxNumberOfPlanets);
        }
        else
        {
            randomNumberOfPlanets = Random.Range(minNumberOfPlanets, maxNumberOfPlanets);
        }

        // Create a certain number of planets.
        for (int i = 0; i < randomNumberOfPlanets; i++)
        {
            Planet newPlanet = new Planet(); // Create an instance of the class "Planet" as a template.
            CreatePlanet(newPlanet);         // We apply to it a method that determines the final appearance and location of the planet.
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // A method that establishes a resource with a specified ID as a frequently encountered resource for the transmitted planet pattern.
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

    // A method that establishes a resource with a specified ID as a normally encountered resource for the transmitted planet pattern.
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

    // A method that establishes a resource with a specified ID as a rarely encountered resource for the transmitted planet pattern.
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

    // The method that determines the type of planet, based on its distance from the star.
    public void SetPlanetType(float rangeFromSun, Planet targetPlanet)
    {
        // We control the distance to the star, on the basis of this we determine the type of planet. 
        // If there are requirements for planets of different types in one zone, we satisfy them in turn. 
        // When assigning a planet type, we always reduce the number of requested planets of this type.
        if (rangeFromSun < 20)
        {
            targetPlanet.type = Planet.planetTypes.Molten;
            addMoltenPlanets--;
        }
        else if (rangeFromSun >= 20 && rangeFromSun < 65)
        {
            if (addTombPlanets > 0)
            {
                targetPlanet.type = Planet.planetTypes.Tomb;
                addTombPlanets--;
            }
            else
            {
                targetPlanet.type = Planet.planetTypes.Twin_Earth;
                addEarthLikePlanets--;
            }          
        }
        else if (rangeFromSun >= 65 && rangeFromSun < 80)
        {
            targetPlanet.type = Planet.planetTypes.Gas_Giant;
            addGasGiantPlanets--;
        }
        else if (rangeFromSun >= 80)
        {
            targetPlanet.type = Planet.planetTypes.Frozen;
            addFrozenPlanets--;
        }
    }

    // A method that randomly assigns resources to a selected planet based on its type.
    public void SetResourcesOfPlanet(Planet targetPlanet, Planet.planetTypes planetType)
    {
        // Each resource has its own ID, on each planet there are three cells for resources.
        int randomFrequentResourceIndex;
        int randomNormalResourceIndex;
        int randomRareResourceIndex;

        // Based on the type of planet...
        switch (planetType)
        {
            // For each cell, we define its resource, randomly generating a digit representing the ID of the resource type, 
            //    using as constraints the limits imposed by the planet type. Then we use a special method to assign it to the pattern of the planet.
            case Planet.planetTypes.Molten:

                if (addIron)
                {
                    randomFrequentResourceIndex = 2;
                    SetFrequentResourceType(randomFrequentResourceIndex, targetPlanet);
                    addIron = false;
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

    // The method of assigning resources to the planet and placing it in space.
    public void PlacePlanet(Planet planetPrefab)
    {
        // When choosing an action, we are based on the transmitted type of planet.
        switch (planetPrefab.type)
        {
            // Then we set the type of resources for a given planet, using not a general template, but a template of a specific type of planet.
            // Just in case, we additionally inform her of her type.
            // Directly place an instance of the planet pattern of the required type, in the place that was previously defined.
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

    // Main method for creating a planet. Randomly generates a starting position for it and calculates the distance to the sun. 
    // Then it calls methods that establish the type of planet and place it in space.
    public void CreatePlanet (Planet planet)
    {
        // If there are requirements to create a planet of a certain type, 
        //    then as coordinates we use the coordinates of the zone in which it may appear. Otherwise, we determine the coordinates randomly.
        if (addEarthLikePlanets > 0)
        {
            randomStartPosition = new Vector3(Random.Range(20.0f, 30.0f), Random.Range(20.0f, 30.0f), 0);
        }
        else if (addMoltenPlanets > 0)
        {
            randomStartPosition = new Vector3(Random.Range(5.0f, 10.0f), Random.Range(5.0f, 15.0f), 0);
        }
        else if (addTombPlanets > 0)
        {
            randomStartPosition = new Vector3(Random.Range(20.0f, 30.0f), Random.Range(20.0f, 30.0f), 0);
        }
        else if (addGasGiantPlanets > 0)
        {
            randomStartPosition = new Vector3(Random.Range(40.0f, 60.0f), Random.Range(40.0f, 60.0f), 0);
        }
        else if (addFrozenPlanets > 0)
        {
            randomStartPosition = new Vector3(Random.Range(90.0f, 100.0f), Random.Range(90.0f, 100.0f), 0);
        }
        else
        {
            randomStartPosition = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), 0);
        }

        // Calculate the distance from the planet to the sun.
        float rangeFromSun = Vector2.Distance(randomStartPosition, new Vector3(0, 0, 0));
        
        // Call the method that determines the type of planet.
        SetPlanetType(rangeFromSun, planet);
        
        // Call the method that places the planet in space.
        PlacePlanet(planet);
    }
}