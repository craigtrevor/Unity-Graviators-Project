using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GravityAxisDisplayScript controls all the visual aspects of the gravity axis including text, colour and whether or not it is visible.
//It also controls the UI that displays the current gravity charge.
public class GravityAxisDisplayScript : MonoBehaviour {
    
    //Gravity variables
    private string gravity;
    private bool gravitySwitching;

    //Gravity charge variables
    private int gravityCharge;
    private bool haveCharge;

    //Control variables
    private bool shiftPressed;
    private int quadrant;

    //Text GameObjects
    public GameObject textUp;
    public GameObject textForward;
    public GameObject textRight;
    public GameObject textBackward;
    public GameObject textLeft;

    //Arrow GameObjects
    public GameObject arrowUp;
    public GameObject arrowRight;
    public GameObject arrowLeft;
    public GameObject arrowForward;
    public GameObject arrowBackward;

    //Arrow Materials
    public Material matY;
    public Material matNY;
    public Material matX;
    public Material matNX;
    public Material matZ;
    public Material matNZ;
    public Material matNoCharge;

    //Ring Visibility Objects
    public GameObject gravityCamera;
    public GameObject gravityCameraFront        ;
    public GameObject ring;

    //UI Objects/Components
    public GameObject UIGravityCharge;

    private Text UIGravityChargeText;


    //Start()
    private void Start() {

        //Get GravityChargeUI Text
        //UIGravityChargeText = GameObject.Find("UIGravityCharge").GetComponent<Text>();
        //UIGravityChargeText = UIGravityCharge.GetComponent<Text>();


    } //End Start()

    //Update() is called once per frame
    private void Update() {

        DisplayAxis();
        ColourAxis();
        UpdateText();
        UpdateCharge();

    } //End Update()

    //FixedUpdate() is called once per fixed framerate frame
    private void FixedUpdate() {

    } //End FixedUpdate()


    //DisplayAxis() makes the axis in/visisble if gravity is switching and if shift is being pressed
    private void DisplayAxis() {

        //Check gravityChanging && shiftPressed  
        if (shiftPressed && !gravitySwitching) { //If shift is pressed and not switching gravity
            //Hide axis
            gravityCamera.GetComponent<Camera>().enabled = true;
            gravityCameraFront.GetComponent<Camera>().enabled = true;
            ring.GetComponent<MeshRenderer>().enabled = true;
        } else {
            //Hide axis
            gravityCamera.GetComponent<Camera>().enabled = false;
            gravityCameraFront.GetComponent<Camera>().enabled = false;
            ring.GetComponent<MeshRenderer>().enabled = false;
        } //End if(shiftPressed && !gravityChanging)

    } //End DisplayAxis()

    //ColourAxis() changes the colours of the arrows on the axis according to the current gravity and gravity charge
    private void ColourAxis() {

        //Check haveCharge
        if (haveCharge) { //If player does have gravity charge            

            if (gravity == "-y") { //-y Gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(matY);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(matZ);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(matNZ);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(matX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(matNX);

            } else if (gravity == "y") { //y gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(matNY);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(matZ);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(matNZ);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(matNX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(matX);

            } else if (gravity == "z") { //z gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(matNZ);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(matY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(matNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(matX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(matNX);

            } else if (gravity == "-z") { //-z gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(matZ);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(matY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(matNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(matNX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(matX);

            } else if (gravity == "x") { //x gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(matNX);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(matY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(matNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(matNZ);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(matZ);

            } else if (gravity == "-x") { //-x gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(matX);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(matY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(matNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(matZ);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(matNZ);

            } //End if (gravity)

        } else { //If player doesn't have gravity charge

            arrowUp.GetComponent<ArrowScript>().ChangeColour(matNoCharge);
            arrowForward.GetComponent<ArrowScript>().ChangeColour(matNoCharge);
            arrowBackward.GetComponent<ArrowScript>().ChangeColour(matNoCharge);
            arrowRight.GetComponent<ArrowScript>().ChangeColour(matNoCharge);
            arrowLeft.GetComponent<ArrowScript>().ChangeColour(matNoCharge);

        } //End if (haveCharge)
    } //End ColorAxis()

    //UpdateText() changes the text on the WASD keys of the axis according to player rotation
    private void UpdateText() {

        //Get button characters
        string buttonUp = "Space";
        string buttonForward = "W";
        string buttonRight = "D";
        string buttonBackward = "S";
        string buttonLeft = "A";

        textUp.GetComponent<TextMesh>().text = buttonUp;

        //Check quadrant
        if (quadrant == 1) { //If facing forward
            textForward.GetComponent<TextMesh>().text = buttonForward;
            textRight.GetComponent<TextMesh>().text = buttonRight;
            textBackward.GetComponent<TextMesh>().text = buttonBackward;
            textLeft.GetComponent<TextMesh>().text = buttonLeft;

        } else if (quadrant == 2) { //If facing right
            textForward.GetComponent<TextMesh>().text = buttonLeft;
            textRight.GetComponent<TextMesh>().text = buttonForward;
            textBackward.GetComponent<TextMesh>().text = buttonRight;
            textLeft.GetComponent<TextMesh>().text = buttonBackward;

        } else if (quadrant == 3) { //If facing backward
            textForward.GetComponent<TextMesh>().text = buttonBackward;
            textRight.GetComponent<TextMesh>().text = buttonLeft;
            textBackward.GetComponent<TextMesh>().text = buttonForward;
            textLeft.GetComponent<TextMesh>().text = buttonRight;

        } else if (quadrant == 4) { //If facing left
            textForward.GetComponent<TextMesh>().text = buttonRight;
            textRight.GetComponent<TextMesh>().text = buttonBackward;
            textBackward.GetComponent<TextMesh>().text = buttonLeft;
            textLeft.GetComponent<TextMesh>().text = buttonForward;

        } //End if (quadrant)
    } //End UpdateText()

    //UpdateCharge changes the text on the UI to show current gravity charge and changes colour based on value
    private void UpdateCharge() {

        //Update gravityCharge value on UI
        // UIGravityChargeText.text = gravityCharge.ToString();

        //Check haveCharge
        if (haveCharge) { //If player does have gravity charge
            //UIGravityChargeText.color = Color.white;
        } else { //If player does not have gravity charge
            //UIGravityChargeText.color = Color.grey;
        } //End if (haveCharge)

    } //End UpdateCharge()


    //SetVariables() retrieves important variables, called in GravityAxisScript
    public void SetVariables(string thisGravity, bool thisGravitySwitching, int thisGravityCharge, bool thisHaveCharge, bool thisShiftPressed, int thisQuadrant) {

        //Gravity variables
        gravity = thisGravity;
        gravitySwitching = thisGravitySwitching;

        //Gravity charge variables
        gravityCharge = thisGravityCharge;
        haveCharge = thisHaveCharge;

        //Control variables
        shiftPressed = thisShiftPressed;
        quadrant = thisQuadrant;

    } //End SetVariables()

} //End GravityAxisDisplayScript
