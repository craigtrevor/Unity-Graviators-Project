using UnityEngine;
using UnityEngine.Networking;
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

    public Vector3 velocity = Vector3.zero; // variable to easily control player movement
    public Quaternion targetRotation; // holds the next rotation
    Rigidbody rBody; // rigidbody of the player
    float forwardInput, turnInput, rightInput, jumpInput; // input recieved from player
    bool turnMode; // horizontal mode turns (true) or strafe (false)

    public Animator playerAnimator;

    public GameObject gravityAxis;
    public GameObject gravityBlock;

    GravityAxisScript gravityAxisScript;
    GravityBlockScript gravityBlockScript;
    Network_Soundscape networkSoundscape;

    public float cameraDisplacement;

    public Quaternion TargetRotation {
        get { return targetRotation; }
    }

    bool Grounded() {
        playerAnimator.SetBool("InAir", false);
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

        if (GetComponentInParent<Rigidbody>()) {
            rBody = GetComponentInParent<Rigidbody>();
        } else {
            Debug.LogError("The player needs a rigidbody.");
        }

        forwardInput = turnInput = jumpInput = 0;

        turnMode = false;

        gravityAxisScript = gravityAxis.GetComponent<GravityAxisScript>();
        gravityBlockScript = gravityBlock.GetComponent<GravityBlockScript>();
        networkSoundscape = GetComponentInParent<Network_Soundscape>();
    }

    void GetInput() {

        forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS); // interpolated 
        rightInput = Input.GetAxis(inputSettings.RIGHT_AXIS); // interpolated 
        turnInput = Input.GetAxis(inputSettings.TURN_AXIS); // interpolated    
        jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS); // non-interpolated

        //If shift is pressed (gravity selection)
        if (UI_PauseMenu.IsOn == true)
            return;

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

    float rotY = 0;
    public GameObject eyes;

    void Update() {

        GetInput();
        Turn();

        gravityBlockScript.UpdatePosition(); //Update gravity block position
    }

    void FixedUpdate() {

        Run();
        Strafe();
        Jump();
        Attack();
        CheckPause();

        cameraDisplacement = Mathf.Min((velocity.y + 20f) / 30f, 0f);
        //print(cameraDisplacement);

        rBody.velocity = transform.TransformDirection(velocity);
    }

    void Attack() {

        if (UI_PauseMenu.IsOn == true)
            return;

        //Attack Placeholder ALEX

        if (Input.GetButton("Fire1")) {
            StartCoroutine(AttackTime());
        }

        //if (Input.GetButtonUp("Fire1"))
        //{
        //    playerAnimator.SetBool("Attack", false);
        //}
    }

    void Run() {

        if (Mathf.Abs(forwardInput) > inputSettings.inputDelay && !gravityAxisScript.GetGravitySwitching()) {

            if (UI_PauseMenu.IsOn == true)
                return;

            // Walking - Alex
            playerAnimator.SetBool("Moving", true);

            // move
            velocity.z = moveSettings.forwardVel * forwardInput;
        } else {
            // zero velocity

            playerAnimator.SetBool("Moving", false);

            velocity.z = 0;
        }

        if (UI_PauseMenu.IsOn == true) {
            // zero velocity

            playerAnimator.SetBool("Moving", false);

            velocity.z = 0;
        }

    }

    void Strafe() {

        if (true) {
            if (Mathf.Abs(rightInput) > inputSettings.inputDelay && !gravityAxisScript.GetGravitySwitching()) {

                if (UI_PauseMenu.IsOn == true)
                    return;
                // move
                velocity.x = moveSettings.rightVel * rightInput;
            } else {
                // zero velocity
                velocity.x = 0;
            }
        }
    }

    void Turn() {

        if (UI_PauseMenu.IsOn == true)
            return;

        if (!gravityAxisScript.GetGravitySwitching()) {
            targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVel * Input.GetAxisRaw("Mouse X") * Time.deltaTime * 2, Vector3.up);

        }
        rotY -= Input.GetAxis("Mouse Y") * 2f;
        rotY = Mathf.Clamp(rotY, -60f, 60f);
        eyes.transform.localRotation = Quaternion.Lerp(eyes.transform.localRotation, Quaternion.Euler(rotY - cameraDisplacement * 15, 0, 0), Time.deltaTime * 30);

        //orbit.yRotation += hOrbitMouseInput * orbit.hOrbitSmooth * Time.deltaTime; no

        transform.localRotation = targetRotation;
    }

    void Jump() {

        if (UI_PauseMenu.IsOn == true)
            return;

        if (jumpInput > 0 && Grounded() && !gravityAxisScript.GetGravitySwitching()) {
            // Jumping - Alex
            StartCoroutine(JumpTime());
            velocity.y = moveSettings.jumpVel;
            networkSoundscape.PlaySound(2, 1, 0.0f);    

        } else if (jumpInput == 0 && Grounded()) {
            // zero out our velociy.y
            velocity.y = 0;
        } else {
            // decrease velocity.y
            playerAnimator.SetBool("InAir", true);
            velocity.y -= physSettings.downAccel;
            velocity.y = Mathf.Max(velocity.y, -100);
        }
    }

    void CheckPause() {
        if (UI_PauseMenu.IsOn) {
            if (!Grounded()) {
                playerAnimator.SetBool("InAir", true);
                velocity.y -= physSettings.downAccel;
                velocity.y = Mathf.Max(velocity.y, -100);
            } else {
                if (jumpInput == 0 && Grounded()) {
                    playerAnimator.SetBool("Moving", false);
                    velocity.z = 0;
                }
            }
        }
    }

    IEnumerator JumpTime() {
        playerAnimator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("Jump", false);
    }

    IEnumerator AttackTime() {
        playerAnimator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("Attack", false);
    }
}
