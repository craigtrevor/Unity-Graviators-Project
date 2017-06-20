using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundscape_PlayerController : MonoBehaviour {

    public Animator anim;

    public AudioSource[] playerSounds;
    public AudioSource playerMovementAudio;
    public AudioSource playerCombatAudio;
    public AudioSource atmosFallAudio;
    //public AudioSource sfxAudio;

    public AudioClip[] metalFootstepSounds;
    public AudioClip[] woodFootstepSounds;
    public AudioClip[] gravitySounds;
    public AudioClip[] weaponSounds;
    public AudioClip[] hitSounds;
    //public AudioClip[] buttonSounds;
    public AudioClip jumpSound;

    private bool weaponSoundPlayed;
    private bool hitSoundPlayed;
    private bool jumpSoundPlayed;
    private bool buttonSoundPlayed;

    public bool gravitySoundPlayed;
    public bool fallSoundPlayed;

    private bool playerStep;
    private float audioStepLength = 0.45f;

    Combat_Manager combatManager;
    PlayerController playerController;

    void Awake()
    {
        atmosFallAudio.volume = 0.0f;
        atmosFallAudio.Play();
    }

	// Use this for initialization
	void Start ()
    {
        playerSounds = GetComponents<AudioSource>();
        playerMovementAudio = playerSounds[0];
        playerCombatAudio = playerSounds[1];

        gravitySoundPlayed = false;
        weaponSoundPlayed = false;
        hitSoundPlayed = false;
        jumpSoundPlayed = false;
        fallSoundPlayed = false;
        playerStep = true;

        combatManager = GetComponent<Combat_Manager>();
        playerController = GetComponentInChildren<PlayerController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GravityCheck();
        AttackCheck();
        JumpCheck();
        FallCheck();
    }

    //void OnTriggerEnter(Collider hit)
    //{
    //    if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && hit.tag == "Metal" && playerStep == true && !Input.GetButton("Jump"))
    //    {
    //        Debug.Log("Walking");
    //        WalkOnMetal();
    //    }

    //    if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && hit.tag == "Wood" && playerStep == true && !Input.GetButton("Jump"))
    //    {
    //        WalkOnWood();
    //    }

    //    //if (hit.tag == "Button 1" && !buttonSoundPlayed)
    //    //{
    //    //    sfxAudio.clip = buttonSounds[Random.Range(0, buttonSounds.Length)];
    //    //    sfxAudio.volume = 0.03f;
    //    //    sfxAudio.Play();
    //    //    buttonSoundPlayed = true;
    //    //    colorParticles[0].SetActive(true);
    //    //}

    //    //if (hit.tag == "Button 2" && !buttonSoundPlayed)
    //    //{
    //    //    sfxAudio.clip = buttonSounds[Random.Range(0, buttonSounds.Length)];
    //    //    sfxAudio.volume = 0.03f;
    //    //    sfxAudio.Play();
    //    //    buttonSoundPlayed = true;
    //    //    colorParticles[1].SetActive(true);
    //    //}

    //    //if (hit.tag == "Button 3" && !buttonSoundPlayed)
    //    //{
    //    //    sfxAudio.clip = buttonSounds[Random.Range(0, buttonSounds.Length)];
    //    //    sfxAudio.volume = 0.03f;
    //    //    sfxAudio.Play();
    //    //    buttonSoundPlayed = true;
    //    //    colorParticles[2].SetActive(true);
    //    //}
    //}

    void OnTriggerEnter(Collider other)
    {
        if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && other.tag == "Metal" && playerStep == true && !Input.GetButton("Jump"))
        {
            WalkOnMetal();
        }

        if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && other.tag == "Wood" && playerStep == true && !Input.GetButton("Jump"))
        {
            WalkOnWood();
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && other.tag == "Metal" && playerStep == true && !Input.GetButton("Jump"))
        {
            WalkOnMetal();
        }

        if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && other.tag == "Wood" && playerStep == true && !Input.GetButton("Jump"))
        {
            WalkOnWood();
        }

    }

    //void OnTriggerStay(Collider hit)
    //{
    //    if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && hit.tag == "Metal" && playerStep == true && !Input.GetButton("Jump"))
    //    {
    //        Debug.Log("Walking");
    //        WalkOnMetal();
    //    }

    //    if (anim.GetBool("InAir") == false && anim.GetBool("Moving") == true && hit.tag == "Wood" && playerStep == true && !Input.GetButton("Jump"))
    //    {
    //        WalkOnWood();
    //    }

    //    //if (hit.tag == "Button 1" && !buttonSoundPlayed)
    //    //{
    //    //    sfxAudio.clip = buttonSounds[Random.Range(0, buttonSounds.Length)];
    //    //    sfxAudio.volume = 0.03f;
    //    //    sfxAudio.Play();
    //    //    buttonSoundPlayed = true;
    //    //    colorParticles[0].SetActive(true);
    //    //}

    //    //if (hit.tag == "Button 2" && !buttonSoundPlayed)
    //    //{
    //    //    sfxAudio.clip = buttonSounds[Random.Range(0, buttonSounds.Length)];
    //    //    sfxAudio.volume = 0.03f;
    //    //    sfxAudio.Play();
    //    //    buttonSoundPlayed = true;
    //    //    colorParticles[1].SetActive(true);
    //    //}

    //    //if (hit.tag == "Button 3" && !buttonSoundPlayed)
    //    //{
    //    //    sfxAudio.clip = buttonSounds[Random.Range(0, buttonSounds.Length)];
    //    //    sfxAudio.volume = 0.03f;
    //    //    sfxAudio.Play();
    //    //    buttonSoundPlayed = true;
    //    //    colorParticles[2].SetActive(true);
    //    //}
    //}

    void WalkOnMetal()
    {
        playerMovementAudio.clip = metalFootstepSounds[Random.Range(0, metalFootstepSounds.Length)];
        playerMovementAudio.volume = 0.03f;
        playerMovementAudio.Play();
        StartCoroutine(WaitForFootSteps(audioStepLength));
    }

    void WalkOnWood()
    {
        playerMovementAudio.clip = woodFootstepSounds[Random.Range(0, woodFootstepSounds.Length)];
        playerMovementAudio.volume = 0.06f;
        playerMovementAudio.Play();
        StartCoroutine(WaitForFootSteps(audioStepLength));
    }

    private IEnumerator WaitForFootSteps(float stepsLength)
    {
        playerStep = false;
        yield return new WaitForSeconds(stepsLength);
        playerStep = true;
     }

    void GravityCheck()
    {
        GravityAxisScript _gravityAxisScript = this.GetComponentInChildren<GravityAxisScript>();

        if (_gravityAxisScript.gravitySwitching == true && _gravityAxisScript.gravityCharge < 2000)
        {
            MinGravityCharge();
        }

        if (_gravityAxisScript.gravitySwitching == true && _gravityAxisScript.gravityCharge < 4000)
        {
            LowGravityCharge();
        }

        if (_gravityAxisScript.gravitySwitching == true && _gravityAxisScript.gravityCharge < 6000)
        {
            MidGravityCharge();
        }

        if (_gravityAxisScript.gravitySwitching == true && _gravityAxisScript.gravityCharge < 8000)
        {
            HighGravityCharge();
        }

        if (_gravityAxisScript.gravitySwitching == true && _gravityAxisScript.gravityCharge < 10000)
        {
            MaxGravityCharge();
        }

        if (_gravityAxisScript.gravitySwitching == false)
        {
            gravitySoundPlayed = false;
        }
    }

    void FallCheck()
    {
        if (anim.GetBool("InAir") == true && !fallSoundPlayed)
        {
            atmosFallAudio.volume = 0.05f;
        }

        if (anim.GetBool("InAir") == false && fallSoundPlayed)
        {
            atmosFallAudio.volume = 0.0f;
        }
    }

    void AttackCheck()
    {
        if (anim.GetBool("Attack") == true && !weaponSoundPlayed && !anim.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
        {
            playerCombatAudio.clip = weaponSounds[0];
            playerCombatAudio.volume = 0.05f;
            playerCombatAudio.Play();
            weaponSoundPlayed = true;
        }

        if (weaponSoundPlayed && anim.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
        {
            weaponSoundPlayed = false;
            hitSoundPlayed = false;
        }
    }

    void JumpCheck()
    {
        GravityAxisScript _gravityAxisScript = this.GetComponentInChildren<GravityAxisScript>();

        if (Input.GetButton("Jump") && !jumpSoundPlayed && _gravityAxisScript.gravitySwitching == false)
        {
            playerMovementAudio.clip = jumpSound;
            playerMovementAudio.volume = 0.1f;
            playerMovementAudio.Play();
            jumpSoundPlayed = true;
        }

        if (!Input.GetButton("Jump") && anim.GetBool("InAir") == false)
        {
            jumpSoundPlayed = false;
        }

        if (_gravityAxisScript.gravitySwitching == true)
        {
            fallSoundPlayed = false;
        }

        if (_gravityAxisScript.gravitySwitching == false)
        {
            fallSoundPlayed = true;
        }
    }

    void MinGravityCharge()
    {
        if (!gravitySoundPlayed)
        {
            playerMovementAudio.clip = gravitySounds[0];
            playerMovementAudio.volume = 0.06f;
            playerMovementAudio.Play();
            gravitySoundPlayed = true;
        }
    }

    void LowGravityCharge()
    {
        if (!gravitySoundPlayed)
        {
            playerMovementAudio.clip = gravitySounds[0];
            playerMovementAudio.volume = 0.07f;
            playerMovementAudio.Play();
            gravitySoundPlayed = true;
        }
    }

    void MidGravityCharge()
    {
        if (!gravitySoundPlayed)
        {
            playerMovementAudio.clip = gravitySounds[0];
            playerMovementAudio.volume = 0.08f;
            playerMovementAudio.Play();
            gravitySoundPlayed = true;
        }
    }

    void HighGravityCharge()
    {
        if (!gravitySoundPlayed)
        {
            playerMovementAudio.clip = gravitySounds[0];
            playerMovementAudio.volume = 0.09f;
            playerMovementAudio.Play();
            gravitySoundPlayed = true;
        }
    }

    void MaxGravityCharge()
    {
        if (!gravitySoundPlayed)
        {
            playerMovementAudio.clip = gravitySounds[0];
            playerMovementAudio.volume = 0.1f;
            playerMovementAudio.Play();
            gravitySoundPlayed = true;
        }
    }
}
