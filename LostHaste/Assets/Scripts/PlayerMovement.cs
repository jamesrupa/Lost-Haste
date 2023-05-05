using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // variables
    // all can later be changed to preferance in Unity UI
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f *2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    
    bool isGrounded;

    private Vector3 lastPosition = new Vector3(0, 0, 0);
    public bool isMoving;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checks to see if we hit the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        // gets controls from Unity Settings default is WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //check if player is on ground
        //then jump
        if(Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(lastPosition != gameObject.transform.position) {
            isMoving = true;
            SoundManager.Instance.playSound(SoundManager.Instance.grassWalkSound);
        } else {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }

        lastPosition = gameObject.transform.position;
    }
}
