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
    public GameObject blastWave;
    public bool hasLanded;

    public GameObject gravityAxis;
    public GameObject gravityBlock;

    public GameObject stunParticle;
    //GameObject playStun;

    GravityAxisScript gravityAxisScript;
    GravityBlockScript gravityBlockScript;
    Network_CombatManager netCombatManager;
    Network_Soundscape networkSoundscape;

    public float cameraDisplacement;
    public bool stunned;
    public bool isDashing;
    private bool recieveInput;
    private bool isShiftPressed;

    public int retainFallOnGrav;

    public GameObject sphere;

    public Quaternion TargetRotation {
        get { return targetRotation; }
    }

    public bool Grounded() {
        //return Physics.Raycast(transform.position, -transform.up, moveSettings.distToGrounded, moveSettings.ground);
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        float radius = capsuleCollider.radius * 0.65f;
        Vector3 pos = capsuleCollider.transform.position + -capsuleCollider.transform.up /** radius * 1.7f */* (capsuleCollider.height / 2 - 0.25f);

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

        //  playStun = (GameObject)Instantiate(stunParticle, this.transform.position + this.transform.up * 2f, /*Quaternion.Euler(this.transform.eulerAngles.x - 90f, this.transform.eulerAngles.y, this.transform.eulerAngles.z)*/this.transform.rotation, this.transform);

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

        recieveInput = true;
        isShiftPressed = false;
        stunned = false;
        inputSettings.GRAVITY_RELEASE = false;
    }

    void GetInput() {
        if (recieveInput && !stunned && !isDashing) {
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS); // interpolated 
            rightInput = Input.GetAxis(inputSettings.RIGHT_AXIS); // interpolated 
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS); // interpolated    
            jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS); // non-interpolated
        } else {
            forwardInput = rightInput = turnInput = jumpInput = 0f;
        }

        shiftPressed();

        if (!stunned && !isDashing) {
            GravityInput(inputSettings.GRAVITY_RELEASE);
        }

        if (isDashing) {
            CancelVelocity();
        }
    }

    void CancelVelocity() {
        velocity = Vector3.zero;
    }

    public void StartStun(float time) {
        StartCoroutine(Stun(Time.time + time));
    }

    IEnumerator Stun(float time) {
        GameObject playStun = (GameObject)Instantiate(stunParticle, this.transform.position + this.transform.up * 2f, /*Quaternion.Euler(this.transform.eulerAngles.x - 90f, this.transform.eulerAngles.y, this.transform.eulerAngles.z)*/this.transform.rotation, this.transform);
        playStun.GetComponent<ParticleSystem>().Play();
        while (Time.time < time) {
            stunned = true;
            //Debug.Log("I am stunning");
            yield return null;
        }
        playStun.GetComponent<ParticleSystem>().Stop();
        playStun.GetComponent<ParticleSystem>().Clear();
        //Destroy(playStun);
        stunned = false;
        //Debug.Log("I'm free");

    }

    void shiftPressed()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isShiftPressed = true;
        }

        if (Input.GetButtonUp("Crouch"))
        {
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
                    //networkSoundscape.PlaySound(22, 4, 0f);
                    //isShiftPressed = false;
                }

                if (Input.GetButtonDown("Horizontal")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(0f, Input.GetAxis("Horizontal"), 0f);
                    //networkSoundscape.PlaySound(22, 4, 0f);
                    //isShiftPressed = false;
                }

                if (Input.GetButtonDown("Vertical")) {
                    // Used Gravity || in the air - Alex
                    gravityAxisScript.ChangeGravity(0f, 0f, Input.GetAxis("Vertical"));
                    //networkSoundscape.PlaySound(22, 4, 0f);
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
    public GameObject eyes;

    void Update() {

        GetInput();
        Turn();

        gravityBlockScript.UpdatePosition(); //Update gravity block position
    }

    void FixedUpdate() {

        Run();
        Strafe();
        ActualJump();
        CheckPause();

        cameraDisplacement = Mathf.Min((velocity.y + 20f) / 30f, 0f);
        //print(cameraDisplacement);

        rBody.velocity = transform.TransformDirection(velocity);
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
        rotY = Mathf.Clamp(rotY, -90f, 40f);
        eyes.transform.localRotation = Quaternion.Lerp(eyes.transform.localRotation, Quaternion.Euler(rotY - cameraDisplacement * 15, 0, 0), Time.deltaTime * 30);

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

    public void ActualJump()
    {
        if (UI_PauseMenu.IsOn == true)
            return;

        if (jumpInput > 0 && Grounded() && !gravityAxisScript.GetGravitySwitching())
        {
            // Jumping - Alex
            StartCoroutine(JumpTime());
            velocity.y = moveSettings.jumpVel;
            //networkSoundscape.PlaySound(22, 4, 0f);
        }
        else if (jumpInput == 0 && Grounded())
        {

            //set the anim to not jumping and spawn a blast wave
            playerAnimator.SetBool("InAir", false);
            if (!hasLanded && velocity.y <= -25)
            {
                Instantiate(blastWave, this.gameObject.transform);
            }
            hasLanded = true;

            // zero out our velocity.y
            velocity.y = 0;

        }
        else
        {
            // decrease velocity.y
            playerAnimator.SetBool("InAir", true);
            hasLanded = false;
            if (CanFall())
            {
                velocity.y -= physSettings.downAccel;
            }
            velocity.y = Mathf.Max(velocity.y, -100);
        }
    }

    IEnumerator JumpTime() {
        playerAnimator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("Jump", false);
    }

    bool CanFall() {
        if (!isDashing && !gravityAxisScript.GetGravitySwitching()) {
            return true;
        } else {
            velocity.y *= retainFallOnGrav;
            return false;
        }
    }
}
