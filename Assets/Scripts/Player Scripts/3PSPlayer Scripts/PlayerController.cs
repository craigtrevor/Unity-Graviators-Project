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
        public float downAccel = 0.75f; // LOOK HOW I AM DEFINING GRAVITY
    }

    [System.Serializable]
    public class InputSettings {
        public float inputDelay = 0.1f; // small delay in movement
        public string FORWARD_AXIS = "Vertical"; // string for vertical
        public string TURN_AXIS = "Horizontal"; // string for horizontal
        public string RIGHT_AXIS = "Horizontal"; // string for horizontal
        public string JUMP_AXIS = "Jump"; // string for jump
        public bool GRAVITY_RELEASE = false;
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
    public Transform playerModel;
    public GameObject blastWave;
    public bool hasLanded;

    public GameObject gravityAxis;
    public GameObject gravityBlock;

    GravityAxisScript gravityAxisScript;
    GravityBlockScript gravityBlockScript;
    [SerializeField]
    Network_CombatManager netCombatManager;
    [SerializeField]
    Network_PlayerManager netPlayerManager;
    Network_Soundscape netSoundscape;

    public float cameraDisplacement;
    public bool stunned;
    public bool isDashing;
    private bool recieveInput;
    public bool isShiftPressed;

    private int cycleMovement;
    private float audioStepLength = 0.45f;
    private bool playerStep;

    public int retainFallOnGrav;
    bool fallPaused = false;
    float tempYVel = 0;
    public bool jumping;

    public GameObject sphere;

    Quaternion strafeRot; //for strafing animation

    public Quaternion TargetRotation {
        get { return targetRotation; }
    }

    public bool Grounded() {
        //return Physics.Raycast(transform.position, -transform.up, moveSettings.distToGrounded, moveSettings.ground);
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        float radius = capsuleCollider.radius * 0.65f;
        Vector3 pos = capsuleCollider.transform.position + -capsuleCollider.transform.up * (capsuleCollider.height / 2 - 0.25f);

        if (sphere != null) {
            sphere.transform.position = pos;
            sphere.transform.localScale = Vector3.one * radius * 2f;
        }

        return Physics.CheckSphere(pos, radius, moveSettings.ground);
    }

    public void FreezeRotation() {

        if (gravityAxisScript.GetGravitySwitching()) {
            rBody.freezeRotation = false;
        } else {
            rBody.freezeRotation = true;
        }
    }

    void Start() {

        hasLanded = true; // setup animation variable
        playerStep = true;
        cycleMovement = 0;

        targetRotation = transform.rotation;

        if (GetComponentInParent<Rigidbody>()) {
            rBody = GetComponentInParent<Rigidbody>();
        } else {
            Debug.LogError("The player needs a rigidbody.");
        }

        forwardInput = turnInput = jumpInput = 0;

        turnMode = false;

        netSoundscape = GetComponentInParent<Network_Soundscape>();
        netCombatManager = transform.root.GetComponent<Network_CombatManager>();
        netPlayerManager = transform.root.GetComponent<Network_PlayerManager>();

        gravityAxisScript = gravityAxis.GetComponent<GravityAxisScript>();
        gravityBlockScript = gravityBlock.GetComponent<GravityBlockScript>();

        recieveInput = true;
        isShiftPressed = false;
        stunned = false;
        inputSettings.GRAVITY_RELEASE = false;

        strafeRot = Quaternion.Euler(Vector3.zero);
    }

    void GetInput() {
        if (recieveInput && !stunned && !netCombatManager.isUlting) {
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS); // interpolated 
            rightInput = Input.GetAxis(inputSettings.RIGHT_AXIS); // interpolated 
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS); // interpolated    
            //jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS); // non-interpolated
            if (Input.GetButtonDown("Jump") && jumpInput == 0f)
            {
                jumpInput = 1;
            } else
            {
                jumpInput = Mathf.Max(0, jumpInput - 0.1f);
            }
        }
        else {
            forwardInput = rightInput = turnInput = jumpInput = 0f;
        }

        shiftPressed();

        if (!stunned && !isDashing && !netCombatManager.isUlting) {
            GravityInput(inputSettings.GRAVITY_RELEASE);
        }

        if (isDashing) {
            CancelVelocity();
        }
    }

    void CancelVelocity() {
        velocity = Vector3.zero;
    }

    void shiftPressed() {
        if (Input.GetButtonDown("Crouch") && !netCombatManager.isUlting) {
            isShiftPressed = true;
        }

        if (Input.GetButtonUp("Crouch") && !netCombatManager.isUlting) {
            isShiftPressed = false;
        }

    }

    void GravityInput(bool gravityRelease) {
        //If shift is pressed (gravity selection)
        if (UI_PauseMenu.IsOn == true)
            return;

        if (/*Input.GetButton("Crouch")*/isShiftPressed) {
            gravityAxisScript.SetShiftPressed(true); //shiftPressed true

            if (!gravityRelease) {//If gravity release is off

                if (Input.GetButtonDown("Jump")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(Input.GetAxis("Jump"), 0f, 0f);
                    netSoundscape.PlaySound(4, 3, 0.1f, 0);
                    //isShiftPressed = false;
                }

                if (Input.GetButtonDown("GravForward")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(0f, 0f, Input.GetAxis("GravForward"));
                    netSoundscape.PlaySound(4, 3, 0.1f, 0);
                    //isShiftPressed = false;
                }

                if (Input.GetButtonDown("GravBack")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(0f, 0f, -Input.GetAxis("GravBack"));
                    netSoundscape.PlaySound(4, 3, 0.1f, 0);
                    //isShiftPressed = false;
                }

                if (Input.GetButtonDown("GravRight")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(0f, Input.GetAxis("GravRight"), 0f);
                    netSoundscape.PlaySound(4, 3, 0.1f, 0);
                    //isShiftPressed = false;
                }

                if (Input.GetButtonDown("GravLeft")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(0f, -Input.GetAxis("GravLeft"), 0f);
                    netSoundscape.PlaySound(4, 3, 0.1f, 0);
                    //isShiftPressed = false;
                }

            } else { //If gravity release is on

                recieveInput = false;

                if (Input.GetButtonUp("Jump")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(1f, 0f, 0f);
                    //isShiftPressed = false;
                }

                if (Input.GetButtonUp("Horizontal")) {
                    // Used Gravity || in the air - Alex
                    if (Input.GetAxis("Horizontal") > 0) {
                        gravityAxisScript.ChangeGravity(0f, 1f, 0f);
                    } else {
                        gravityAxisScript.ChangeGravity(0f, -1f, 0f);
                    }
                    //isShiftPressed = false;
                }

                if (Input.GetButtonUp("Vertical")) {
                    // Used Gravity || in the air - Alex
                    if (Input.GetAxis("Vertical") > 0) {
                        gravityAxisScript.ChangeGravity(0f, 0f, 1f);
                    } else {
                        gravityAxisScript.ChangeGravity(0f, 0, -1f);
                    }
                    //isShiftPressed = false;
                }

            }

        } else {
            gravityAxisScript.SetShiftPressed(false); ; //shiftPressed false
            recieveInput = true;
        } //End if shift
    }

    float rotY = 0;
    float camY = 0;
    public GameObject eyes;

    void Update() {

        GetInput();
        Turn();
        Animations();

        gravityBlockScript.UpdatePosition(); //Update gravity block position
    }

    void FixedUpdate() {

        Run();
        CheckMovementAudio();
        Strafe();
        Jump();
        CheckPause();

        if (!gravityAxisScript.gravitySwitching) {
            cameraDisplacement = Mathf.Min((velocity.y + 20f) / 30f, 0f);
        }

        //print(cameraDisplacement);

        rBody.velocity = transform.TransformDirection(velocity);
    }

    void Animations() {

        if (!gravityAxisScript.gravitySwitching && !stunned) { //dont strafe when: grav switching or  stunned

            if (Mathf.Abs(forwardInput) + Mathf.Abs(rightInput) > inputSettings.inputDelay) {
                playerAnimator.SetBool("Moving", true);
            } else {
                playerAnimator.SetBool("Moving", false);
            }


            if (Input.GetAxis(inputSettings.RIGHT_AXIS) > 0f && Input.GetButton(inputSettings.RIGHT_AXIS)) { //if moving right

                if (Input.GetAxis(inputSettings.FORWARD_AXIS) > 0f && Input.GetButton(inputSettings.FORWARD_AXIS)) {//forward right
                    strafeRot = Quaternion.Euler(Vector3.up * 45f);
                } else if (Input.GetAxis(inputSettings.FORWARD_AXIS) < 0f && Input.GetButton(inputSettings.FORWARD_AXIS)) {//back right
                    strafeRot = Quaternion.Euler(Vector3.up * 135f);
                } else { //just right 
                    strafeRot = Quaternion.Euler(Vector3.up * 90f);
                }

            } else if (Input.GetAxis(inputSettings.RIGHT_AXIS) < 0f && Input.GetButton(inputSettings.RIGHT_AXIS)) { //if moving left

                if (Input.GetAxis(inputSettings.FORWARD_AXIS) > 0f && Input.GetButton(inputSettings.FORWARD_AXIS)) {//forward right
                    strafeRot = Quaternion.Euler(Vector3.up * -45f);
                } else if (Input.GetAxis(inputSettings.FORWARD_AXIS) < 0f && Input.GetButton(inputSettings.FORWARD_AXIS)) {//back right
                    strafeRot = Quaternion.Euler(Vector3.up * -135f);
                } else { //just right 
                    strafeRot = Quaternion.Euler(Vector3.up * -90f);
                }

            } else { //if not strafing

                if (Input.GetAxis(inputSettings.FORWARD_AXIS) > 0f && Input.GetButton(inputSettings.FORWARD_AXIS)) {//forward 
                    strafeRot = Quaternion.Euler(Vector3.zero);
                } else if (Input.GetAxis(inputSettings.FORWARD_AXIS) < 0f && Input.GetButton(inputSettings.FORWARD_AXIS)) {//back 
                    strafeRot = Quaternion.Euler(Vector3.up * 180f);
                }
            }

            if (netCombatManager.isUlting || Input.GetButtonDown("Fire2") || netCombatManager.isRanging) { //if range or ult
                strafeRot = Quaternion.Euler(Vector3.zero);
            }

            if (playerModel != null) {
                playerModel.localRotation = Quaternion.Lerp(playerAnimator.transform.localRotation, strafeRot, 10f * Time.deltaTime);
            }
        }
    }

    void Run() {

        if (Mathf.Abs(forwardInput) > inputSettings.inputDelay && !gravityAxisScript.GetGravitySwitching()) {

            if (UI_PauseMenu.IsOn == true)
                return;

            // move
            velocity.z = moveSettings.forwardVel * forwardInput;

        } else {
            // zero velocity

            velocity.z = 0;
        }

        if (UI_PauseMenu.IsOn == true) {
            // zero velocity

            velocity.z = 0;
        }

    }

    void CheckMovementAudio() {
        if (playerAnimator.GetBool("InAir") == false && playerAnimator.GetBool("Moving") == true && playerStep == true && playerAnimator == true && !Input.GetButton("Jump")) {

            if (cycleMovement == 0)
            {
                netSoundscape.PlaySound(0, 3, 0.1f, 0);
                cycleMovement++;

            } else if (cycleMovement == 1)
            {
                netSoundscape.PlaySound(1, 3, 0.1f, 0);
                cycleMovement--;
            }

            StartCoroutine(WaitForFootSteps(audioStepLength));
        }
    }

    private IEnumerator WaitForFootSteps(float stepsLength) {
        playerStep = false;
        yield return new WaitForSeconds(stepsLength);
        playerStep = true;
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

    void Turn() { //we don't use this

        if (UI_PauseMenu.IsOn == true)
            return;

        if (!gravityAxisScript.GetGravitySwitching()) {
            targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVel * Input.GetAxis("Mouse X") * 0.015f, Vector3.up);

        }

        rotY -= Input.GetAxis("Mouse Y") * 2f;
        rotY = Mathf.Clamp(rotY, -90f, 60f);
        camY = Mathf.Clamp(rotY - cameraDisplacement * 30f, -90f, 60f);
        eyes.transform.localRotation = Quaternion.Lerp(eyes.transform.localRotation, Quaternion.Euler(camY, 0, 0), Time.deltaTime * 30f);
        

        //orbit.yRotation += hOrbitMouseInput * orbit.hOrbitSmooth * Time.deltaTime; no

        transform.localRotation = targetRotation;
    }
    void CheckPause() {
        if (UI_PauseMenu.IsOn) {
            if (!Grounded()) {
                playerAnimator.SetBool("InAir", true);
                if (CanFall()) {
                    velocity.y -= physSettings.downAccel;
                }
                velocity.y = Mathf.Max(velocity.y, -100);
            } else {
                if (jumpInput == 0 && Grounded()) {
                    playerAnimator.SetBool("Moving", false);
                    velocity.z = 0;
                }
            }
        }
    }

    public void Jump() {
        if (UI_PauseMenu.IsOn == true)
            return;

        if (jumpInput > 0 && Grounded() && !gravityAxisScript.GetGravitySwitching() && !jumping) {
            // Jumping - Alex
            StartCoroutine(JumpTime());
            jumping = true;
            //velocity.y = moveSettings.jumpVel;
        } else if (jumpInput == 0 && Grounded()) {

            //set the anim to not jumping and spawn a blast wave
            playerAnimator.SetBool("InAir", false);
            if (!hasLanded && velocity.y <= -25) {
                Instantiate(blastWave, this.gameObject.transform);
            }
            hasLanded = true;

            // zero out our velocity.y
            if (!jumping) {
                velocity.y = 0;
            }

        } else {
            // decrease velocity.y
            playerAnimator.SetBool("InAir", true);
            hasLanded = false;
            PauseFall();
            if (CanFall()) {
                velocity.y -= physSettings.downAccel;
            }
            velocity.y = Mathf.Max(velocity.y, -100);
        }
    }

    public void ActualJump() {
        velocity.y = moveSettings.jumpVel;
    }

    IEnumerator JumpTime() {
        playerAnimator.SetBool("Jump", true);
        yield return new WaitForSeconds(1f);
        playerAnimator.SetBool("Jump", false);
        jumping = false;
    }

    bool CanFall() {
        if (!isDashing && !gravityAxisScript.GetGravitySwitching()) {
            return true;
        } else {
            velocity.y *= retainFallOnGrav;
            return false;
        }
    }

    void PauseFall() {

        if (gravityAxisScript.GetGravitySwitching() && !fallPaused) {
            tempYVel = velocity.y;
            fallPaused = true;
            velocity.y = 0;
            //Debug.Log("i did a thing " + tempYVel);
        } else if (!gravityAxisScript.GetGravitySwitching() && fallPaused) {
            velocity.y = tempYVel;
            fallPaused = false;
            //Debug.Log("I did another thing " + tempYVel);
        }

        //Debug.Log("ytemp" + tempYVel);
    }
}
