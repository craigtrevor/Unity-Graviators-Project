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
	public GameObject NoNameCollideParticle;

    // D1
    public int d1Damage;

    //Sparkus
    public int sparkusDamage;

    private Collider[] hitColliders;
    private Vector3 attackOffset;
    private float attackRadius;
    private const string PLAYER_TAG = "Player";
    private const string THROWINGSWORD_TAG = "ThrowingSword";
    private const string UNITD1RANGEWEAPON_TAG = "UnitD1_RangedWeapon";
    private const string SPARKUSRANGEWEAPON_TAG = "Sparkus_RangedWeapon";

    // scripts
    Network_PlayerManager networkPlayerManager;
    PlayerController playerController;

    void SetInitialReferences(string _sourceID) {
        sourceID = _sourceID;
    }

    void SetRight() {
        rotateSpeed *= -1;
    }

    void transformSparkusRanged() {
        transform.Translate(Vector3.forward * 0.2f);
        transform.localScale += new Vector3(0.2f, 0.2f, 0f);

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

    [Client]
    void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" && this.tag != SPARKUSRANGEWEAPON_TAG) {
            transform.SetParent(other.gameObject.transform);
            Die();
        } else {

            //Physics.IgnoreLayerCollision (10, 30); // the ranged object will ignore the local player layer
            hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

            foreach (Collider hitCol in hitColliders) {
                if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && hitCol.transform.name != sourceID) {

                    if (this.gameObject.tag == THROWINGSWORD_TAG)// if a throwing sword hit the player
                    {
                        CmdTakeDamage(hitCol.gameObject.name, swordDamage, sourceID);
                        transform.SetParent(hitCol.gameObject.transform);
						Die();
                    }


                    if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG) // if UnitD1 range weapon hit the player
                    {
                        CmdTakeDamage(hitCol.gameObject.name, d1Damage, sourceID);
						Die();
                    }

                    if (this.gameObject.tag == SPARKUSRANGEWEAPON_TAG) // if UnitD1 range weapon hit the player
                    {
                        CmdTakeDamage(hitCol.gameObject.name, sparkusDamage, sourceID);
						Die();
                    }
                }
            }
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
			Instantiate (NoNameCollideParticle, this.transform);
            dying = true;
            deathCount = 0.25f;
        }
        deathCount += Time.time * 0.0005f;

        if (deathCount >= 1 || true) {
            Destroy(this.gameObject);
        }
    }

}
