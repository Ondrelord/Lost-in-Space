using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    GameObject craftingPanel;
    GameObject comet;
    GameObject craftingGUI;
    GameObject craftButton;
    GameObject WarningSpaceOpen;
    Camera mainCamera;
    Camera menuCamera;
    Camera endCamera;
    Button gameButton;
    Button restartButton;
    Button backtomenuButton;
    Button quitButton;
    public Text time;
    public Vector2 pausedVelocity;
    public double hP = 100;        // Health counter.
    public float maxHP = 100;
    public float foodCount = 60;
    public float maxFoodCount = 100;
    public double fuelCount = 200;   // Fuel counter.
    public float maxFuelCount = 300;
    public double playtime = 0;
    public double lasttime = 0;
    public double lasttime2 = 0;
    public double fuelWasted = 0.1f;
    public double foodWasted = 1;   //For 1s
    public float gamespeed;
    public double checkingTimeShield = 0;
    public bool asteroidShieldOpen = false;
    public bool craftingSystemOpen = true;
    public GameObject asteroidShield;
    public GameObject cooldown;
    public Inventory inventory;
    public int shieldPeriod;
    public bool paused;
    public bool gameIsRunning;      //false until new game button is clicked
    public Animator anim1;

    public SimpleHealthBar healthBar;
    public SimpleHealthBar fuelBar;
    public SimpleHealthBar foodBar;
    double lastColorChanged = 0;
    bool colorNormal = true;
    bool shipDamaged = false;

    // Use this for initialization
    void Start()
    {
        gamespeed = 0.1f; //0.1s
        shieldPeriod = 10;
        StartCoroutine("PlayTimer"); //start timer for game
        asteroidShield = GameObject.Find("Circle");
        cooldown = GameObject.Find("Cooldown");

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        craftingPanel = GameObject.Find("CraftingPanel");
        craftingGUI = GameObject.Find("CraftingGUIPanel");
        craftButton = GameObject.Find("Canvas").transform.Find("btnCraftPanelOn").gameObject;
        craftButton.SetActive(false);

        WarningSpaceOpen = GameObject.Find("Warning space open");
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        menuCamera = GameObject.Find("Menu Camera").GetComponent<Camera>();
        endCamera = GameObject.Find("End Game Camera").GetComponent<Camera>();
        gameButton = GameObject.Find("Game button").GetComponent<Button>();
        restartButton = GameObject.Find("Restart button").GetComponent<Button>();
        restartButton.gameObject.SetActive(false);
        backtomenuButton = GameObject.Find("Menu Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit button").GetComponent<Button>();
        anim1 = WarningSpaceOpen.GetComponent<Animator>();
        gameIsRunning = false;
        paused = true;

        comet = GameObject.Find("Comet_prototype");
        healthBar = GameObject.Find("HB_Status Fill 00").GetComponent<SimpleHealthBar>();
        fuelBar = GameObject.Find("HB_Status Fill 01").GetComponent<SimpleHealthBar>();
        foodBar = GameObject.Find("FB_Status Fill 01").GetComponent<SimpleHealthBar>();
    }

    void Update()
    {
        if (Input.GetKeyUp("escape") && gameIsRunning)
        {
            gameButton.enabled = !gameButton.enabled;
            restartButton.enabled = !restartButton.enabled;
            quitButton.enabled = !quitButton.enabled;
            mainCamera.enabled = !mainCamera.enabled;
            menuCamera.enabled = !menuCamera.enabled;
            PauseGame();
        }

        if (paused)
            return;

        //Open and Hide Crafting system
        if (Input.GetKeyUp("c")) //KeyUp is better interaction for one click.
        {
            if (!craftingSystemOpen)
            {
                craftingSystemOpen = true;
                craftingPanel.SetActive(true);
                craftingGUI.SetActive(true);
                craftButton.SetActive(false);
            }
            else
            {
                craftingSystemOpen = false;
                craftingPanel.SetActive(false);
                craftingGUI.SetActive(false);
                craftButton.SetActive(true);
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (paused)
            return;

        //Fuel wasted for move
        Mathf.Clamp((float)fuelCount, 0, 300);

        if (fuelCount >= 0)
        {
            if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("e") || Input.GetKey("q"))
            {
                FuelWaste(fuelWasted);
            }

            if (Input.GetKey("up") || Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("down"))
            {
                FuelWaste(fuelWasted / 2);
            }
        }

        //Food wasted for 1s
        if (playtime - lasttime > 1)
        {
            if (foodCount > 0)
            {
                FoodWaste(1);
            }
            else   //Sufering by food - ubere dva životy každou vteřinu.
            {
                this.hP -= 2;
            }
            lasttime = playtime;
        }

        RenderProperties();

        if (hP < 0)
        {
            EndGame(false);
            //InitGame(); 
        }

        InputForAsteroidShield();
        CheckAsteroidShield();
        CheckEndOfScene();
    }

    void RenderProperties()
    {
        healthBar.UpdateBar((float)hP, maxHP);
        fuelBar.UpdateBar((float)fuelCount, maxFuelCount);
        foodBar.UpdateBar(foodCount, maxFoodCount);
        float interval;

        if (shipDamaged)
        {
            interval = 0.3f;
        }
        else
        {
            interval = 0.5f;
        }

        //Change the color, when the ship suffer by food or hit by asteroid.
        if (foodCount == 0 || shipDamaged)
        {
            if (playtime > lastColorChanged + interval)
            {
                if (colorNormal)
                {
                    ChangeColorToRed();
                }
                else
                {
                    if (asteroidShieldOpen)
                    {
                        ChangeColorToBlue();
                    }
                    else
                    {
                        ChangeColorToNormal();
                    }
                }
                lastColorChanged = playtime;

                if (shipDamaged)
                {
                    shipDamaged = false;
                }
            }
        }
        else
        {
            if (asteroidShieldOpen)
            {
                ChangeColorToBlue();
            }
            else
            {
                ChangeColorToNormal();
            }
        }
        time.text = "PlayTime: " + (Mathf.Round((float)playtime)).ToString();
    }

    void ChangeColorToNormal()
    {
        healthBar.GetComponent<Image>().color = new Color32(255, 100, 0, 255);
        colorNormal = true;
    }

    void ChangeColorToBlue()
    {
        healthBar.GetComponent<Image>().color = new Color32(0, 150, 255, 255);
        colorNormal = true;
    }

    void ChangeColorToRed()
    {
        healthBar.GetComponent<Image>().color = new Color32(255, 20, 0, 200);
        colorNormal = false;
    }

    void FoodWaste(float number)
    {
        foodCount -= number;
    }

    void FuelWaste(double number)
    {
        fuelCount -= number;
    }

    public void HitPlayer(double number)
    {
        this.hP -= number;
        ChangeColorToRed();
        colorNormal = false;
        lastColorChanged = playtime;
        shipDamaged = true;
    }

    public void AddFuel(int number)
    {
        fuelCount += number;
        if (fuelCount > maxFuelCount) { fuelCount = maxFuelCount; }
    }

    public void RunAsteroidShield()
    {
        checkingTimeShield = playtime;
        asteroidShieldOpen = true;
    }

    private void InputForAsteroidShield()
    {
        if (Input.GetKeyDown("z"))
        {
            if (inventory.FindItem("Shield for the asteroid belt") != -1)
            {
                inventory.RemoveItem("Shield for the asteroid belt");
                RunAsteroidShield();
            }
        }
    }

    public void CheckAsteroidShield()
    {
        if (asteroidShieldOpen)
        {
            if (checkingTimeShield + shieldPeriod < playtime)
            {
                asteroidShieldOpen = false;
            }
            else
            {
                if (GameObject.Find("Cooldown") == null)
                {
                    asteroidShield.SetActive(true);
                    cooldown.SetActive(true);
                }
                cooldown.GetComponent<Text>().text = ((int)(10 - (playtime - checkingTimeShield))).ToString();
            }
        }
        else
        {
            asteroidShield.SetActive(false);
            cooldown.SetActive(false);
        }
    }

    public void CheckEndOfScene()
    {
        if (lasttime2 + 3 < playtime) //rise warning info
        {
            if (gameObject.transform.position.x > 170 || gameObject.transform.position.x < -100 || gameObject.transform.position.y > 100 || gameObject.transform.position.y < -120)
            {
                WarningSpaceOpen.GetComponent<Renderer>().enabled = true;
                anim1.Play("Entry");
            }
            else
            {
                if (WarningSpaceOpen.GetComponent<Renderer>().enabled == true)
                {
                    WarningSpaceOpen.GetComponent<Renderer>().enabled = false;
                }
            }
            lasttime2 = playtime;
        }
    }

    private IEnumerator PlayTimer()
    {
        while (true)
        {
            if (!paused)
                playtime += gamespeed;
            yield return new WaitForSeconds(gamespeed);
        }
    }

    public void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            pausedVelocity = GameObject.Find("SpaceShip").GetComponent<Rigidbody2D>().velocity;
            GameObject.Find("SpaceShip").GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        else
        {
            GameObject.Find("SpaceShip").GetComponent<Rigidbody2D>().velocity = pausedVelocity;
            pausedVelocity = new Vector2(0, 0);
        }
    }

    public void NewGame()
    {
        restartButton.gameObject.SetActive(true);
        mainCamera.enabled = true;
        gameIsRunning = true;
        paused = false;
        gameButton.GetComponentInChildren<Text>().text = " RESUME";
    }

    public void RestartGame()
    {
        InitGame();
        NewGame();
    }

    public void EndGame(bool win)
    {
        Text message = GameObject.Find("Message").GetComponent<Text>();

        if (win)
        {
            message.text = "YOU WON";
            message.color = new Color(0, 0, 0);
        }
        else
        {
            message.text = "YOU DIED";
            message.color = new Color(170, 0, 0);
        }

        GameObject.Find("SpaceShip").transform.SetPositionAndRotation(new Vector2(25, 10), Quaternion.Euler(0, -180, 180)); //just moving ship outside of canvas

        mainCamera.enabled = false;
        backtomenuButton.enabled = true;
        endCamera.enabled = true;
        gameIsRunning = false;
        paused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InitGame()
    {
        //stats
        hP = 100;        
        foodCount = 60;
        fuelCount = 200;   
        playtime = 0;
        lasttime = 0;

        //map
        GameObject.Find("Sun - Core").GetComponent<Sun>().lasttime = -3;
        comet.GetComponent<CometHandler>().InitComet();
        comet.GetComponent<CometHandler>().InitAsteroids();

        //ship
        GameObject startPlanet = GameObject.Find("LavaPlanet");
        GameObject.Find("SpaceShip").transform.SetPositionAndRotation(new Vector2(startPlanet.transform.position.x + 2 , startPlanet.transform.position.y), Quaternion.Euler(0,-180,180 ));
        GameObject.Find("SpaceShip").GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GameObject.Find("SpaceShip").GetComponent<Ship>().Stop();
        pausedVelocity = new Vector2(0, 0);
        ChangeColorToNormal();

        //menu
        gameButton.GetComponentInChildren<Text>().text = " NEW GAME";
        gameButton.enabled = !gameButton.enabled;
        restartButton.enabled = !restartButton.enabled;
        restartButton.gameObject.SetActive(false);
        quitButton.enabled = !quitButton.enabled;
        mainCamera.enabled = false;
        menuCamera.enabled = !menuCamera.enabled;
        gameIsRunning = false;
        paused = true;

        //inv & crafting
        inventory.clearInventory();
        for (int i = 0; i < 17; i++)
            craftingPanel.transform.GetChild(i).GetChild(0).GetComponent<CraftingHandler>().clearCraftPanel();
    }

}
