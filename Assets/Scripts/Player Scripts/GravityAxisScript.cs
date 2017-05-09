using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityAxisScript : MonoBehaviour {

    const int GRAVITY_MAX = 1000;
    const int GRAVITY_COST = 250;

    string gravity;
    public bool gravityChanging;
    int gravityCharge;
    int quadrant;
    Quaternion playerRotation;

    public bool shiftPressed;

    GameObject arrowUp;
    GameObject arrowRight;
    GameObject arrowLeft;
    GameObject arrowForward;
    GameObject arrowBackward;

    GameObject vertArrows;
    GameObject player;
    GameObject gravityBlock;
    GameObject rotationBlock;

    GameObject textUp;
    GameObject textRight;
    GameObject textLeft;
    GameObject textForward;
    GameObject textBackward;

    GameObject gravityCamera;
    GameObject gravityCameraRing;
    GameObject UIGravityCharge;

    GameObject ring;
    GameObject ringPivot;
    GravityRingRotator gravityRingRotator;

    public Material toY;
    public Material toNY;
    public Material toX;
    public Material toNX;
    public Material toZ;
    public Material toNZ;
    public Material noCharge;

    // Use this for initialization
    void Start() {

        gravity = "-y";
        gravityChanging = false;
        gravityCharge = GRAVITY_MAX;
        quadrant = 1;
        playerRotation = transform.rotation;

        shiftPressed = false;

        arrowUp = GameObject.Find("ArrowUp");
        arrowRight = GameObject.Find("ArrowRight");
        arrowLeft = GameObject.Find("ArrowLeft");
        arrowForward = GameObject.Find("ArrowForward");
        arrowBackward = GameObject.Find("ArrowBackward");

        vertArrows = GameObject.Find("VertArrows");
        player = GameObject.Find("PlayerModel");
        gravityBlock = GameObject.Find("GravityBlock");
        rotationBlock = GameObject.Find("RotationBlock");

        textUp = GameObject.Find("TextUp");
        textRight = GameObject.Find("TextRight");
        textLeft = GameObject.Find("TextLeft");
        textForward = GameObject.Find("TextForward");
        textBackward = GameObject.Find("TextBackward");

        gravityCamera = GameObject.Find("GravityCamera");
    
        gravityCameraRing = GameObject.Find("GravityCameraRing");
        gravityCameraRing.GetComponent<Camera>().enabled = false;

        ring = GameObject.Find("Ring");
        ring.GetComponent<MeshRenderer>().enabled = false;
        ringPivot = GameObject.Find("RingPivot");
        gravityRingRotator = ringPivot.GetComponent<GravityRingRotator>();

        UIGravityCharge = GameObject.Find("UIGravityCharge");
        UIGravityCharge.GetComponent<Text>().text = gravityCharge.ToString();
        UIGravityCharge.GetComponent<Text>().color = Color.white;

    }

    // Update is called once per frame
    void Update() {

        if (gravityChanging) { //If gravity is changing
            

            //player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, gravityBlock.transform.rotation, 10); //Rotate player toward GravityBlock

            playerRotation = Quaternion.Lerp(player.transform.rotation, gravityBlock.transform.rotation, Time.deltaTime * 10);

            player.transform.rotation = playerRotation;

            if (Mathf.Abs(player.transform.eulerAngles.x - gravityBlock.transform.eulerAngles.x) <= 1 &&
                Mathf.Abs(player.transform.eulerAngles.y - gravityBlock.transform.eulerAngles.y) <= 1 &&
                Mathf.Abs(player.transform.eulerAngles.z - gravityBlock.transform.eulerAngles.z) <= 1) { //If fully rotated
                player.transform.rotation = gravityBlock.transform.rotation;
                gravityChanging = false;
            }

            //gravityCamera.GetComponent<Camera>().enabled = false; //Hide gravity axis
            //gravityCameraRing.GetComponent<Camera>().enabled = false;
            //ring.GetComponent<MeshRenderer>().enabled = false;

        } else { //If gravity is not changing

            if (shiftPressed) { //If shift pressed

                gravityCamera.GetComponent<Camera>().enabled = true; //Show gravity axis
                gravityCameraRing.GetComponent<Camera>().enabled = true;
                ring.GetComponent<MeshRenderer>().enabled = true;

                if (gravityCharge >= GRAVITY_COST && (Input.GetButtonDown("Jump") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))) { //If enough gravity charge and button down

                    ChangeGravity();
                }

                if (!(Input.GetButton("Jump") || Input.GetButton("Vertical") || Input.GetButton("Horizontal"))) {
                    gravityRingRotator.direction = -1;
                }

            } else { //If shift not pressed

                gravityCamera.GetComponent<Camera>().enabled = false; //Hide gravity axis
                gravityCameraRing.GetComponent<Camera>().enabled = false;
                ring.GetComponent<MeshRenderer>().enabled = false;

            } //End if shift pressed

        } //End if gravity is changing        

        gravityCharge = Mathf.Min(GRAVITY_MAX, gravityCharge + 1); //Recharge gravity
        UIGravityCharge.GetComponent<Text>().text = gravityCharge.ToString();

    } //End Update

    //Fixed update
    private void FixedUpdate() {

        RotateRotationBlock();
        SetQuadrant();
        ColourAxis(gravity, gravityCharge);
        UpdateText();
        SetGravity();
        gravityRingRotator.SetKeys(quadrant);

    }

    //Colours the arrows on the gravity axis
    void ColourAxis(string gravity, int gravityCharge) {

        if (gravityCharge < GRAVITY_COST) { //If don't have gravity charge

            arrowUp.GetComponent<ArrowScript>().ChangeColour(noCharge);
            arrowForward.GetComponent<ArrowScript>().ChangeColour(noCharge);
            arrowBackward.GetComponent<ArrowScript>().ChangeColour(noCharge);
            arrowRight.GetComponent<ArrowScript>().ChangeColour(noCharge);
            arrowLeft.GetComponent<ArrowScript>().ChangeColour(noCharge);

            UIGravityCharge.GetComponent<Text>().color = Color.grey;

        } else { //If have gravity charge

            UIGravityCharge.GetComponent<Text>().color = Color.white;

            if (gravity == "-y") { //-y Gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(toY);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(toZ);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(toNZ);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(toX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(toNX);

            } else if (gravity == "y") { //y gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(toNY);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(toZ);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(toNZ);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(toNX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(toX);

            } else if (gravity == "z") { //z gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(toNZ);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(toY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(toNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(toX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(toNX);

            } else if (gravity == "-z") { //-z gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(toZ);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(toY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(toNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(toNX);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(toX);

            } else if (gravity == "x") { //x gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(toNX);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(toY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(toNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(toNZ);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(toZ);

            } else if (gravity == "-x") { //-x gravity

                arrowUp.GetComponent<ArrowScript>().ChangeColour(toX);
                arrowForward.GetComponent<ArrowScript>().ChangeColour(toY);
                arrowBackward.GetComponent<ArrowScript>().ChangeColour(toNY);
                arrowRight.GetComponent<ArrowScript>().ChangeColour(toZ);
                arrowLeft.GetComponent<ArrowScript>().ChangeColour(toNZ);

            } //End if gravity
        } //End if gravity charge
    }

    //Updates the button text on the gravity axis
    void UpdateText() {

        //Get button characters
        string forwardButton = "W";
        string rightButton = "D";
        string backwardButton = "S";
        string leftButton = "A";

        textUp.GetComponent<TextMesh>().text = "Space";

        if (quadrant == 1) { //Forward quadrant

            textForward.GetComponent<TextMesh>().text = forwardButton;
            textRight.GetComponent<TextMesh>().text = rightButton;
            textBackward.GetComponent<TextMesh>().text = backwardButton;
            textLeft.GetComponent<TextMesh>().text = leftButton;

        } else if (quadrant == 2) { //Right quadrant

            textForward.GetComponent<TextMesh>().text = leftButton;
            textRight.GetComponent<TextMesh>().text = forwardButton;
            textBackward.GetComponent<TextMesh>().text = rightButton;
            textLeft.GetComponent<TextMesh>().text = backwardButton;

        } else if (quadrant == 3) { //Backward quadrant

            textForward.GetComponent<TextMesh>().text = backwardButton;
            textRight.GetComponent<TextMesh>().text = leftButton;
            textBackward.GetComponent<TextMesh>().text = forwardButton;
            textLeft.GetComponent<TextMesh>().text = rightButton;

        } else if (quadrant == 4) { //Left quadrant

            textForward.GetComponent<TextMesh>().text = rightButton;
            textRight.GetComponent<TextMesh>().text = backwardButton;
            textBackward.GetComponent<TextMesh>().text = leftButton;
            textLeft.GetComponent<TextMesh>().text = forwardButton;

        }
    }

    //Change gravity upwards
    void GravityUp() {

        gravityBlock.transform.rotation = player.transform.rotation;

        if (quadrant == 1 || quadrant == 3) { //If facing forward/backward 

            if (gravity == "-y" || gravity == "y") { //If on y gravities
                gravityBlock.transform.Rotate(0, 0, 180, Space.World);
            } else { //If on z or x gravities
                gravityBlock.transform.Rotate(0, 180, 0, Space.World);
            }

        } else { //If facing left/right 

            if (gravity == "-x" || gravity == "x") { //If on x gravities
                gravityBlock.transform.Rotate(0, 0, 180, Space.World);
            } else { //If on y or z gravities
                gravityBlock.transform.Rotate(180, 0, 0, Space.World);
            }

        } //End if quadrant

    } //End GravityUp

    //Change gravity horizontally
    void GravityHor(int turnMod) {

        //Wrap quadrant < 0 and > 4

        quadrant += turnMod - 1;

        if (quadrant > 4) {
            quadrant -= 4;
        } else if (quadrant < 1) {
            quadrant += 4;
        }

        if (quadrant == 1) { //Forward quadrant
            GravityForward();
        } else if (quadrant == 2) { //Right quadrant
            GravityRight();
        } else if (quadrant == 3) { //Backward quadrant
            GravityBackward();
        } else if (quadrant == 4) { //Left quadrant
            GravityLeft();
        }
    }

    //Change gravity forward
    void GravityForward() {

        if (gravity == "-y" || gravity == "z") { //If -y or z gravities
            gravityBlock.transform.Rotate(-90, 0, 0, Space.World);
        } else if (gravity == "y" || gravity == "-z") { //If y or -z gravities
            gravityBlock.transform.Rotate(90, 0, 0, Space.World);
        } else if (gravity == "x") { //If x gravity
            gravityBlock.transform.Rotate(0, 0, 90, Space.World);
        } else if (gravity == "-x") { //If -x gravity
            gravityBlock.transform.Rotate(0, 0, -90, Space.World);
        }

    }

    //Change gravity backward
    void GravityBackward() {

        if (gravity == "-y" || gravity == "z") { //If -y or z gravities
            gravityBlock.transform.Rotate(90, 0, 0, Space.World);
        } else if (gravity == "y" || gravity == "-z") { //If y or -z gravities
            gravityBlock.transform.Rotate(-90, 0, 0, Space.World);
        } else if (gravity == "x") { //If x gravity
            gravityBlock.transform.Rotate(0, 0, -90, Space.World);
        } else if (gravity == "-x") { //If -x gravity
            gravityBlock.transform.Rotate(0, 0, 90, Space.World);
        }

    }

    //Change gravity right
    void GravityRight() {

        if (gravity == "-y" || gravity == "y") { //If y gravities
            gravityBlock.transform.Rotate(0, 0, 90, Space.World);
        } else { //If x or z gravities
            gravityBlock.transform.Rotate(0, 90, 0, Space.World);
        }

    }

    //Change gravity left
    void GravityLeft() {

        if (gravity == "-y" || gravity == "y") { //If y gravities
            gravityBlock.transform.Rotate(0, 0, -90, Space.World);
        } else { //If x or z gravities
            gravityBlock.transform.Rotate(0, -90, 0, Space.World);
        }

    }

    //Overall change gravity method
    void ChangeGravity() {

        gravityBlock.transform.rotation = player.transform.rotation; //Initialise gravity block rotation

        if (Input.GetButtonDown("Jump")) { //Spacebar

            GravityUp();

            gravityRingRotator.direction = 0;

        } else if (Input.GetButtonDown("Vertical")) { //Vertical buttons

            if (Input.GetAxis("Vertical") > 0) { //Forward button
                GravityHor(1);
                gravityRingRotator.direction = 1;
            } else { //Backward button
                GravityHor(3);
                gravityRingRotator.direction = 3;
            }

        } else if (Input.GetButtonDown("Horizontal")) { //Horizontal buttons

            if (Input.GetAxis("Horizontal") > 0) { //Right button
                GravityHor(2);
                gravityRingRotator.direction = 2;
            } else { //Left button
                GravityHor(4);
                gravityRingRotator.direction = 4;
            }

        } //End if (button)

        gravityChanging = true; //Gravity changing is true

        gravityCharge -= GRAVITY_COST; //Decrease gravity charge

    } //End ChangeGravity()

    //Set gravity string reading from gravityBlock
    void SetGravity() {

        int gravBlockUpX = Mathf.RoundToInt(gravityBlock.transform.up.x);
        int gravBlockUpY = Mathf.RoundToInt(gravityBlock.transform.up.y);
        int gravBlockUpZ = Mathf.RoundToInt(gravityBlock.transform.up.z);

        if (gravBlockUpY == Vector3.up.y) {
            gravity = "-y";
        } else if (gravBlockUpY == Vector3.down.y) {
            gravity = "y";
        } else if (gravBlockUpZ == Vector3.back.z) {
            gravity = "z";
        } else if (gravBlockUpX == Vector3.left.x) {
            gravity = "x";
        } else if (gravBlockUpZ == Vector3.forward.z) {
            gravity = "-z";
        } else if (gravBlockUpX == Vector3.right.x) {
            gravity = "-x";
        } else {
            gravity = null;
        }

    }

    //Set quadrant (yRot)
    void SetQuadrant() {

        float yRot = vertArrows.GetComponent<VertArrowsScript>().yRot; //relative y-rotation

        if (315 < yRot || yRot <= 45) { //Forward quadrant
            quadrant = 1;
        } else if (45 < yRot && yRot <= 135) { //Right quadrant
            quadrant = 2;
        } else if (135 < yRot && yRot <= 225) { //Backward quadrant
            quadrant = 3;
        } else if (225 < yRot && yRot <= 315) { //Left quadrant
            quadrant = 4;
        }
    }

    //Rotate RotationBlock
    void RotateRotationBlock() {

        if (gravity == "-y") {
            rotationBlock.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (gravity == "y") {
            rotationBlock.transform.rotation = Quaternion.Euler(0, 0, 180);
        } else if (gravity == "z") {
            rotationBlock.transform.rotation = Quaternion.Euler(-90, 0, 0);
        } else if (gravity == "-z") {
            rotationBlock.transform.rotation = Quaternion.Euler(-90, -180, 0);
        } else if (gravity == "x") {
            rotationBlock.transform.rotation = Quaternion.Euler(-90, 90, 0);
        } else if (gravity == "-x") {
            rotationBlock.transform.rotation = Quaternion.Euler(-90, -90, 0);
        }

    }

}
