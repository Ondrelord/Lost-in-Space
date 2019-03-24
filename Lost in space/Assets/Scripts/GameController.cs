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
    GameObject tutorial;
    GameObject lesson1;
    GameObject lesson2;
    GameObject mission;
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
    GameObject engines;

    public SimpleHealthBar healthBar;
    public SimpleHealthBar fuelBar;
    public SimpleHealthBar foodBar;
    double lastColorChanged = 0;
    bool colorNormal = true;
    bool shipDamaged = false;

    public bool upgWorkbench1;
    public bool upgWorkbench2;
    public int  upgNavigation;
    public bool upgFoodStorage;
    public bool upgFuelStorage;
    public bool upgEngines;

    private AudioSource warningAudioSource;
    private AudioSource hitAudioSource;
    private AudioSource shieldAudioSource;
    private AudioClip warningHealthStart;
    private AudioClip warningHealthLoop;
    private AudioClip warningLowFuel;
    private AudioClip warningLowFood;
    private AudioClip shieldLoop;
    private AudioClip craftPanelOpen;
    private AudioClip craftPanelClose;
    private AudioClip death;
    private AudioClip[] hitSounds;
    public bool tutorialOn = true;
    bool warningOn = false;


    // Use this for initialization
    void Start()
    {
        gamespeed = 0.1f; //0.1s
        //shieldPeriod = 10;
        StartCoroutine("PlayTimer"); //start timer for game
        asteroidShield = GameObject.Find("Circle");
        cooldown = GameObject.Find("Cooldown");

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        craftingPanel = GameObject.Find("CraftingPanel");
        craftingGUI = GameObject.Find("CraftingGUIPanel");
        craftButton = GameObject.Find("Canvas").transform.Find("btnCraftPanelOn").gameObject;
        craftButton.SetActive(false);
        mission = GameObject.Find("Mission");
        tutorial = GameObject.Find("Tutorial");
        lesson1 = GameObject.Find("Lesson1Panel");
        lesson2 = GameObject.Find("Lesson2Panel");
        lesson2.SetActive(false);
        tutorialOn = true;

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

        engines = GameObject.Find("engines_player");
        engines.SetActive(false);
        comet = GameObject.Find("Comet");
        healthBar = GameObject.Find("HB_Status Fill 00").GetComponent<SimpleHealthBar>();
        fuelBar = GameObject.Find("HB_Status Fill 01").GetComponent<SimpleHealthBar>();
        foodBar = GameObject.Find("FB_Status Fill 01").GetComponent<SimpleHealthBar>();

        warningAudioSource = GetComponents<AudioSource>()[1];
        hitAudioSource = GetComponents<AudioSource>()[2];
        shieldAudioSource = GetComponents<AudioSource>()[3];
        warningHealthStart = Resources.Load<AudioClip>("Sounds/warnings/alarm health start");
        warningHealthLoop = Resources.Load<AudioClip>("Sounds/warnings/alarm health loop");
        warningLowFuel = Resources.Load<AudioClip>("Sounds/warnings/warning low fuel");
        warningLowFood = Resources.Load<AudioClip>("Sounds/warnings/warning low food");
        shieldLoop = Resources.Load<AudioClip>("Sounds/shield loop");
        craftPanelOpen = Resources.Load<AudioClip>("Sounds/craft panel open");
        craftPanelClose = Resources.Load<AudioClip>("Sounds/craft panel close");
        death = Resources.Load<AudioClip>("Sounds/death");
        hitSounds = Resources.LoadAll<AudioClip>("Sounds/hit sounds");


        InitGame(true);
    }

    void Update()
    {
        if (Input.GetKeyUp("escape") && gameIsRunning)
        {
            if (tutorialOn)
                SetUpTutorial();
            else if (mission.activeSelf)
            {
                mission.SetActive(false);
                PauseGame();
            }
            else if ((transform.parent != null) && (transform.parent.tag == "Planet" || transform.parent.tag == "Living Planet") && transform.parent.parent.GetComponent<Planet>().scaned)
                transform.parent.parent.GetComponent<Planet>().ScaningOff();
            else
            {
                gameButton.enabled = !gameButton.enabled;
                restartButton.enabled = !restartButton.enabled;
                quitButton.enabled = !quitButton.enabled;
                mainCamera.enabled = !mainCamera.enabled;
                menuCamera.enabled = !menuCamera.enabled;
                PauseGame();
            }
        }

        if (paused)
            return;

        if (mission.activeSelf)
            PauseGame();

        if (Input.GetKeyUp("h"))
        {
            SetUpTutorial();
        }

        //Open and Hide Crafting system
        if (Input.GetKeyUp("c")) //KeyUp is better interaction for one click.
        {
            if (!craftingSystemOpen)
            {
                craftingSystemOpen = true;
                craftingPanel.SetActive(true);
                craftingGUI.SetActive(true);
                craftButton.SetActive(false);

                hitAudioSource.PlayOneShot(craftPanelOpen);
            }
            else
            {
                craftingSystemOpen = false;
                craftingPanel.SetActive(false);
                craftingGUI.SetActive(false);
                craftButton.SetActive(true);

                hitAudioSource.PlayOneShot(craftPanelClose);
            }
        }

        checkLowHP();
        checkLowFood();
        checkLowFuel();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (paused)
            return;

        //Fuel wasted for move
        Mathf.Clamp((float)fuelCount, 0, 300);

        bool moving = (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d")
                || Input.GetKey("up") || Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("down")
                || Input.GetKey("e") || Input.GetKey("q"));

        if (fuelCount >= 0)
        {
            engines.transform.GetChild(1).gameObject.SetActive(true);
            if (moving)
            {
                engines.SetActive(true);
                FuelWaste(fuelWasted);
            }
            else
                engines.SetActive(false);
        }
        else
        {
            engines.transform.GetChild(1).gameObject.SetActive(false);
            if (moving)
                engines.SetActive(true);
            else
                engines.SetActive(false);
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
            hitAudioSource.PlayOneShot(death);
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

        float minPitch = 0.3f;
        float maxPitch = 1f;

        hitAudioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)], Random.Range(minPitch, maxPitch));
    }

    public void AddFuel(int number)
    {
        fuelCount += number;
        if (fuelCount > maxFuelCount) { fuelCount = maxFuelCount; }
    }

    public void SetUpTutorial()
    {
        //tutorial.SetActive(!tutorial.activeSelf);
        if (tutorialOn)
        {
            tutorial.SetActive(false);
            lesson1.SetActive(false);
            lesson2.SetActive(false);
            tutorialOn = false;
        }
        else
        {
            tutorial.SetActive(true);
            lesson1.SetActive(true);
            tutorialOn = true;
        }
    }

    public void RunAsteroidShield(int time)
    {
        shieldPeriod = time;
        checkingTimeShield = playtime;
        asteroidShieldOpen = true;
    }

    private void InputForAsteroidShield()
    {
        if (Input.GetKeyDown("z"))
        {
            if (inventory.FindItem("Asteroid Shield") != -1)
            {
                inventory.RemoveItem("Asteroid Shield");
                RunAsteroidShield(inventory.database.GetItemData("Asteroid Shield").stats["time"]);
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

            if (!shieldAudioSource.isPlaying && !inventory.GetComponent<AudioSource>().isPlaying)
            {
                shieldAudioSource.clip = shieldLoop;
                shieldAudioSource.Play();
            }
        }
        else
        {
            asteroidShield.SetActive(false);
            cooldown.SetActive(false);
            shieldAudioSource.Stop();
        }
    }

    public void CheckEndOfScene()
    {
        if (lasttime2 + 3 < playtime) //rise warning info
        {
            if (gameObject.transform.position.x > 150 || gameObject.transform.position.x < -150 || gameObject.transform.position.y > 150 || gameObject.transform.position.y < -150)
            {
                WarningSpaceOpen.GetComponent<Renderer>().enabled = true;
                WarningSpaceOpen.GetComponent<AudioSource>().enabled = true;
                anim1.Play("Entry");
            }
            else
            {
                if (WarningSpaceOpen.GetComponent<Renderer>().enabled == true)
                {
                    WarningSpaceOpen.GetComponent<Renderer>().enabled = false;
                    WarningSpaceOpen.GetComponent<AudioSource>().enabled = false;
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

    private void checkLowHP()
    {
        if (hP < 25)
        {
            if (!warningOn)
            {
                warningOn = true;
                warningAudioSource.clip = warningHealthStart;
                warningAudioSource.Play();
            }
            if (warningOn)
            {
                if (!warningAudioSource.isPlaying)
                {
                    warningAudioSource.clip = warningHealthLoop;
                    warningAudioSource.Play();
                    warningAudioSource.volume -= 0.2f;
                    if (warningAudioSource.volume < 0.25f)
                        warningAudioSource.volume = 0.25f;
                }
            }
        }
        else if (warningAudioSource.clip == warningHealthLoop)
        {
            warningAudioSource.volume = 1f;
            warningAudioSource.Stop();
            warningOn = false;
        }
    }

    private void checkLowFuel()
    {
        if (fuelCount < maxFuelCount/5)
        {
            if (!warningOn)
            {
                if (!warningAudioSource.isPlaying)
                {
                    warningAudioSource.clip = warningLowFuel;
                    warningAudioSource.volume = 0.6f;
                    warningAudioSource.Play();
                }
            }
        }
    }

    private void checkLowFood()
    {
        if (foodCount < maxFoodCount/5)
        {
            if (!warningOn)
            {
                if (!warningAudioSource.isPlaying)
                {
                    warningAudioSource.clip = warningLowFood;
                    warningAudioSource.volume = 0.6f;
                    warningAudioSource.Play();
                }
            }
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

    public void InitGame(bool first = false)
    {
        //stats
        hP = 100;        
        foodCount = 60;
        fuelCount = 200;   
        playtime = 0;
        lasttime = 0;
        asteroidShield.SetActive(false);

        //map
        GameObject.Find("Sun - Core").GetComponent<Sun>().lasttime = -3;
        comet.GetComponent<CometHandler>().InitComet();
        comet.GetComponent<CometHandler>().InitAsteroids();
        Transform planets = GameObject.Find("Planets").transform;
        for (int i = 0; i < planets.childCount; i++)
            planets.GetChild(i).GetComponent<Planet>().resetPlanet();


        //ship
        GameObject startPlanet = GameObject.Find("LavaPlanet");
        GameObject.Find("SpaceShip").transform.SetPositionAndRotation(new Vector2(startPlanet.transform.position.x + 2, startPlanet.transform.position.y), Quaternion.Euler(0, 0, 0));
        GameObject.Find("SpaceShip").GetComponent<Ship>().Stop();
        pausedVelocity = new Vector2(0, 0);
        ChangeColorToNormal();

        //menu
        gameButton.GetComponentInChildren<Text>().text = " NEW GAME";
        mission.SetActive(true);
        if (!first)
        {
            gameButton.enabled = !gameButton.enabled;
            restartButton.enabled = !restartButton.enabled;
            restartButton.gameObject.SetActive(false);
            quitButton.enabled = !quitButton.enabled;
            mainCamera.enabled = false;
            menuCamera.enabled = !menuCamera.enabled;
        }
        gameIsRunning = false;
        paused = true;
        

        //inv & crafting
        inventory.clearInventory();
        for (int i = 0; i < 17; i++)
            craftingPanel.transform.GetChild(i).GetChild(0).GetComponent<CraftingHandler>().clearCraftPanel();
        upgEngines = false;
        upgFoodStorage = false;
        upgFuelStorage = false;
        upgNavigation = 0;
        upgWorkbench1 = false;
        upgWorkbench2 = false;
    }



}
