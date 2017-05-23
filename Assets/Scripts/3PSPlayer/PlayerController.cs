using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [System.Serializable]
    public class MoveSettings {
        public float forwardVel = 12; // speed of forward movement
        public float rightVel = 12; // speed of right movement
        public float rotateVel = 100; // speed of rotation movement
        public float jumpVel = 25; // speed of jumping movement
        public float distToGrounded = 0.1f; // player is value from the ground - then player is grounded
        public LayerMask ground; // layer for the ground - player allowed to jump on this layer
    }

    [System.Serializable]
    public class PhysSettings {
        public float downAccel = 0.75f; // speed of falling (not grounded) GRAVITY ~~~~~~~~~~~~
    }

    [System.Serializable]
    public class InputSettings {
        public float inputDelay = 0.1f; // small delay in movement
        public string FORWARD_AXIS = "Vertical"; // string for vertical
        public string TURN_AXIS = "Horizontal"; // string for horizontal
        public string RIGHT_AXIS = "Horizontal"; // string for horizontal
        public string JUMP_AXIS = "Jump"; // string for jump
    }

    public MoveSettings moveSettings = new MoveSettings();
    public PhysSettings physSettings = new PhysSettings();
    public InputSettings inputSettings = new InputSettings();

    Vector3 velocity = Vector3.zero; // variable to easily control player movement
    public Quaternion targetRotation; // holds the next rotation
    Rigidbody rBody; // rigidbody of the player
    float forwardInput, turnInput, rightInput, jumpInput; // input recieved from player
    bool turnMode; // horizontal mode turns (true) or strafe (false)

    public GameObject gravityAxis;
    public GameObject gravityBlock;

    GravityAxisScript gravityAxisScript;
    GravityBlockScript gravityBlockScript;

    public Quaternion TargetRotation {
        get { return targetRotation; }
    }

    bool Grounded() {
        return Physics.Raycast(transform.position, -transform.up, moveSettings.distToGrounded, moveSettings.ground);
    }

    public void FreezeRotation() {

        if (gravityAxisScript.GetGravitySwitching()) {
            rBody.freezeRotation = false;
        } else {
            rBody.freezeRotation = true;
        }
    }

    void Start() {
        targetRotation = transform.rotation;

        if (GetComponent<Rigidbody>()) {
            rBody = GetComponent<Rigidbody>();
        } else {
            Debug.LogError("The player needs a rigidbody.");
        }

        forwardInput = turnInput = jumpInput = 0;

        turnMode = false;

        gravityAxisScript = gravityAxis.GetComponent<GravityAxisScript>();
        gravityBlockScript = gravityBlock.GetComponent<GravityBlockScript>();
    }

    void GetInput() {

        forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS); // interpolated 
        rightInput = Input.GetAxis(inputSettings.RIGHT_AXIS); // interpolated 
        turnInput = Input.GetAxis(inputSettings.TURN_AXIS); // interpolated    
        jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS); // non-interpolated

        //If shift is pressed (gravity selection)
        if (Input.GetButton("Crouch")) {

            gravityAxisScript.SetShiftPressed(true); //shiftPressed true

            if ((Input.GetButton("Jump") || Input.GetButton("Vertical") || Input.GetButton("Horizontal"))) {
                // Used Gravity || in the air - Alex
                gravityAxisScript.SetAxes(Input.GetAxis("Jump"), Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                gravityAxisScript.ChangeGravity();
            }

        } else {
            gravityAxisScript.SetShiftPressed(false); ; //shiftPressed false
        } //End if shift

    }

    void Update() {

        if (UI_PauseMenu.isOn)
            return;

        GetInput();
        Turn();

        gravityBlockScript.UpdatePosition(); //Update gravity block position
    }

    void FixedUpdate() {

        Run();
        Strafe();
        Jump();

        rBody.velocity = transform.TransformDirection(velocity);
    }

    void Run() {

        if (Mathf.Abs(forwardInput) > inputSettings.inputDelay && !gravityAxisScript.GetGravitySwitching()) {
            // Walking - Alex
            // move
            velocity.z = moveSettings.forwardVel * forwardInput;
        } else {
            // zero velocity
            velocity.z = 0;
        }
    }

    void Strafe() {
        if (true) {
            if (Mathf.Abs(rightInput) > inputSettings.inputDelay && !gravityAxisScript.GetGravitySwitching()) {
            // move
            velocity.x = moveSettings.rightVel * rightInput;
        } else {
            // zero velocity
            velocity.x = 0;
        }
    }
}

    void Turn() {
        if (true) {

            //if (Mathf.Abs(turnInput) > inputSettings.inputDelay && !gravityAxisScript.GetGravitySwitching()) {
            //    targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVel * turnInput * Time.deltaTime, Vector3.up);
            //}

        }

        targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVel * Input.GetAxisRaw("Mouse X") * Time.deltaTime * 2, Vector3.up);
        //orbit.yRotation += hOrbitMouseInput * orbit.hOrbitSmooth * Time.deltaTime;

        transform.localRotation = targetRotation;
    }

    void Jump() {

        if (jumpInput > 0 && Grounded() && !gravityAxisScript.GetGravitySwitching()) {
            // Jumping - Alex
            // jump
            velocity.y = moveSettings.jumpVel;
        } else if (jumpInput == 0 && Grounded()) {
            // zero out our velociy.y
            velocity.y = 0;
        } else {
            // decrease velocity.y
            velocity.y -= physSettings.downAccel;
        }
    }

}
