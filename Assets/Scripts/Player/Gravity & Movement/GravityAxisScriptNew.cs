//NEW ONE

using UnityEngine;
using System;

namespace Gravity {
    enum Orientation {
        Up,
        Down,
        Right,
        Left,
        Forward,
        Backward
    }

    //GravityAxisScript controls all the aspects of the gravity axis and gravity switching.
    public class GravityAxisScript : MonoBehaviour {
        //Gravity charge constants
        private const int GRAVITY_MAX = 10000;
        private const int GRAVITY_COST = 2500;
        private const int RECHARGE_RATE = 10000000;

        //Gravity variables
        private Gravity gravity;
        private bool gravitySwitching;
        private bool cameraSwitching;

        //Gravity charge variables
        private int gravityCharge;
        private int rechargeRate;
        private bool haveCharge;
        private bool rechargeOn;

        //Control variables
        private bool shiftPressed;
        private int quadrant;

        //Other Objects/Scripts
        public GameObject controller;
        public GameObject cameraPos;
        public GameObject vertArrows;
        public GameObject gravityBlock;
        public GameObject rotationBlock;

        private PlayerController playerControllerScript;
        //private CameraPosScript cameraPosScript;
        private YRotScript yRotScript;

        private GravityAxisDisplayScript gravityAxisDisplayScript;

        public float degRotated;
        public Vector3 rotation = new Vector3(0, 0, 0);

        //Start()
        private void Start() {
            //Initialise gravity variables
            gravity = new Gravity(Orientation.Up);
            gravitySwitching = false;
            cameraSwitching = false;

            //Initialise gravity charge variables
            gravityCharge = GRAVITY_MAX;
            rechargeRate = RECHARGE_RATE;
            haveCharge = true;
            rechargeOn = true;

            //Initialise control variables
            shiftPressed = false;
            quadrant = 1;

            //Get Scripts
            playerControllerScript = controller.GetComponent<PlayerController>();
            //cameraPosScript = cameraPos.GetComponent<CameraPosScript>();
            yRotScript = vertArrows.GetComponent<YRotScript>();

            gravityAxisDisplayScript = GetComponent<GravityAxisDisplayScript>();

            degRotated = 0;
        } //End Start()

        private void DebugStuff() {
            print(gravity);
        }

        //Update()
        private void Update() {

            DebugStuff();

            degRotated += playerControllerScript.moveSettings.rotateVel * Input.GetAxisRaw("Mouse X") * Time.deltaTime * 2;
            degRotated %= 360;

            SetCharge();
            gravityAxisDisplayScript.SetVariables(gravity.ToString(), gravitySwitching, gravityCharge, haveCharge, shiftPressed, this.getQuadrant());

        } //End Update()

        //FixedUpdate()
        private void FixedUpdate() {
            RotatePlayer();
            SetQuadrant();

        } //End FixedUpdate()


        //SetGravity() sets the gravity variable based on the rotation of rotationBlock
        private void SetGravity() {
            float smallestAngle = Vector3.Angle(Vector3.up, rotationBlock.transform.up);
            Orientation orientation = Orientation.Up;

            if (smallestAngle > Vector3.Angle(Vector3.up, -rotationBlock.transform.up)) {
                smallestAngle = Vector3.Angle(Vector3.up, -rotationBlock.transform.up);
                orientation = Orientation.Down;
            }

            if (smallestAngle > Vector3.Angle(Vector3.up, rotationBlock.transform.right)) {
                smallestAngle = Vector3.Angle(Vector3.up, rotationBlock.transform.right);
                orientation = Orientation.Right;
            }

            if (smallestAngle > Vector3.Angle(Vector3.up, -rotationBlock.transform.right)) {
                smallestAngle = Vector3.Angle(Vector3.up, -rotationBlock.transform.right);
                orientation = Orientation.Left;
            }

            if (smallestAngle > Vector3.Angle(Vector3.up, rotationBlock.transform.forward)) {
                smallestAngle = Vector3.Angle(Vector3.up, rotationBlock.transform.forward);
                orientation = Orientation.Forward;
            }

            if (smallestAngle > Vector3.Angle(Vector3.up, -rotationBlock.transform.forward)) {
                smallestAngle = Vector3.Angle(Vector3.up, -rotationBlock.transform.forward);
                orientation = Orientation.Backward;
            }

            gravity.setOrientation(orientation);
        } //End SetGravity()

        public int getQuadrant() {
            //Check yRot
            if (315 < degRotated || degRotated <= 45) {
                //Facing forward
                return 1;

            } else if (45 < degRotated && degRotated <= 135) {
                //Facing right
                return 2;

            } else if (135 < degRotated && degRotated <= 225) {
                //Facing backward
                return 3;
            } else if (225 < degRotated && degRotated <= 315) {
                //Facing left
                return 4;
            } //End if (yRot)

            return -1;
        }

        //ChangeGravity() calls checks for gravity charge and if gravity is switching and calls the appropriate gravity switching method
        public void ChangeGravity(float jumpAxis, float horizontalAxis, float verticalAxis) {
            int quadrant = this.getQuadrant();
            bool flipVertical = false;
            bool flipHorizontal = false;
            bool exchangeVertHor = false;

            switch (quadrant) {
                case 1:
                    break;
                case 2:
                    exchangeVertHor = true;
                    break;
                case 3:
                    flipVertical = true;
                    break;
                case 4:
                    exchangeVertHor = true;
                    flipHorizontal = true;
                    break;
            }

            if (flipVertical) {
                verticalAxis *= -1;
            }

            if (flipHorizontal) {
                horizontalAxis *= -1;
            }

            if (exchangeVertHor) {
                float temp = horizontalAxis;
                horizontalAxis = verticalAxis;
                verticalAxis = temp;
            }

            //Check haveCharge & gravitySwitching
            if (haveCharge && !gravitySwitching) { //If player has gravity charge and gravity is not switching
                Vector3 axisOfRotation = rotationBlock.transform.right;
                float angle = 0;

                if (jumpAxis > 0) {
                    angle = 180.0f;
                } else if (verticalAxis > 0) {
                    axisOfRotation = rotationBlock.transform.right;
                    angle = -90.0f;
                } else if (horizontalAxis > 0) {
                    axisOfRotation = rotationBlock.transform.forward;
                    angle = 90.0f;
                } else if (verticalAxis < 0) {
                    axisOfRotation = rotationBlock.transform.right;
                    angle = 90.0f;
                } else if (horizontalAxis < 0) {
                    axisOfRotation = rotationBlock.transform.forward;
                    angle = -90.0f;
                }

                Vector3 inverseAxisOfRotation = new Vector3(
                    axisOfRotation.x == 0 ? 1 : 0,
                    axisOfRotation.y == 0 ? 1 : 0,
                    axisOfRotation.z == 0 ? 1 : 0
                );
                rotation = rotation + (axisOfRotation * angle);
                rotationBlock.transform.rotation = controller.transform.rotation;
                rotationBlock.transform.eulerAngles = rotation;
                this.SetGravity();

                Debug.Log(rotation);

                gravitySwitching = true; //Gravity switching is true
                gravityCharge -= GRAVITY_COST; //Decrease gravity charge
            } //End If (haveCharge && !gravitySwitching)

        } //End ChangeGravity()


        //RotatePlayer() will rotate the player when gravity is switching towards rotation block
        private void RotatePlayer() {

            //Player rotation as Quaternion
            Quaternion playerRotation;
            playerRotation = controller.transform.rotation;

            if (gravitySwitching) { //If gravity is switching            

                //Lerp playerRotation towards rotation block and apply it to player
                playerRotation = Quaternion.Lerp(controller.transform.rotation, rotationBlock.transform.rotation, Time.deltaTime * 10);
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

            float yRot = yRotScript.GetYRot(); //relative y-rotation

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

        //GetGravitySwitching() returns gravitySwitching, called in ControllerScript
        public bool GetGravitySwitching() {
            return gravitySwitching;
        } //End GetGravitySwitching()

        private class Gravity {
            private static float velocity = 15.0f;
            private static Vector3 Up = new Vector3(0, 1, 0);        // y+
            private static Vector3 Down = new Vector3(0, -1, 0);     // y-
            private static Vector3 Right = new Vector3(1, 0, 0);     // x+
            private static Vector3 Left = new Vector3(-1, 0, 0);     // x-
            private static Vector3 Forward = new Vector3(0, 0, 1);   // z+
            private static Vector3 Backward = new Vector3(0, 0, -1); // z-

            private Orientation currentOrientation;
            private Vector3 currentGravityDirection;
            private string gravityString;

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
                    case Orientation.Up:
                        gravityDirection = Gravity.Down;
                        this.gravityString = "y";
                        break;
                    case Orientation.Down:
                        gravityDirection = Gravity.Up;
                        this.gravityString = "y-";
                        break;
                    case Orientation.Right:
                        gravityDirection = Gravity.Left;
                        this.gravityString = "x";
                        break;
                    case Orientation.Left:
                        gravityDirection = Gravity.Right;
                        this.gravityString = "x-";
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
}