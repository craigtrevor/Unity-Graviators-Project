//THIS IS THE IN-GAME ONE

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//GravityAxisScript controls all the aspects of the gravity axis and gravity switching.
public class GravityAxisScript : MonoBehaviour {

    enum Orientation {
        Right,   //0
        Left,    //1
        Up,      //2
        Down,    //3
        Forward, //4
        Backward //5
    }

    //Gravity charge constants
    private const int GRAVITY_MAX = 10000;
    private const int GRAVITY_COST = 2500;
    private const int RECHARGE_RATE = 10;

    //Gravity variables
    public string gravity;
    public bool gravitySwitching;
    private bool cameraSwitching;

    //Gravity charge variables
    public int gravityCharge;
    private int rechargeRate;
    private bool haveCharge;
    private bool rechargeOn;

    //Control variables
    private bool shiftPressed;
    private float jumpAxis;
    private float verticalAxis;
    private float horizontalAxis;
    private int quadrant;

    //Other Objects/Scripts
    public GameObject controller;
    public GameObject cameraPos;
    public GameObject vertArrows;
    public GameObject gravityBlock;
    public GameObject rotationBlock;

    private PlayerController playerControllerScript;
    //private CameraPosScript cameraPosScript;
    private VertArrowsScript vertArrowsScript;

    private GravityAxisDisplayScript gravityAxisDisplayScript;


    //Start()
    private void Start() {

        //Initialise gravity variables
        gravity = "-y";
        gravitySwitching = false;
        cameraSwitching = false;

        //Initialise gravity charge variables
        gravityCharge = GRAVITY_MAX;
        rechargeRate = RECHARGE_RATE;
        haveCharge = true;
        rechargeOn = true;

        //Initialise control variables
        shiftPressed = false;
        jumpAxis = 0f;
        verticalAxis = 0f;
        horizontalAxis = 0f;
        quadrant = 1;

        //Get Scripts
        playerControllerScript = controller.GetComponent<PlayerController>();
        //cameraPosScript = cameraPos.GetComponent<CameraPosScript>();
        vertArrowsScript = vertArrows.GetComponent<VertArrowsScript>();

        gravityAxisDisplayScript = GetComponent<GravityAxisDisplayScript>();

    } //End Start()

    //Update()
    private void Update() {
        SetCharge();
        gravityAxisDisplayScript.SetVariables(gravity, gravitySwitching, gravityCharge, haveCharge, shiftPressed, quadrant);

    } //End Update()

    //FixedUpdate()
    private void FixedUpdate() {

        SetGravity();
        RotateGravityBlock();
        RotatePlayer();
        //RotateCamera(); //print(cameraSwitching);
        SetQuadrant();

    } //End FixedUpdate()

    //FixSkew()
    private void FixSkew() {
        if (!gravitySwitching) {
            if (gravity == "-y" || gravity == "y") {
                rotationBlock.transform.eulerAngles = new Vector3(RoundNum(rotationBlock.transform.eulerAngles.x, 90),
                                                                  rotationBlock.transform.eulerAngles.y,
                                                                  RoundNum(rotationBlock.transform.eulerAngles.z, 90));

                //rotationBlock.transform.rotation = Quaternion.Euler(RoundNum(rotationBlock.transform.eulerAngles.x, 90),
                //                                                    rotationBlock.transform.eulerAngles.y,
                //                                                    RoundNum(rotationBlock.transform.eulerAngles.z, 90));
            } else if (gravity == "z" || gravity == "-z") {
                //I don't know why this works - it should be z
                rotationBlock.transform.eulerAngles = new Vector3(rotationBlock.transform.eulerAngles.x,
                                                                  RoundNum(rotationBlock.transform.eulerAngles.y, 90),
                                                                  RoundNum(rotationBlock.transform.eulerAngles.z, 90));

                //rotationBlock.transform.rotation = Quaternion.Euler(RoundNum(rotationBlock.transform.eulerAngles.x, 90),
                //                                                    RoundNum(rotationBlock.transform.eulerAngles.y, 90),
                //                                                    rotationBlock.transform.eulerAngles.z);
            } else if (gravity == "x" || gravity == "-x") {
                rotationBlock.transform.eulerAngles = new Vector3(rotationBlock.transform.eulerAngles.x,
                                                                  RoundNum(rotationBlock.transform.eulerAngles.y, 90),
                                                                  RoundNum(rotationBlock.transform.eulerAngles.z, 90));

                //rotationBlock.transform.rotation = Quaternion.Euler(rotationBlock.transform.eulerAngles.x,
                //                                                     RoundNum(rotationBlock.transform.eulerAngles.y, 90),
                //                                                     RoundNum(rotationBlock.transform.eulerAngles.z, 90));
            }
        }
    }

    //SetGravity() sets the gravity variable based on the rotation of rotationBlock
    private void SetGravity() {

        //rotationBlockUp variables
        int rotationBlockUpX = Mathf.RoundToInt(rotationBlock.transform.up.x);
        int rotationBlockUpY = Mathf.RoundToInt(rotationBlock.transform.up.y);
        int rotationBlockUpZ = Mathf.RoundToInt(rotationBlock.transform.up.z);

        //Check rotationBlockUp
        if (rotationBlockUpY == Vector3.up.y) {  //-y gravity
            gravity = "-y";
        } else if (rotationBlockUpY == Vector3.down.y) { //y gravity
            gravity = "y";
        } else if (rotationBlockUpX == Vector3.left.x) { //x gravity
            gravity = "x";
        } else if (rotationBlockUpX == Vector3.right.x) { //-x gravity
            gravity = "-x";
        } else if (rotationBlockUpZ == Vector3.back.z) { //z gravity
            gravity = "z";
        } else if (rotationBlockUpZ == Vector3.forward.z) { //-z gravity
            gravity = "-z";
        } else {
            gravity = null;
        } //End if (rotationBlockUp)

    } //End SetGravity()

    //RotateGravityBlock() sets the rotation of gravityBlock which controls axis orientation
    private void RotateGravityBlock() {

        //Check gravity
        if (gravity == "-y") {
            gravityBlock.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (gravity == "y") {
            gravityBlock.transform.rotation = Quaternion.Euler(0, 0, 180);
        } else if (gravity == "z") {
            gravityBlock.transform.rotation = Quaternion.Euler(-90, 0, 0);
        } else if (gravity == "-z") {
            gravityBlock.transform.rotation = Quaternion.Euler(-90, -180, 0);
        } else if (gravity == "x") {
            gravityBlock.transform.rotation = Quaternion.Euler(-90, 90, 0);
        } else if (gravity == "-x") {
            gravityBlock.transform.rotation = Quaternion.Euler(-90, -90, 0);
        } //End if (gravity)

    } //End RotateGravityBlock()


    //ChangeGravity() calls checks for gravity charge and if gravity is switching and calls the appropriate gravity switching method
    public void ChangeGravity() {

        //Check haveCharge & gravitySwitching
        if (haveCharge && !gravitySwitching) { //If player has gravity charge and gravity is not switching

            //Initialise rotation block rotation
            rotationBlock.transform.rotation = controller.transform.rotation;
            FixSkew();

            //Check input
            if (jumpAxis > 0) { //If spacebar
                GravityUp();

            } else if (verticalAxis + horizontalAxis != 0) { //If vert/hor buttons                
                int turnMod = -1;

                //Check verticalAxis
                if (verticalAxis != 0) { //If vertical buttons
                    if (verticalAxis > 0) { //Forward button
                        turnMod = 1;
                    } else { //Backward button
                        turnMod = 3;
                    }

                } else if (horizontalAxis != 0) { //If horizontal buttons
                    if (horizontalAxis > 0) { //Right button
                        turnMod = 2;
                    } else { //Left button
                        turnMod = 4;
                    }

                } //End if vert/hor button

                GravityHor(turnMod);

            } //End if input         

            gravitySwitching = true; //Gravity switching is true

            gravityCharge -= GRAVITY_COST; //Decrease gravity charge

        } //End If (haveCharge && !gravitySwitching)

    } //End ChangeGravity()

    int RoundNum(float num, int round) {
        return Mathf.RoundToInt(num / round) * round;
    }

    //GravityUp() rotates the player "up" (180 degrees on relative z-axis)
    private void GravityUp() {

        //Check gravity
        if (gravity == "-y" || gravity == "y" || true) { //If y gravities
            rotationBlock.transform.Rotate(0, 0, 180, Space.Self);
        } else if (gravity == "z" || gravity == "-z") { //If z gravities
            rotationBlock.transform.Rotate(0, 0, 180, Space.Self);
        } else if (gravity == "x" || gravity == "-x") { //If x gravities
            rotationBlock.transform.Rotate(0, 0, 180, Space.Self);
        } //End if (gravity)


    } //End GravityUp()


    //GravityHor() calls the respective method for rotating the player horizontally basied on button press and quadrant
    private void GravityHor(int turnMod) {

        //Apply turn Mod to quadrant
        quadrant += turnMod - 1;

        //Wrap quadrant values <0 and >4
        if (quadrant > 4) {
            quadrant -= 4;
        } else if (quadrant < 1) {
            quadrant += 4;
        }

        //Check quadrant
        if (quadrant == 1) { //Facing forward
            GravityForward();
        } else if (quadrant == 2) { //Facing right
            GravityRight();
        } else if (quadrant == 3) { //Facing backward
            GravityBackward();
        } else if (quadrant == 4) { //Facing left
            GravityLeft();
        } //End if (quadrant)

    } //End GravityHor()

    //GravityForward() rotates the player "forward" (-90 degrees on relative x-axis)
    private void GravityForward() {

        //Check gravity
        if (gravity == "-y" || gravity == "z") { //If -y or z gravities
            rotationBlock.transform.Rotate(-90, 0, 0, Space.World);
        } else if (gravity == "y" || gravity == "-z") { //If y or -z gravities
            rotationBlock.transform.Rotate(90, 0, 0, Space.World);
        } else if (gravity == "x") { //If x gravity
            rotationBlock.transform.Rotate(0, 0, 90, Space.World);
        } else if (gravity == "-x") { //If -x gravity
            rotationBlock.transform.Rotate(0, 0, -90, Space.World);
        } //End if (gravity)

    } //End GravityForward()

    //GravityBackward() rotates the player "backward" (90 degrees on relative x-axis)
    private void GravityBackward() {

        //Check gravity
        if (gravity == "-y" || gravity == "z") { //If -y or z gravities
            rotationBlock.transform.Rotate(90, 0, 0, Space.World);
        } else if (gravity == "y" || gravity == "-z") { //If y or -z gravities
            rotationBlock.transform.Rotate(-90, 0, 0, Space.World);
        } else if (gravity == "x") { //If x gravity
            rotationBlock.transform.Rotate(0, 0, -90, Space.World);
        } else if (gravity == "-x") { //If -x gravity
            rotationBlock.transform.Rotate(0, 0, 90, Space.World);
        } //End if (gravity)

    } //End GravityBackward()

    //GravityRight() rotates the player "right" (90 degrees on relative z-axis)
    private void GravityRight() {

        //Check gravity
        if (gravity == "-y" || gravity == "y") { //If y gravities
            rotationBlock.transform.Rotate(0, 0, 90, Space.World);
        } else { //If x or z gravities
            rotationBlock.transform.Rotate(0, 90, 0, Space.World);
        } //End if (gravity) 

    } //End GravityRight()

    //GravityRight() rotates the player "left" (-90 degrees on relative z-axis)
    private void GravityLeft() {

        //Check gravity
        if (gravity == "-y" || gravity == "y") { //If y gravities
            rotationBlock.transform.Rotate(0, 0, -90, Space.World);
        } else { //If x or z gravities
            rotationBlock.transform.Rotate(0, -90, 0, Space.World);
        } //End if (gravity) 

    } //End GravityLeft()


    //RotatePlayer() will rotate the player when gravity is switching towards rotation block
    private void RotatePlayer() {

        //Player rotation as Quaternion
        Quaternion playerRotation;
        playerRotation = controller.transform.rotation;

        if (gravitySwitching) { //If gravity is switching            

            //Lerp playerRotation towards rotation block and apply it to player
            playerRotation = Quaternion.Lerp(controller.transform.rotation, rotationBlock.transform.rotation, Time.deltaTime * 8);
            //controller.transform.rotation = playerRotation;
            playerControllerScript.targetRotation = playerRotation;

            //Check if full rotated
            if (Mathf.Abs(controller.transform.eulerAngles.x - rotationBlock.transform.eulerAngles.x) <= 1 &&
                Mathf.Abs(controller.transform.eulerAngles.y - rotationBlock.transform.eulerAngles.y) <= 1 &&
                Mathf.Abs(controller.transform.eulerAngles.z - rotationBlock.transform.eulerAngles.z) <= 1) {

                controller.transform.rotation = rotationBlock.transform.rotation; //Completely rotate player
                gravitySwitching = false;

                //Invoke("SetCameraSwitching", 0.1f);
                //cameraSwitching = true;

            } //End if (fully rotated)

        } //End if (gravitySwitching)

    } //end RotatePlayer()

    private void SetCameraSwitching() {
        cameraSwitching = true;
        gravitySwitching = false; //Turn of gravitySwitching
    }

    private void RotateCamera() {


        if (cameraSwitching) {

            Transform cameraTransform, targetTransform;
            targetTransform = gravityBlock.transform;
            cameraTransform = cameraPos.transform;

            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetTransform.rotation, Time.deltaTime * 10);

            if (Mathf.Abs(cameraPos.transform.up.x - gravityBlock.transform.up.x) <= 0.1 &&
                Mathf.Abs(cameraPos.transform.up.y - gravityBlock.transform.up.y) <= 0.1 &&
                Mathf.Abs(cameraPos.transform.up.z - gravityBlock.transform.up.z) <= 0.1) {

                cameraTransform.rotation = targetTransform.rotation;

                cameraSwitching = false;

            }
            //cameraPosScript.gravityUp = cameraTransform.up;
        }
    }

    //SetQuadrant() sets the quadrant variable based on the relative y-rotation of the 
    private void SetQuadrant() {

        float yRot = vertArrowsScript.GetYRot(); //relative y-rotation

        //Check yRot
        if (315 < yRot || yRot <= 45) { //Facing forward
            quadrant = 1;
        } else if (45 < yRot && yRot <= 135) { //Facing right
            quadrant = 2;
        } else if (135 < yRot && yRot <= 225) { //Facing backward
            quadrant = 3;
        } else if (225 < yRot && yRot <= 315) { //Facing left
            quadrant = 4;
        } //End if (yRot)

    } //End SetQuadrant() 


    //SetCharge() recharges the gravity charge and sets haveCharge accordingly
    private void SetCharge() {

        if (rechargeOn) { //If recharge is on
            gravityCharge = Mathf.Min(GRAVITY_MAX, gravityCharge + rechargeRate); //Recharge gravity charge
        }

        if (gravityCharge < GRAVITY_COST) { //If gravity charge doesn't have enough for a gravity switch
            haveCharge = false;
        } else {
            haveCharge = true;
        }

    } //End SetCharge()


    //SetShiftPressed() retrieves shiftPressed, called in ControllerScript
    public void SetShiftPressed(bool thisShiftPressed) {
        shiftPressed = thisShiftPressed;
    } //End SetShiftPressed()

    //SetAxes() retrieves input axes, called in ControllerScript
    public void SetAxes(float thisJumpAxis, float thisVerticalAxis, float thisHorizontalAxis) {
        jumpAxis = thisJumpAxis;
        verticalAxis = thisVerticalAxis;
        horizontalAxis = thisHorizontalAxis;
    } //End GetAxes()

    //GetGravitySwitching() returns gravitySwitching, called in ControllerScript
    public bool GetGravitySwitching() {
        return gravitySwitching;
    } //End GetGravitySwitching()

    private class Gravity {
        private static float velocity = 15.0f;

        private static Vector3 Right = new Vector3(1, 0, 0);     // x+
        private static Vector3 Left = new Vector3(-1, 0, 0);     // x-
        private static Vector3 Up = new Vector3(0, 1, 0);        // y+
        private static Vector3 Down = new Vector3(0, -1, 0);     // y-
        private static Vector3 Forward = new Vector3(0, 0, 1);   // z+
        private static Vector3 Backward = new Vector3(0, 0, -1); // z-

        private Orientation currentOrientation; //Current orientation
        private Vector3 currentGravityDirection; //Current direction of gravity
        private string gravityString; //Worded representation of current direction of gravity 

        public Gravity(Orientation orientation) {
            this.setOrientation(orientation);
        }

        public Vector3 getDistanceTravelled(float deltaTime) {
            return this.currentGravityDirection * Gravity.velocity * deltaTime;
        }

        public Vector3 getGravityDirection() {
            return this.currentGravityDirection;
        }

        public Orientation getOrientation() {
            return this.currentOrientation;
        }

        public void setOrientation(Orientation orientation) {
            Vector3 gravityDirection = new Vector3(0, 0, 0);
            this.currentOrientation = orientation;

            switch (currentOrientation) {

                case Orientation.Right:
                    gravityDirection = Gravity.Left;
                    this.gravityString = "x";
                    break;
                case Orientation.Left:
                    gravityDirection = Gravity.Right;
                    this.gravityString = "x-";
                    break;

                case Orientation.Up:
                    gravityDirection = Gravity.Down;
                    this.gravityString = "y";
                    break;
                case Orientation.Down:
                    gravityDirection = Gravity.Up;
                    this.gravityString = "y-";
                    break;    
                    
                case Orientation.Forward:
                    gravityDirection = Gravity.Backward;
                    this.gravityString = "z";
                    break;
                case Orientation.Backward:
                    gravityDirection = Gravity.Forward;
                    this.gravityString = "z-";
                    break;

                default:
                    break;
            }

            this.currentGravityDirection = gravityDirection;
        }

        public override string ToString() {
            return this.gravityString;
        }
    }

} //End GravityAxisScript
