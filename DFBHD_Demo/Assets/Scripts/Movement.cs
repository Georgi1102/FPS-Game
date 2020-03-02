using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Variables
    #region Variables 
    // reference to the player  
    public CharacterController playerController;
  //  private Animator anim;
    public float healt = 20f;

    //reference to the ground
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    //jump variables
    private float jumpHeigt = 1.5f;

    //movement vriables
    private float moveSpeed = 4f;
    private float walkingSpeed;
    private float sprintSpeed = 7f;
    private float gravity = -9.81f;
    Vector3 velocity;
    #endregion

   //Update called every frame
    #region MethodUpdate
    void Update()
    {
        //checking if we are on the ground and reseting the Y velocity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
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
                moveSpeed -= 3;
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

    //Custom logic for the player
    #region CustomMethods
    public void Run()
    {
        //lets check if we are pressing shift 
        //you probably can not run backwards
        //lets check if we move forward and if you are on the ground 
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && isGrounded)
        {           
            walkingSpeed = sprintSpeed;
        }
        else
        {
            walkingSpeed = moveSpeed;
        }

    }

    public bool Jump()
    {
        //lets check if we are pressing space 
        if (Input.GetKey(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeigt * -2f * gravity);//adding gravity pull down 
            return true;
        }
        return false;
    }

    public bool Crouch()
    {
        //lets check if we are pressing C 
        if (Input.GetKey(KeyCode.C))
        {
            playerController.height = 1.0f;
            velocity.y += (gravity * Time.deltaTime) * 3f; // adding force to crowch for faster crouch     
            velocity.y *= 10f;
           
            playerController.Move(velocity * Time.deltaTime);//moving the body downwards
            return true;
        }
        else
        {
            playerController.height = 1.9f;          
            return false;
        }

    }
    #endregion

}
