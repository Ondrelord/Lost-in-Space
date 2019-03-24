using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float speed;             // Speed factor
    public float maxForwardSpeed;   // Forward motion restriction     
    public float maxBackwardSpeed;  // Restriction of movement back
    public float rotationSpeed;     // Rotation speed factor

    public float movementDirection; // The direction of movement
    public float rotationDirection; // The direction of rotation
    public float strafeMovement;    // How much you can move to left/right side?

    private float moveVertical;
    private float moveHorizontal;

    public Transform target;        // Component to get target coordinates

    public bool foodReplenishment;  // The determinant of whether food is replenished near the planet
    public bool speedUpgrade = false;

    private Rigidbody2D myRigidbody; // For physics
    private GameController gC;       // To access health and nutrition indicators

    private AudioSource[] audioSource;
    public AudioSource engineAudioSource;
    public AudioSource warningsAudioSource;
    public AudioSource hitAudioSource;

    AudioClip engineStart;
    AudioClip engineSteady;
    AudioClip engineEnd;
    AudioClip engineIdle;

    private bool moveChange;
    bool strafing = false;
    bool rotating = false;


    // Use this for initialization
    void Start ()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>(); // We take a component from our object
        gC = this.GetComponent<GameController>();       // Get GameController
        audioSource = GetComponents<AudioSource>();
        engineAudioSource = audioSource[0];
        warningsAudioSource = audioSource[1];
        warningsAudioSource = audioSource[2];

        engineStart = Resources.Load<AudioClip>("Sounds/engine/pohon start");
        engineSteady = Resources.Load<AudioClip>("Sounds/engine/pohon steady");
        engineEnd = Resources.Load<AudioClip>("Sounds/engine/pohon end");
        engineIdle = Resources.Load<AudioClip>("Sounds/engine/pohon idle");

        speed = 1f;
        maxForwardSpeed = 3f;
        maxBackwardSpeed = -1f;
        rotationSpeed = 1f;
        foodReplenishment = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gC.paused)
            return;

        if (gC.fuelCount <= 0)
        {
            maxForwardSpeed = 1.5f;
            maxBackwardSpeed = -0.5f;
            rotationSpeed = 0.5f;
        }
        else
        {
            if (speedUpgrade)
            {
                maxForwardSpeed = 5f;
                maxBackwardSpeed = -2f;
                rotationSpeed = 2f;
            }
            else
            {
                maxForwardSpeed = 3f;
                maxBackwardSpeed = -1f;
                rotationSpeed = 1f;
            }
        }

        ChangeLocation();    // Moving.
        ChangeRotation();    // Rotation.
    }

    void ChangeLocation() // The function determines the movement forward and backward
    {
        bool stopped = false;
        if (moveVertical == 0)
        {
            moveChange = false;
            stopped = true;

            if (!engineAudioSource.isPlaying)
            {
                engineAudioSource.clip = engineIdle;
                engineAudioSource.Play();
            }
        }

        moveVertical = Input.GetAxis("Vertical"); // Extract vertical axis information

        if (moveVertical != 0)
        {
            moveChange = true;
            if (stopped)
            {
                engineAudioSource.clip = engineStart;
                engineAudioSource.Play();
            }
            else if (!engineAudioSource.isPlaying || engineAudioSource.clip == engineIdle)
            {
                engineAudioSource.clip = engineSteady;
                engineAudioSource.Play();
            }
        }

        if (moveVertical == 0 && moveChange)
        {
            engineAudioSource.Stop();
            engineAudioSource.clip = engineEnd;
            engineAudioSource.Play();
        }
        

        movementDirection += moveVertical * speed; // Determine the direction of movement, 
                                                   //  multiplying the number extracted from the control by the speed factor

        if (Input.GetAxis("Jump") > 0) // If pressed the spacebar.
        {
            movementDirection *= 0.9f; // Then we reduce the value of the direction of movement by 10%, 
                                       //  which will lead to a slowdown, and if it is held down, it will stop

            if (Mathf.Abs(movementDirection) < 0.2)
            {
                Stop();
            }
        }

        /* We limit the maximum and minimum value of the direction of movement. */
        if (movementDirection > maxForwardSpeed)
        {
            movementDirection = maxForwardSpeed;
        }
        if (movementDirection < maxBackwardSpeed)
        {
            movementDirection = maxBackwardSpeed;
        }

        /* We change the speed of the rigidbody, which gives the engine a command to move the object.
            For this purpose, the position of the object along the Y axis is inverted and multiplied by the direction of motion. */
        myRigidbody.velocity = transform.up * movementDirection + -transform.right * StrafeMovement();
    }

    float StrafeMovement()
    {

        strafing = false;
        if (Input.GetKey("q"))
        {
            strafing = true;

            if (!engineAudioSource.isPlaying || engineAudioSource.clip == engineIdle)
            {
                engineAudioSource.clip = engineSteady;
                engineAudioSource.Play();
            }

            strafeMovement += speed / 4;
        }
        else if (Input.GetKey("e"))
        {
            strafing = true;

            if (!engineAudioSource.isPlaying || engineAudioSource.clip == engineIdle)
            {
                engineAudioSource.clip = engineSteady;
                engineAudioSource.Play();
            }

            strafeMovement -= speed / 4;
        }
        
        if (!strafing && !rotating && !moveChange && engineAudioSource.clip == engineSteady)
        {
            engineAudioSource.Stop();
            engineAudioSource.clip = engineEnd;
            engineAudioSource.Play();
        }
            

        if (strafeMovement > maxForwardSpeed/4)
        {
            strafeMovement = maxForwardSpeed/4;
        }
        if (strafeMovement < -maxForwardSpeed/4)
        {
            strafeMovement = -maxForwardSpeed/4;
        }

        if (Input.GetAxis("Jump") > 0) // If pressed the spacebar
        {
            strafeMovement *= 0.9f;    // Then we reduce the value of the direction of movement by 10%, 
                                       //  which will lead to a slowdown, and if it is held down, it will stop

            if (Mathf.Abs(strafeMovement) < 0.2)
            {
                strafeMovement = 0;
            }
        }

        return strafeMovement;
    }

    public void Stop()
    {
        movementDirection = 0;
        rotationDirection = 0;
        strafeMovement = 0;
        myRigidbody.velocity = new Vector2(0, 0);
    }

    void ChangeRotation()                                   // The function determines the turns of the object
    {
        rotating = false;

        if (moveHorizontal != 0)
            rotating = true;

        moveHorizontal = Input.GetAxis("Horizontal"); // Extract horizontal axis information

        if (moveHorizontal != 0)
        {
            if (!engineAudioSource.isPlaying || engineAudioSource.clip == engineIdle)
            {
                engineAudioSource.clip = engineSteady;
                engineAudioSource.Play();
            }
        }
        /*else if(!rotating && !strafing && engineAudioSource.clip == engineSteady)
        {
            engineAudioSource.clip = engineEnd;
            engineAudioSource.Play();
        }*/

        rotationDirection = moveHorizontal * rotationSpeed; // Determine the direction of rotation, 
                                                            //  multiplying the number extracted from the control by the rotation speed factor

            transform.Rotate(0, 0, -rotationDirection);      // Rotate the object according to the direction of rotation
    }

    void OnCollisionEnter2D(Collision2D collision) // It is executed when the object's collider is in contact with another collider
    {
        GameObject.Find("SpaceShip").GetComponent<GameController>().HitPlayer(1); // Reduce HP  

        if (collision.gameObject.tag == "AsteroidBelt")
        {
            if (!gameObject.GetComponent<GameController>().asteroidShieldOpen)
            GameObject.Find("SpaceShip").GetComponent<GameController>().HitPlayer(100); // Reduce HP  
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // It is executed when the object's collider is in contact with another collider marked "Trigger"
    {
        if(collision.tag == "Planet" || collision.tag == "Living Planet") // If with the planet (including live)
        {
            target = collision.transform;                                 // The coordinates of what we encountered
            transform.SetParent(target);                                  // Set the object we encountered as the parent
            myRigidbody.AddRelativeForce(Vector3.forward * 100);          // Direct the force on the ship to attract it to the parent element
        }

        if(collision.tag == "Living Planet") // If with a living planet
        {
            foodReplenishment = true;        // Then put a flag that you can replenish food stocks
        }
    }

    void OnTriggerExit2D(Collider2D collision) // It is executed when the object's collider stops contacting the collider marked as “Trigger”
    {
        if (collision.tag == "Planet")         // If with a planet
        {
            target = null;                     // Clear
            this.transform.parent = null;      // Breaking the link with the parent element
        }

        if (collision.tag == "Living Planet")  // If with a living planet
        {
            foodReplenishment = false;         // Then remove the flag that you can replenish food supplies
            target = null;                     // Clear
            this.transform.parent = null;      // Breaking the link with the parent element
        }

        if (collision.tag == "AsteroidBelt")         
        {
            target = null;                     // Clear
            this.transform.parent = null;      // Breaking the link with the parent element
        }
    }

    [ContextMenu("Upgrade Engine")]
    public void MakeShipFaster()
    {
        speedUpgrade = true;
        maxBackwardSpeed = maxBackwardSpeed * 2;
        maxForwardSpeed = maxForwardSpeed * 2;
        rotationSpeed = rotationSpeed * 2;

        engineStart = Resources.Load<AudioClip>("Sounds/engine/pohon 2 start");
        engineSteady = Resources.Load<AudioClip>("Sounds/engine/pohon 2 steady");
        engineEnd = Resources.Load<AudioClip>("Sounds/engine/pohon 2 end");
    }
}
