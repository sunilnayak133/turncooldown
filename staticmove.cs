using UnityEngine;
using System.Collections;

public class staticmove : MonoBehaviour
{ 
    [SerializeField]
    private float baseThrust;

    [SerializeField]
    private float maxThrust;

    [SerializeField]
    private float thrustPerKick;

    private float thrust;


    // time it takes to reach max angle delta
    [SerializeField]
    private float lerpTime;

    // current time (always <= lerpTime and >= 0)
    [SerializeField]
    private float currentTime;

    // cooldown time for after turning (smooth stop)
    [SerializeField]
    private float turnSlowDownTime;

    // current angle delta
    [SerializeField]
    private float curAng;

    // 0 - not turning, 1 - turning left, 2 - turning right
    [SerializeField]
    private int isTurning;

    // max angle delta
    [SerializeField]
    private float maxAng;
    
    //to see if gameObject is moving forward or not
    private bool isMoving;

    private Rigidbody rigb;

    private Vector3 front;


    // Use this for initialization
    void Start()
    {
        
        isMoving = false;
        front = transform.forward;
        rigb = gameObject.GetComponent<Rigidbody>();
        thrust = baseThrust;
        isTurning = 0;
        
    }

    void FixedUpdate()
    {
        front = transform.forward;

        // Stop motion
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) && isMoving)
        {
            isMoving = false;
            thrust = 0;
        }

        // Forward motion
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!startTitle.started)
            {
                startTitle.StartGame();
            }

            // Start with initial thrust
            if(thrust == 0)
            {
                thrust = baseThrust;
            }

           
            isMoving = true;
            // Increase speed with every up arrow
            thrust += thrustPerKick;
            thrust = Mathf.Clamp(thrust, baseThrust, maxThrust);
        }

        if (isMoving)
        {
            rigb.velocity = thrust * front;
        }
        else
        {
            rigb.velocity = new Vector3(0, 0, 0);
        }

        // Left motion - detect left is pressed down
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            //print("Left Press");
            currentTime = 0f;
        }

        // Left motion - actually turn left
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            //print("Left");
            isTurning = 1;
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime) 
            {
                currentTime = lerpTime;
            }
            curAng = maxAng * (currentTime / lerpTime);
            transform.Rotate(new Vector3(0, 1, 0), -curAng);
            
        } 
        
        else 
        {
            if (isTurning == 1 && curAng > 0)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= turnSlowDownTime)
                {
                    currentTime = turnSlowDownTime;
                }
                curAng -= curAng * (currentTime / turnSlowDownTime);
                transform.Rotate(new Vector3(0, 1, 0), -curAng);
                //print("Cooldown Left");
            }

            if (curAng < 0.001)
            {
                isTurning = 0;
                //print("Not Turning Left");
                
            }
            
        }

        // Right motion - detect right is pressed down
        if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            //print("Right Press");
            currentTime = 0f;
        }

        // Right motion - actually turn right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //print("Right");
            isTurning = 2;
            currentTime += Time.deltaTime;
            if (currentTime >= lerpTime) 
            {
                currentTime = lerpTime;
            }
            curAng = maxAng * (currentTime / lerpTime);
            transform.Rotate(new Vector3(0, 1, 0), curAng);
            

        }

        else 
        {
            if (isTurning == 2 && curAng > 0)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= turnSlowDownTime)
                {
                    currentTime = turnSlowDownTime;
                }
                curAng -= curAng * (currentTime / turnSlowDownTime);
                transform.Rotate(new Vector3(0, 1, 0), curAng);
                //print("Cooldown Right");
            }

            if (curAng < 0.001)
            {
                isTurning = 0;
                //print("Not Turning Right");
                
            }
            
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (isTurning == 2)
                currentTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (isTurning == 1)
                currentTime = 0;

        }


    }

    
}
