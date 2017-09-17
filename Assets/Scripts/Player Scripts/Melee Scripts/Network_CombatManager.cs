using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_CombatManager : NetworkBehaviour {

    // Player Animator
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private NetworkAnimator netanim;

    private PlayerController playerController;
    public List<GameObject> safeList = new List<GameObject>();

    [SerializeField]
    Transform playerModel;

    string stateName;

    //textures
    public Renderer rend;

    // Rigidbody
    private Rigidbody playerRigidbody;
    private const string PLAYER_TAG = "Player";

    private Collider[] hitColliders;

    public Transform cameraRotation; // the camerasrotation, assign in game editor

    // Int
    public float playerDamage = 25;
    public int thrust = 2000; //change this for speed of knock back
    public float delay = 0.2f;
    //private int attackMask;
    private Vector3 attackOffset;

    // Float
    private float attackRadius;
    [SerializeField]
    public float lowDamageVelocity = 40;
    [SerializeField]
    public float highDamageVelocity = 80;

    //stun timers
    private float d1StunTime = 2f;
    private float sparkusStunTime = 5f;

    // slow info
    public float slowTime = 2f;
    private int reducedWalkSpeed = 6;
    private int normalWalkSpeed = 12;

    private int reducedJumpSpeed = 7;
    private int normalJumpSpeed = 15;

    // Boolean
    public bool isAttacking;
    public bool isHitting;
    public bool isUlting;
    [SerializeField]
    public bool animationPlaying;


    // Floats
    private float speed;
    [SerializeField]
    public float ultGain;

    // Ints
    [SerializeField]
    private int attackCounter;

    //particles
    public ParticleManager particleManager;

    // Scripts
    Network_Soundscape networkSoundscape;
    Network_PlayerManager networkPlayerManager;
    Dash dashScript;
    WeaponSpawn weaponSpawn;

    public bool clashActive;
    public bool alwaysAttack;

    // Use this for initialization
    void Awake() {
        playerController = transform.GetComponentInChildren<PlayerController>();
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        weaponSpawn = transform.GetComponent<WeaponSpawn>();
        playerRigidbody = transform.GetComponent<Rigidbody>();
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();
        particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();

        playerDamage = 5;
        attackRadius = 3;
        attackCounter = 0;

        isUlting = false;

        //anim = GetComponent<Animator>();
        //anim.speed = 0.2f;
    }

    void Update() {
        CheckAnimation();
        AttackPlayer();
        PlayerVelocity();
        //Debug.Log("isulting "+isUlting);
    }

    void OnTriggerEnter(Collider other) {
        if (!isUlting) {
            if (!safeList.Contains(other.gameObject)) {
                if (other.tag == "UnitD1_RangedWeapon") {
                    StartCoroutine(stunTimer(d1StunTime, false));
                }
                if (other.tag == "ThrowingSword") {
                    StartCoroutine(slowTimer(slowTime, false));
                }
                if (other.tag == "Sparkus_Ranged" || other.tag == "D1_Ult") {
                    StartCoroutine(stunTimer(sparkusStunTime, false));
                }
            }

            if (other.tag == "collider") {
                GameObject playGravLandMed = Instantiate(particleManager.GetParticle("gravLandParticleMed"), this.transform.position + Vector3.down, this.transform.rotation);
            }
        }
    }

    public void StunForSeconds(float stunTime) {
        StartCoroutine(slowTimer(stunTime, true));
    }

    IEnumerator stunTimer(float stunTime, bool ignoreLocalWarning) {
        if (!isLocalPlayer || ignoreLocalWarning) {
            playerController.stunned = true;
            anim.SetBool("Stun", true);
            GameObject stunParticle = Instantiate(particleManager.GetParticle("stunEffect"), this.transform.position + Vector3.down, this.transform.rotation);
            stunParticle.GetComponent<DestroyParticle>().delayBeforeDeath = stunTime;
            yield return new WaitForSeconds(stunTime);
        }
        playerController.stunned = false;
        anim.SetBool("Stun", false);
    }

    public void SlowForSeconds(float slowTime) {
        StartCoroutine(slowTimer(slowTime, true));
    }

    public IEnumerator slowTimer(float slowTime, bool ignoreLocalWarning) {
        if (!isLocalPlayer || ignoreLocalWarning) {
            playerController.moveSettings.forwardVel = reducedWalkSpeed;
            playerController.moveSettings.rightVel = reducedWalkSpeed;
            playerController.moveSettings.jumpVel = reducedJumpSpeed;
            yield return new WaitForSeconds(slowTime);
        }
        playerController.moveSettings.forwardVel = normalWalkSpeed;
        playerController.moveSettings.rightVel = normalWalkSpeed;
        playerController.moveSettings.jumpVel = normalJumpSpeed;
    }

    void CheckAnimation() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            anim.SetBool("Attacking", false);
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("UltimateDash")) {
            anim.SetBool("Attacking", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            anim.SetBool("Jump", false);
        }
    }

    void PlayMeleeSound() {
        if (networkPlayerManager.playerCharacterID == "ERNN") {
            networkSoundscape.PlaySound(5, 0, 0.2f, 0f);
        } else if (networkPlayerManager.playerCharacterID == "SPKS") {
            networkSoundscape.PlaySound(6, 0, 0.2f, 0f);
        } else if (networkPlayerManager.playerCharacterID == "UT-D1") {
            networkSoundscape.PlaySound(7, 0, 0.2f, 0f);
        }
    }

    void AttackPlayer() {
        if (UI_PauseMenu.IsOn == true)
            return;

        if (Input.GetKeyUp(KeyCode.Mouse0) && !isAttacking && !isUlting && !weaponSpawn.m_Fired) {
            anim.SetBool("Attacking", true);
            netanim.SetTrigger("Attack");
            PlayMeleeSound();
            isAttacking = true;
        }

        if (alwaysAttack && !isAttacking && !isUlting) {
            anim.SetBool("Attacking", true);
            netanim.SetTrigger("Attack");
            PlayMeleeSound();
            isAttacking = true;
        }

        if (isAttacking) {
            Attacking();
        }

        if (isLocalPlayer && Input.GetKeyUp(KeyCode.K)) {
            playerDamage = 10;
            CmdTakeDamage(transform.name, playerDamage, transform.name);
        }
    }

    public void AttackFinished() {
        attackCounter = 0;
        isAttacking = false;
        anim.SetBool("Attacking", false);
    }

    [Client]
    public void Attacking() {
        //CheckCollision();
    }

    public void weaponCollide(Collider collision, Vector3 hitPoint, bool airStrike) {
        //Debug.Log (collision.gameObject.name + " struck at " + hitPoint);
        if (isAttacking) {
            GameObject tempParticle = Instantiate(particleManager.GetParticle("grindParticle"));
            tempParticle.transform.position = hitPoint;
            if ((airStrike && !playerController.Grounded()) || !airStrike) {
                if (collision.gameObject.tag == PLAYER_TAG) {
                    isHitting = true;

                    if (!clashActive) {
                        if (attackCounter == 0) {
                            SendDamage(collision, airStrike);
                            attackCounter = 1;
                        }
                    } else if (clashActive) {
                        // Clash Active
                        if (collision.gameObject.GetComponent<Network_CombatManager>().isAttacking == true) { // check to see if the other player is attacking
                            if (collision.gameObject.GetComponent<Network_CombatManager>().playerDamage == this.GetComponent<Network_CombatManager>().playerDamage) {  // if the player has equal damage as oppenent
                                StartCoroutine(KnockBack(collision, hitPoint));
                            }
                        } else if (collision.gameObject.GetComponent<Network_CombatManager>().playerDamage < this.GetComponent<Network_CombatManager>().playerDamage) { // if the player has more damage then oponent
                            if (attackCounter == 0) {
                                SendDamage(collision, airStrike);
                                attackCounter = 1;
                            }

                        } else {
                            StartCoroutine(KnockBack(collision, hitPoint));
                        }
                    }
                } else if (clashActive) {
                    StartCoroutine(KnockBack(collision, hitPoint));
                }
            }
        }
    }

    void SendDamage(Collider hitCol, bool airStrike) {
        if (isHitting) {
            if (airStrike) {
                CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
                StartCoroutine(AttackDelay());
            }
            if (networkPlayerManager.playerCharacterID == "ERNN" && !airStrike) {
                CmdTakeDamage(hitCol.gameObject.name, playerDamage / 15, transform.name);
                StartCoroutine(AttackDelay());
                //GetComponent<Dash>().chargePercent += ultGain;
            } else if (networkPlayerManager.playerCharacterID == "SPKS" && !airStrike) {
                CmdTakeDamage(hitCol.gameObject.name, playerDamage / 2, transform.name);
                StartCoroutine(AttackDelay());
            } else if (networkPlayerManager.playerCharacterID == "UT-D1" && !airStrike) {
                CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
                StartCoroutine(AttackDelay());
            }
        }
    }

    IEnumerator AttackDelay() {
        yield return new WaitForSeconds(0.1f);
        attackCounter = 0;
    }

    IEnumerator KnockBack(Collider hitCol, Vector3 hitPoint) {
        isAttacking = false;
        gameObject.GetComponentInChildren<PlayerController>().enabled = false;
        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        playerRigidbody.AddExplosionForce(1000, hitPoint, 10f);
        yield return new WaitForSeconds(0.5f);
        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        gameObject.GetComponentInChildren<PlayerController>().enabled = true;
    }

    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    void PlayerVelocity() {
        speed = playerRigidbody.velocity.magnitude;

        if (speed < lowDamageVelocity) {
            playerDamage = 25.0f;
            ultGain = 5;
        } else if (lowDamageVelocity < speed && speed < highDamageVelocity) {
            playerDamage = 50.0f;
            ultGain = 10;
        } else if (highDamageVelocity < speed) {
            playerDamage = 70.0f;
            ultGain = 20;
        }
    }

}
