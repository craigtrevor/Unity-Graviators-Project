using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class damageRange : NetworkBehaviour {

    private string sourceID;

    // Sword Stats
    public int swordDamage = 30;
    public float rotateSpeed = 500;
    //slow Variables
    [SerializeField]
    private int reducedWalkSpeed = 6;
    [SerializeField]
    private int normalWalkSpeed = 12;

    [SerializeField]
    private int reducedJumpSpeed = 7;
    [SerializeField]
    private int normalJumpSpeed = 15;

    public float stunTime = 2f;

    public bool dying = false;
    public float deathCount;
    public ParticleManager particleManager;
    //public GameObject colliderFrame;

    // D1
    public int d1Damage;

    //Sparkus
    public int sparkusDamage = 30;

    private Collider[] hitColliders;
    private Vector3 attackOffset;

    private float attackRadius;
    private const string PLAYER_TAG = "Player";
    private const string BOT_TAG = "NetBot";
    private const string THROWINGSWORD_TAG = "ThrowingSword";
    private const string UNITD1RANGEWEAPON_TAG = "UnitD1_RangedWeapon";
    private const string SPARKUSRANGEWEAPON_TAG = "Sparkus_RangedWeapon";

    public float d1StunTime = 2f;
    public float sparkusStunTime = 5f;
    public float nonameSlowTime = 2f;

    // scripts
    Network_PlayerManager networkPlayerManager;
    Network_Soundscape networkSoundscape;
    PlayerController playerController;

    void Awake() {
        particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
    }

    void SetInitialReferences(string _sourceID) {
        sourceID = _sourceID;
        //Debug.Log("GotRef: " + sourceID + " from " + _sourceID);
    }

    void SetRight() {
        rotateSpeed *= -1;
    }

    void transformSparkusRanged() {
        //transform.Translate(Vector3.forward * 0.1f);
        //transform.localScale += new Vector3(0.03f, 0.03f, 0f);

        transform.localScale = Vector3.one * 2f;

        Destroy(this.gameObject, 2f);
    }

    void Update() {
        if (dying) {
            Die();
        }

        if (this.gameObject.tag == THROWINGSWORD_TAG && !dying) {
            transform.Rotate(Vector3.down, rotateSpeed * Time.deltaTime);
        }

        if (this.gameObject.tag == SPARKUSRANGEWEAPON_TAG) {
            transformSparkusRanged();
        }
    }

    //private void FixedUpdate() {
    //    SparkusRanged(sparkusDamage);
    //}

    //[Client]
    //public void SparkusRanged() {

    //    if (this.gameObject.tag == SPARKUSRANGEWEAPON_TAG) { // if sparkus range weapon hit the player
    //        Debug.Log("boop");

    //        //hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);
    //        hitColliders = Physics.OverlapBox(colliderFrame.transform.position, colliderFrame.transform.localScale / 2f);

    //        foreach (Collider hitCol in hitColliders) {
    //            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && sourceID != hitCol.gameObject.name) {
    //                /*Debug.Log("Hit Player " + hitCol.transform.name);*/
    //                /*Debug.Log("sourceid is " + sourceID);*/

    //                CmdTakeDamage(hitCol.gameObject.name, sparkusDamage, sourceID);
    //            }
    //        }
    //    }
    //}

    [Client]
    void OnCollisionEnter(Collision other) {
        if (other.transform.root != transform.root && (other.gameObject.tag == PLAYER_TAG || other.gameObject.tag == BOT_TAG) && other.transform.name != sourceID) {
            if (this.gameObject.tag == THROWINGSWORD_TAG && other.gameObject.tag != THROWINGSWORD_TAG)// if a throwing sword hit the player
            {
                GameObject temp2 = new GameObject();
                temp2.transform.SetParent(other.gameObject.transform);
                transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
                GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                if (other.gameObject.tag == PLAYER_TAG) {
                    CmdTakeDamage(other.gameObject.name, swordDamage, sourceID);
                    other.gameObject.GetComponent<Network_CombatManager>().SlowForSeconds(nonameSlowTime);
                }

                if (other.gameObject.tag == BOT_TAG) {
                    other.gameObject.GetComponent<Network_Bot>().TakeDamage(swordDamage);
                    other.gameObject.GetComponent<Network_Bot>().Slow(nonameSlowTime);
                }
                //networkSoundscape = GameObject.Find(sourceID).transform.GetComponent<Network_Soundscape>();
                //PlayImpactSound();
                Die();
            }

            if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG) // if UnitD1 range weapon hit the player
            {
                GameObject temp2 = new GameObject();
                temp2.transform.SetParent(other.gameObject.transform);
                transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
                GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                if (other.gameObject.tag == PLAYER_TAG) {
                    CmdTakeDamage(other.gameObject.name, d1Damage, sourceID);
                    other.gameObject.GetComponent<Network_CombatManager>().StunForSeconds(d1StunTime);
                }

                if (other.gameObject.tag == BOT_TAG) {
                    other.gameObject.GetComponent<Network_Bot>().TakeDamage(d1Damage);
                    other.gameObject.GetComponent<Network_Bot>().Stun(d1StunTime);
                }
                //networkSoundscape = GameObject.Find(sourceID).transform.GetComponent<Network_Soundscape>();
                //PlayImpactSound();
                Die();
            }
            //Sparkus ranged handled by OnTrigger
            /*
            if (this.gameObject.tag == SPARKUSRANGEWEAPON_TAG) // if sparkus range weapon hit the player
            {
                if (other.gameObject.tag == PLAYER_TAG) {
                    CmdTakeDamage(other.gameObject.name, sparkusDamage, sourceID);
                    other.gameObject.GetComponent<Network_CombatManager>().StunForSeconds(sparkusStunTime);
                }

                if (other.gameObject.tag == BOT_TAG) {
                    other.gameObject.GetComponent<Network_Bot>().TakeDamage(sparkusDamage);
                    other.gameObject.GetComponent<Network_Bot>().Slow(sparkusStunTime);
                }
                //Die();
            }*/
        } else if (other.transform.root != transform.root && other.gameObject.tag != PLAYER_TAG && other.transform.name != sourceID) {
            if (this.gameObject.tag == THROWINGSWORD_TAG)// if a throwing sword hit the arena
            {
                GameObject temp2 = new GameObject();
                temp2.transform.SetParent(other.gameObject.transform);
                transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
                GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                //PlayImpactSound();
                Die();
            }

            if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG) // if UnitD1 range weapon hit the arena
            {
                GameObject temp2 = new GameObject();
                temp2.transform.SetParent(other.gameObject.transform);
                transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
                GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                //PlayImpactSound();
                Die();
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.root != transform.root && (other.gameObject.tag == PLAYER_TAG || other.gameObject.tag == BOT_TAG) && other.transform.name != sourceID) {
            if (this.gameObject.tag == SPARKUSRANGEWEAPON_TAG) // if sparkus range weapon hit the player
            {
                if (other.gameObject.tag == PLAYER_TAG) {
                    CmdTakeDamage(other.gameObject.name, sparkusDamage*Time.deltaTime, sourceID);
                    other.gameObject.GetComponent<Network_CombatManager>().StunForSeconds(sparkusStunTime);
                    Debug.Log("sourceID is: " + sourceID);
                    Debug.Log("hit: " + other.transform.name);
                }

                if (other.gameObject.tag == BOT_TAG) {
                    other.gameObject.GetComponent<Network_Bot>().TakeDamage(sparkusDamage * Time.deltaTime);
                    other.gameObject.GetComponent<Network_Bot>().Slow(sparkusStunTime);
                }
                //Die();
            }
        }
    }

    void PlayImpactSound() {
        if (this.gameObject.tag == THROWINGSWORD_TAG) {
            networkSoundscape.PlayNonNetworkedSound(11, 1, 0.2f);
        }

        if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG) {
            networkSoundscape.PlayNonNetworkedSound(12, 1, 0.2f);
        }
    }

    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    public void Die() {
        if (!dying) {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<MeleeWeaponTrail>());
            Destroy(GetComponent<BoxCollider>());

            dying = true;
            StartCoroutine(DieNow());
        }
    }

    private IEnumerator DieNow() {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
