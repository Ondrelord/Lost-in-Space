using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
     public enum planetTypes  // List of Planet Types
    {
        Twin_Earth,
        Gas_Giant,
        Molten,
        Frozen,
        Tomb
    }

    public enum resourceTypes // List of Resource Types
    {
        Empty,         // 0
        Helium,        // 1 
        Iron,          // 2
        Silicium,      // 3
        Gold,          // 4
        Uranium,       // 5
        Nuclear_Fuel,  // 6
        Water          // 7
        
    }

    public planetTypes type;                 // Planet type

    public resourceTypes frequentResource;   // Choosing a frequent resource that is often found on this planet
    string frequentResourceName;        // Index of a frequent resource that is often found on this planet
    int frequentResourceCount;        // The number of units of a frequent resource on this planet

    public resourceTypes normalResource;     // Choosing a usual resource for this planet
    string normalResourceName;          // The index is usually encountered in this planet's resources
    int normalResourceCount;          // The number of units of the usual resource on this planet

    public resourceTypes rareResource;       // Choosing a rare resource for this planet
    string rareResourceName;            // Index of resource rarely found on this planet
    int rareResourceCount;            // The number of units of a rare resource on this planet

    public GameObject scanInterface; // Slot for attaching a panel with information about the planet
    public GameObject planetInfoInterface; // Slot for attaching a panel with information about the planet
    public Text iconText;
    GameObject planetImage;
    GameObject resourceIcon1;
    GameObject resourceIcon2;
    GameObject resourceIcon3;
    public Text descriptionText;

    public Text typeText;                    // Text variable to display the type of planet on the information panel

    public Text frequentResourceCountText;   // Text variable to display the number of units of the frequent resource located here on the info panel
    public Text normalResourceCountText;     // Text variable to display the number of units of a regular resource located here on the info panel
    public Text rareResourceCountText;       // Text variable to display the number of units of a rare resource located here on the info panel

    GameObject btnFrequentResGetting; // Slot for the button that calls the FrequentResourceGetting function
    GameObject btnNormalResGetting;   // Slot for the button that calls the NormalResourceGetting function
    GameObject btnRareResGetting;     // Slot for the button that calls the RareResourceGetting function

    public float updateTime;                   // The time, in seconds, after which resources will be updated

    public bool scaned;                      // Determining whether the planet is scanned

    public bool gasAbsorbent;
    public bool farm;
    public float hydratation;
    public bool miningDrill;
    public bool scaningStation;
    public bool miningStation;               // Determining the presence of a mining station on the planet

    GameObject gasAbsorbentToolToggle;
    GameObject farmToggle;
    GameObject miningDrillToggle;
    GameObject scaningStationToggle;
    GameObject miningStationToggle;

    GameObject btnGAT;
    GameObject btnFarm;
    GameObject btnHydratation;
    Text hydratationCount;
    GameObject btnMiningDrill;
    GameObject btnScaningStation;
    GameObject btnMiningStation;

    private Inventory inventory;             // To interact with the inventory
    private ItemDatabase database;           // To interact with the database
    GameObject gc;

    AudioSource audioSource;
    AudioClip hydratationBtnSound;
    AudioClip miningBtnSound;
    AudioClip buildingBtnSound;
    AudioClip notPossibleSound;

    // Use this for initialization
    void Start ()
    {
        planetInfoInterface.SetActive(true);

        database = GetComponent<ItemDatabase>();                            // We get the database
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>(); // We get inventory
        gc = GameObject.Find("SpaceShip");

        resourceIcon1 = GameObject.Find("frequentResource");
        resourceIcon2 = GameObject.Find("normalResource");
        resourceIcon3 = GameObject.Find("rareResource");

        btnFrequentResGetting = GameObject.Find("btnFrequentResource");
        btnNormalResGetting = GameObject.Find("btnNormalResource");
        btnRareResGetting = GameObject.Find("btnRareResource");

        gasAbsorbentToolToggle = GameObject.Find("togGasTool");
        farmToggle = GameObject.Find("togFarm");
        hydratationCount = GameObject.Find("txtHydratationCount").GetComponent<Text>();
        miningDrillToggle = GameObject.Find("togMiningDrill") ;
        scaningStationToggle = GameObject.Find("togScanStation");
        miningStationToggle = GameObject.Find("togMiningStation");

        btnGAT = GameObject.Find("btnGasTool");
        btnFarm = GameObject.Find("btnFarm");
        btnHydratation = GameObject.Find("btnHydratation");
        btnMiningDrill = GameObject.Find("btnMiningDrill");
        btnScaningStation = GameObject.Find("btnScanStation");
        btnMiningStation = GameObject.Find("btnMiningStation");

        planetImage = GameObject.Find("PlanetIcon");
        scanInterface = GameObject.Find("PlanetInfoOff");

        audioSource = planetInfoInterface.GetComponent<AudioSource>();
        hydratationBtnSound = Resources.Load<AudioClip>("Sounds/hydratation");
        miningBtnSound = Resources.Load<AudioClip>("Sounds/mining");
        buildingBtnSound = Resources.Load<AudioClip>("Sounds/building");
        notPossibleSound = Resources.Load<AudioClip>("Sounds/warnings/not possible");

        if (type == planetTypes.Twin_Earth)
        {
            hydratation = 10;
        }
        else
        {
            hydratation = 0;
        }

        updateTime = 30f;
        InvokeRepeating("MiningStationResourceGeneration", updateTime, updateTime); // Call the ResourceGeneration method at the start, and then every (updateTime) seconds

        planetInfoInterface.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        ResourceDifinition(); // Calculate resource ID

        if (scaned == true) // If the scan was successful, then we initialize the info panel text fields with values and show it
        {
            scanInterface.GetComponent<Image>().enabled = false;

            resourceIcon1.GetComponent<Image>().sprite = Resources.Load<Sprite>("items/" + frequentResourceName);
            resourceIcon2.GetComponent<Image>().sprite = Resources.Load<Sprite>("items/" + normalResourceName);
            resourceIcon3.GetComponent<Image>().sprite = Resources.Load<Sprite>("items/" + rareResourceName);
            planetImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("planeticons/" + gameObject.name);

            planetInfoInterface.SetActive(true);

            if (frequentResourceCount == 0)
                btnFrequentResGetting.GetComponent<Button>().interactable = false;
            else
                btnFrequentResGetting.GetComponent<Button>().interactable = true;

            if (normalResourceCount == 0)
                btnNormalResGetting.GetComponent<Button>().interactable = false;
            else
                btnNormalResGetting.GetComponent<Button>().interactable = true;

            if (rareResourceCount == 0)
                btnRareResGetting.GetComponent<Button>().interactable = false;
            else
                btnRareResGetting.GetComponent<Button>().interactable = true;


            if (gasAbsorbent)
            {
                btnGAT.GetComponent<Button>().interactable = false;
                gasAbsorbentToolToggle.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
                btnGAT.GetComponent<Button>().interactable = true;

            if (farm)
            {
                btnFarm.GetComponent<Button>().interactable = false;
                btnHydratation.GetComponent<Button>().interactable = true;
                farmToggle.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                btnFarm.GetComponent<Button>().interactable = true;
                btnHydratation.GetComponent<Button>().interactable = false;
            }

            if (miningDrill)
            {
                btnMiningDrill.GetComponent<Button>().interactable = false;
                miningDrillToggle.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
                btnMiningDrill.GetComponent<Button>().interactable = true;

            if (scaningStation)
            {
                btnScaningStation.GetComponent<Button>().interactable = false;
                scaningStationToggle.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
                btnScaningStation.GetComponent<Button>().interactable = true;

            if (miningStation)
            {
                btnMiningStation.GetComponent<Button>().interactable = false;
                miningStationToggle.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
                btnMiningStation.GetComponent<Button>().interactable = true;


            iconText.text = "...Scan report...";
            typeText.text = "Type: " + type.ToString();

            if(type == planetTypes.Molten)
            {
                descriptionText.text = "This planet, located close to the sun, is a hot piece of stone on which there is no life. But here you can find rich deposits of minerals.";
            }

            if(type == planetTypes.Gas_Giant)
            {
                descriptionText.text = "This planet belongs to the type of gas giants. What does it mean? FUEL!";
            }

            if(type == planetTypes.Twin_Earth)
            {
                descriptionText.text = "There is life on this planet. At such a moment it would be worthwhile to think about the sublime,\nmeeting with aliens ... But now for you it is just an opportunity to eat.";
            }

            if(type == planetTypes.Frozen)
            {
                descriptionText.text = "History is silent about what happened to this frozen planet, but only nuclear fuel remained from its former inhabitants ... and the throne. Frozen Throne.";
            }

            frequentResourceCountText.text = frequentResourceCount.ToString();
            normalResourceCountText.text = normalResourceCount.ToString();
            rareResourceCountText.text = rareResourceCount.ToString();
            hydratationCount.text = ((int) hydratation).ToString();
        }

        if (gameObject.ToString().Substring(0, 5) == "Earth")
        {
            FoodReplenishment();
        }
    }

    void FoodReplenishment()                         // Function to collect food
    {
        if (gc.GetComponent<Ship>().foodReplenishment && gc.GetComponent<GameController>().foodCount < gc.GetComponent<GameController>().maxFoodCount) // As long as the collection of food is possible and stocks are not at the maximum
        {
            int farmVariable;

            if (farm)
            {
                farmVariable = inventory.database.GetItemData("Farm").stats["boost"];
            }
            else
            {
                farmVariable = 3;
            }



            if (hydratation < 1)
            {
                gc.GetComponent<GameController>().foodCount += Time.deltaTime * farmVariable;                          // We increase the variable responsible for food
                hydratation = 0;
            }
            else
            {
                gc.GetComponent<GameController>().foodCount += Time.deltaTime * farmVariable * 2;                          // We increase the variable responsible for food
                hydratation -= Time.deltaTime;
            }
        }
    }
    
    public void MiningStationResourceGeneration()
    {
        if (miningStation)
            ResourceGeneration();
    }

    public void ResourceGeneration() // The method of renewing the amount of resources on the planets
    {
        int frequentMin = scaningStation ? 5 : 3;
        int frequentMax = scaningStation ? 10 : 5;
        int normalMin = scaningStation ? 4 : 2;
        int normalMax = scaningStation ? 6 : 4;
        int rareMin = scaningStation ? 2 : 1;
        int rareMax = scaningStation ? 4 : 2;

        /* We generate a random number of each type of resource for a given planet in the indicated lower and upper bounds */

        if (miningStation)
        {
            if (frequentResourceName != "Empty")
            {
                if ((frequentResourceCount += Random.Range(frequentMin, frequentMax)) > 15)
                    frequentResourceCount = 15;
            }
            else
                frequentResourceCount = 0;

            if (normalResourceName != "Empty")
            {
                if ((normalResourceCount += Random.Range(normalMin, normalMax)) > 10)
                    normalResourceCount = 10;
            }
            else
                normalResourceCount = 0;

            if (rareResourceName != "Empty")
            {
                if (rareResourceName == "Nuclear fuel")
                    rareResourceCount = 1;
                else if ((rareResourceCount += Random.Range(rareMin, rareMax)) > 5)
                    rareResourceCount = 5;
            }
            else
                rareResourceCount = 0;
        }
        else
        {
            if (frequentResourceName != "Empty")
                frequentResourceCount = Random.Range(frequentMin, frequentMax);
            else
                frequentResourceCount = 0;

            if (normalResourceName != "Empty")
                normalResourceCount = Random.Range(normalMin, normalMax);
            else
                normalResourceCount = 0;

            if (rareResourceName != "Empty")
            {
                if (rareResourceName == "Nuclear fuel")
                    rareResourceCount = 1;
                else
                    rareResourceCount = Random.Range(rareMin, rareMax);
            }
            else
                rareResourceCount = 0;
        }
    }

    /*
    void OnMouseUp() // When clicking on the planet
    {
        ScaningOff(); // Forcibly complete the past scan

        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find a player by tag
        GameObject planet = this.gameObject;

        double distance = GetInteractionDistance(planet, player); // Calculate the distance between the player and the planet

        if (distance < 5) // If the distance is small
        {
            this.scaned = true; // That scan will be successful
        }
        else        // Otherwise, no
        {
            scaned = false;
            return; // And do not need to do anything
        }
    }/**/

    public static double GetInteractionDistance(GameObject g1, GameObject g2) // Returns the distance between objects
    {
        double a = (g1.transform.position.x - g2.transform.position.x) * (g1.transform.position.x - g2.transform.position.x);  //distance on X-axis
        double b = (g1.transform.position.y - g2.transform.position.y) * (g1.transform.position.y - g2.transform.position.y);  //distance on Y-axis
        double c = Mathf.Sqrt((float)(a + b)); //Pythagoras

        return c;
    }

    public void ScaningOff() // Function to stop the planet being marked as scanned
    {
        scaned = false; // Remove the flag

        scanInterface.GetComponent<Image>().enabled = true;

        /* Deactivate what was activated, otherwise the next scan will conflict */
        planetInfoInterface.SetActive(false);
        
        gasAbsorbentToolToggle.GetComponent<SpriteRenderer>().enabled = false;
        farmToggle.GetComponent<SpriteRenderer>().enabled = false;
        miningDrillToggle.GetComponent<SpriteRenderer>().enabled = false;
        scaningStationToggle.GetComponent<SpriteRenderer>().enabled = false;
        miningStationToggle.GetComponent<SpriteRenderer>().enabled = false;

        iconText.text = "Not scanned\nHold [F] to scan";
    }

    public void FrequentResourceGetting() // Function to collect the most common resource on the planet
    {
        int amount = 1;

        if(frequentResourceCount == 0)                       // If the resource is exhausted
        {
            inventory.RiseInfoPrefab("Resource exhausted!"); // Then we display the message
            return;                                          // And we will not do anything else
        }

        if (!inventory.AddItem(frequentResourceName))
        {
            inventory.RiseInfoPrefab("Inventory is full!");
            return;
        }

        audioSource.PlayOneShot(miningBtnSound);

        frequentResourceCount -= amount;                            // And reduce the counter of this resource on the planet
    }

    public void NormalResourceGetting() // Function to collect a commonly found resource on the planet
    {
        if (!(miningDrill || gasAbsorbent))
        {
            if (type == planetTypes.Gas_Giant)
                inventory.RiseInfoPrefab("You need Gas absorbent tool on this planet to get this resource!");
            else
                inventory.RiseInfoPrefab("You need Mining drill on this planet to mine this resource!");
            return;
        }

        int amount = 1;

        if (normalResourceCount == 0)
        {
            inventory.RiseInfoPrefab("Resource exhausted!");
            return;
        }

        
        if (!inventory.AddItem(normalResourceName))
        {
            inventory.RiseInfoPrefab("Inventory is full!");
            return;
        }

        audioSource.PlayOneShot(miningBtnSound);

        normalResourceCount -= amount;
    }

    public void RareResourceGetting() // Function to collect a rare resource on the planet
    {
        if (!(miningDrill || gasAbsorbent))
        {
            if (type == planetTypes.Gas_Giant)
                inventory.RiseInfoPrefab("You need Gas absorbent tool on this planet to get this resource!");
            else if (type != planetTypes.Twin_Earth)
                inventory.RiseInfoPrefab("You need Mining drill on this planet to mine this resource!");
            return;
        }

        int amount = 1;

        if (rareResourceCount == 0)
        {
            inventory.RiseInfoPrefab("Resource exhausted!");
            return;
        }

        
        if (!inventory.AddItem(rareResourceName))
        {
            inventory.RiseInfoPrefab("Inventory is full!");
            return;
        }

        audioSource.PlayOneShot(miningBtnSound);

        rareResourceCount -= amount;
    }

    void ResourceDifinition() // The function calculates the ID of the resource located on the planet, 
                              //  since there is no direct correspondence between the resource here and in the database
    {
        /* 1 */
        switch (frequentResource)
        {
            case resourceTypes.Empty:
                frequentResourceName = "Empty";
                break;
            case resourceTypes.Helium:
                frequentResourceName = "Helium";
                break;
            case resourceTypes.Iron:
                frequentResourceName = "Alloy";
                break;
            case resourceTypes.Silicium:
                frequentResourceName = "Silicium";
                break;
            case resourceTypes.Gold:
                frequentResourceName = "Gold";
                break;
            case resourceTypes.Uranium:
                frequentResourceName = "Uranium";
                break;
            case resourceTypes.Nuclear_Fuel:
                frequentResourceName = "Nuclear Fuel";
                break;
            case resourceTypes.Water:
                frequentResourceName = "Water";
                break;
        }

        /* 2 */
        switch (normalResource)
        {
            case resourceTypes.Empty:
                normalResourceName = "Empty";
                break;
            case resourceTypes.Helium:
                normalResourceName = "Helium";
                break;
            case resourceTypes.Iron:
                normalResourceName = "Alloy";
                break;
            case resourceTypes.Silicium:
                normalResourceName = "Silicium";
                break;
            case resourceTypes.Gold:
                normalResourceName = "Gold";
                break;
            case resourceTypes.Uranium:
                normalResourceName = "Uranium";
                break;
            case resourceTypes.Nuclear_Fuel:
                normalResourceName = "Nuclear Fuel";
                break;
            case resourceTypes.Water:
                normalResourceName = "Water";
                break;
        }

        /* 3 */
        switch (rareResource)
        {
            case resourceTypes.Empty:
                rareResourceName = "Empty";
                break;
            case resourceTypes.Helium:
                rareResourceName = "Helium";
                break;
            case resourceTypes.Iron:
                rareResourceName = "Alloy";
                break;
            case resourceTypes.Silicium:
                rareResourceName = "Silicium";
                break;
            case resourceTypes.Gold:
                rareResourceName = "Gold";
                break;
            case resourceTypes.Uranium:
                rareResourceName = "Uranium";
                break;
            case resourceTypes.Nuclear_Fuel:
                rareResourceName = "Nuclear Fuel";
                break;
            case resourceTypes.Water:
                rareResourceName = "Water";
                break;
        }
    }

    public void AddGAT()
    {
        if (type != planetTypes.Gas_Giant)
        {
            inventory.RiseInfoPrefab("It can only be used on gas giants!");
            audioSource.PlayOneShot(notPossibleSound);
            return;
        }

        if (inventory.CheckItemCount("Gas absorbent tool") > 0)
        {
            inventory.RemoveItem("Gas absorbent tool");
            gasAbsorbent = true;

            audioSource.PlayOneShot(buildingBtnSound);
        }
        else
        {
            inventory.RiseInfoPrefab("You need Gas absorbent tool to do that.");
            audioSource.PlayOneShot(notPossibleSound);
        }
    }

    public void AddFarm()
    {
        if (type != planetTypes.Twin_Earth)
        {
            inventory.RiseInfoPrefab("It can only be used by planets with life!");
            audioSource.PlayOneShot(notPossibleSound);
            return;
        }

        if (inventory.CheckItemCount("Farm") > 0)
        {
            inventory.RemoveItem("Farm");
            farm = true;

            audioSource.PlayOneShot(buildingBtnSound);
        }
        else
        {
            inventory.RiseInfoPrefab("You need Farm to do that.");
            audioSource.PlayOneShot(notPossibleSound);
        }
    }

    public void AddHydratation()
    {
        if (farm == false)
        {
            inventory.RiseInfoPrefab("For this you need to build a farm!");
            return;
        }

        if (inventory.CheckItemCount("Water") > 0)
        {
            inventory.RemoveItem("Water");
            hydratation += inventory.database.GetItemData("Water").stats["hydratation"];
            audioSource.PlayOneShot(hydratationBtnSound);
        }
        else
        {
            inventory.RiseInfoPrefab("You don't have enough water to do that.");
            audioSource.PlayOneShot(notPossibleSound);
        }
    }

        public void AddMiningDrill()
    {
        /*if (type == planetTypes.Gas_Giant || type == planetTypes.Twin_Earth) // Why?
        {
            inventory.RiseInfoPrefab("It is impossible to use it on gas giants!");
            audioSource.PlayOneShot(notPossibleSound);
            return;
        }*/

        if (inventory.CheckItemCount("Mining drill") > 0)
        {
            inventory.RemoveItem("Mining drill");
            miningDrill = true;

            audioSource.PlayOneShot(buildingBtnSound);
        }
        else
        {
            inventory.RiseInfoPrefab("You need Mining drill to do that.");
            audioSource.PlayOneShot(notPossibleSound);
        }

    }

    public void AddScaningStation()
    {
        if (inventory.CheckItemCount("Scanning station") > 0)
        {
            inventory.RemoveItem("Scanning station");
            scaningStation = true;

            audioSource.PlayOneShot(buildingBtnSound);
        }
        else
        {
            inventory.RiseInfoPrefab("You need Scanning station to do that.");
            audioSource.PlayOneShot(notPossibleSound);
        }

    }

    public void AddMiningStation()
    {
        if (inventory.CheckItemCount("Mining station") > 0)
        {
            inventory.RemoveItem("Mining station");
            miningStation = true;

            audioSource.PlayOneShot(buildingBtnSound);
        }
        else
        {
            inventory.RiseInfoPrefab("You need Mining station to do that.");
            audioSource.PlayOneShot(notPossibleSound);
        }

    }

    public void resetPlanet()
    {
        frequentResourceCount = 0;
        normalResourceCount = 0;
        rareResourceCount = 0;

        gasAbsorbent = false;
        farm = false;
        if(gameObject.name == "Earth")
            hydratation = 10;
        else
            hydratation = 0;
        miningDrill = false;
        scaningStation = false;
        miningStation = false;
    }
}
