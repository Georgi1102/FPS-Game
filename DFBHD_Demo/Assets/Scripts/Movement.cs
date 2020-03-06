using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Variables
    #region Variables 
    // reference to the player  
    public CharacterController playerController;
    public float healt = 20f;

    private float crouchTime = 0.5f;

    //reference to the ground
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;
    public Animation jump;   
    //jump variables
    private float jumpHeigt = 1.5f;

    //movement vriables
    private float moveSpeed = 4f;
    private float walkingSpeed;
    private float sprintSpeed = 7f;
    private float crouchSpeed = 1.5f;
    private float gravity = -9.81f;
    Vector3 velocity;
    private float rotateSpeed = 3f;
    #endregion
    private float speed = 0.1f;

    //Update called every frame   
    void Update()
    {    
        MovementFinalChecks();     
    }
    

    //Custom logic for the player
    #region CustomMethods
    public bool Run()
    {
        //lets check if we are pressing shift 
        //you probably can not run backwards
        //lets check if we move forward and if you are on the ground 
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && isGrounded && !Crouch())
        {           
            walkingSpeed = sprintSpeed;
            return true;
        }
        else
        {
            walkingSpeed = moveSpeed;
            return false;
        }

    }

    public bool Jump()
    {
        //lets check if we are pressing space 
        if (Input.GetKeyUp(KeyCode.Space) && !Crouch())
        {
            velocity.y = Mathf.Sqrt(jumpHeigt * -2f * gravity);//adding gravity pull down 
            jump.Play("JumpAnimation");
            
            return true;
        }
        return false;
    }

    private void Tilt()
    {
        
        if (Input.GetKey(KeyCode.Q))
        {
           playerController.transform.rotation *= Quaternion.Euler(0, 0, 20 * Time.deltaTime * 10);

            Quaternion t = playerController.transform.rotation * Quaternion.Euler(0, 0, 20 * Time.deltaTime * 10);
            if (t.eulerAngles.z > 20)
            {
                t = Quaternion.Euler(t.eulerAngles.x, t.eulerAngles.y, 20);
            }

            playerController.transform.rotation = t;

        }
        else if (Input.GetKey(KeyCode.E))
        {
            
            playerController.transform.rotation *= Quaternion.Euler(0, 0, -20 * Time.deltaTime);

            Quaternion t = playerController.transform.rotation * Quaternion.Euler(0, 0, -20 * Time.deltaTime);
            if (t.eulerAngles.z > 20)
            {
                t = Quaternion.Euler(t.eulerAngles.x, t.eulerAngles.y, -20);
            }

            playerController.transform.rotation = t;
        }

        else
        {
            playerController.transform.rotation *= Quaternion.Euler(0, 0, 0 * Time.deltaTime * 10);

            Quaternion t = playerController.transform.rotation * Quaternion.Euler(0, 0, 0 * Time.deltaTime * 10);
            if (t.eulerAngles.z > 0)
            {
                t = Quaternion.Euler(t.eulerAngles.x, t.eulerAngles.y, 0);
            }

            playerController.transform.rotation = t;

        }
    }



    public bool Crouch()
    {
        //lets check if we are pressing C 
        if (Input.GetKey(KeyCode.C))
        {           
            velocity.y += (gravity * Time.deltaTime) * 3f; // adding force to crowch for faster crouch     
            velocity.y *= 10f;          
            playerController.height = Mathf.MoveTowards(playerController.height, 1.0f, Time.deltaTime / crouchTime);           
            return true;
        }
        else
        {         
            playerController.height = Mathf.MoveTowards(playerController.height, 1.9f, Time.deltaTime / crouchTime);        
            return false;
        }

    }

   
    private void MovementFinalChecks()
    {
        //checking if we are on the ground and reseting the Y velocity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Tilt();

        if (isGrounded && velocity.y < 0)
        {          
            velocity.y = 0;
        }
        if (isGrounded)
        {

           
            //you can not move and sprint while in the air         
            if (Jump())
            {
                moveSpeed -= 1;
            }
            else if (Crouch())
            {

                walkingSpeed = crouchSpeed;
            }
            else
            {
                moveSpeed = 4f;
                Run();
            }
           
        }

        // getting the horizontal and vertical  axis
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // simple movement
        Vector3 move = (transform.right * moveX) + (transform.forward * moveY);
        playerController.Move(move * walkingSpeed * Time.deltaTime);

        //Run();

        //simple gravity TIME IS SQUARED due to the formula 
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }
    #endregion

}
