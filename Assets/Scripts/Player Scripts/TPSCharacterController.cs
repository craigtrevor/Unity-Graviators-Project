﻿using UnityEngine;
using System.Collections;

public class TPSCharacterController : MonoBehaviour {

    //const float SPEED_INITIAL = 10f;
    //const float SENSITIVITY_INITIAL = 2f;

    //const float JUMP_MULTIPLIER_INITIAL = 0.75f;

    const float GRAVITY = -15f;

    public int EP; // enhancement points

    public float attack;
    public float health; 
    public float speed;

    public float sensitivity;

    public float jumpMultiplier;
    public float jumpForce;

    public bool isGrounded;

    float moveFB;
    float moveLR;

    float rotX;
    float rotY;
    float vertVelocity;

    GameObject cameraPivot;
    GameObject gravityAxis;
    GameObject rotationBlock;
    GameObject vertArrows;

    GravityAxisScript gravityAxisScript;
    RotationBlockScript rotationBlockScript;
    CharacterController controller;

    // Use this for initialization
    void Start() {

        //speed = SPEED_INITIAL;
        //sensitivity = SENSITIVITY_INITIAL;

        //jumpMultiplier = JUMP_MULTIPLIER_INITIAL;

        attack = 5f;
        health = 20f;
        speed = 10f;
        sensitivity = 2f;
        jumpMultiplier = 0.75f;

        isGrounded = false;

        cameraPivot = GameObject.Find("CameraPivot");
        gravityAxis = GameObject.Find("GravityAxis");
        rotationBlock = GameObject.Find("RotationBlock");
        vertArrows = GameObject.Find("VertArrows");

        gravityAxisScript = gravityAxis.GetComponent<GravityAxisScript>();
        rotationBlockScript = rotationBlock.GetComponent<RotationBlockScript>();

        controller = GetComponent<CharacterController>(); //Assign controller

        Cursor.lockState = CursorLockMode.Locked; //Lock cursor

    }

    // Update is called once per frame
    void Update() {

        //If shift is pressed (gravity selection)
        if (Input.GetButton("Crouch")) {

            gravityAxisScript.shiftPressed = true; ; //shiftPressed true

        } else {
            gravityAxisScript.shiftPressed = false; ; //shiftPressed false

            if (isGrounded == true) { //If on ground

                if (Input.GetButtonDown("Jump")) { //If space pressed
                    jumpForce = jumpMultiplier * -GRAVITY;
                    vertVelocity += jumpForce; //Jump
                }

            } //End if on ground

        } //End if shift

        //Movement
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;
                
        //Camera look
        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotY = Mathf.Clamp(rotY, -60f, 60f);

        //Calculate movement
        Vector3 movement = new Vector3(moveLR, vertVelocity, moveFB);

        if (!gravityAxisScript.gravityChanging) {
            transform.Rotate(0, rotX, 0);
            cameraPivot.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
        }

        //Move player
        movement = transform.rotation * movement; //Relative to rotation
        controller.Move(movement * Time.deltaTime);

        //Update vertArrow rotation
        vertArrows.transform.rotation = transform.rotation; //Set vertArrows to player rotation

        rotationBlockScript.UpdatePosition(); //Update rotation block position
    }
    
    //FixedUpdate
    void FixedUpdate() {

        //Raycast to detect if grounded
        if (Physics.Raycast(transform.position, -transform.up, 1.09f * controller.transform.lossyScale.y)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        //If on the ground
        if (isGrounded == false) {
            vertVelocity += GRAVITY * Time.deltaTime; //Look how I am defining gravity
        } else if (!Input.GetButtonDown("Jump")) { //If on the ground and not jumping
            vertVelocity = 0f; //Reset vertVelocity
        }
    }
}