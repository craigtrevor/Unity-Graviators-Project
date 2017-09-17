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
    public GameObject colliderFrame;

    // D1
    public int d1Damage;

    //Sparkus
    public int sparkusDamage = 30;

    private Collider[] hitColliders;
    private Vector3 attackOffset;

    private float attackRadius;
    private const string PLAYER_TAG = "Player";
    private const string THROWINGSWORD_TAG = "ThrowingSword";
    private const string UNITD1RANGEWEAPON_TAG = "UnitD1_RangedWeapon";
    private const string SPARKUSRANGEWEAPON_TAG = "Sparkus_RangedWeapon";

    // scripts
    Network_PlayerManager networkPlayerManager;
    Network_Soundscape networkSoundscape;
    PlayerController playerController;

	void Awake() {
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
	}

    void SetInitialReferences(string _sourceID)
    {
        sourceID = _sourceID;
    }

    void SetRight()
    {
        rotateSpeed *= -1;
    }

    void transformSparkusRanged()
    {
        transform.Translate(Vector3.forward * 0.2f);
        transform.localScale += new Vector3(0.2f, 0.2f, 0f);

        Destroy(this.gameObject, 2f);
    }

    void Update()
    {
        if (dying)
        {
            Die();
        }

        if (this.gameObject.tag == THROWINGSWORD_TAG && !dying)
        {
            transform.Rotate(Vector3.down, rotateSpeed * Time.deltaTime);
        }

        if (this.gameObject.tag == SPARKUSRANGEWEAPON_TAG)
        {
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
	void OnCollisionEnter(Collision other)
    {
        if (other.transform.root != transform.root && other.gameObject.tag == PLAYER_TAG && other.transform.name != sourceID)
        {
            if (this.gameObject.tag == THROWINGSWORD_TAG)// if a throwing sword hit the player
            {
                CmdTakeDamage(other.gameObject.name, swordDamage, sourceID);
				GameObject temp2 = new GameObject ();
				temp2.transform.SetParent(other.gameObject.transform);
				transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
				GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                PlayImpactSound();
                Die();
            }

            if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG) // if UnitD1 range weapon hit the player
            {
				GameObject temp2 = new GameObject ();
				temp2.transform.SetParent(other.gameObject.transform);
				transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
				GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                CmdTakeDamage(other.gameObject.name, d1Damage, sourceID);
                PlayImpactSound();
                Die();
            }

            if (this.gameObject.tag == SPARKUSRANGEWEAPON_TAG) // if sparkus range weapon hit the player
            {
                CmdTakeDamage(other.gameObject.name, sparkusDamage, sourceID);
                Die();
            }
        }

        else if (other.transform.root != transform.root && other.gameObject.tag != PLAYER_TAG && other.transform.name != sourceID)
        {
            if (this.gameObject.tag == THROWINGSWORD_TAG)// if a throwing sword hit the player
            {
				GameObject temp2 = new GameObject ();
				temp2.transform.SetParent(other.gameObject.transform);
				transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
				GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                PlayImpactSound();
                Die();
            }

            if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG) // if UnitD1 range weapon hit the player
            {
				GameObject temp2 = new GameObject ();
				temp2.transform.SetParent(other.gameObject.transform);
				transform.SetParent(temp2.transform);
                transform.position = other.contacts[0].point;
				GameObject temp = Instantiate(particleManager.GetParticle("collideParticle"), this.gameObject.transform);
                temp.transform.position = other.contacts[0].point;
                PlayImpactSound();
                Die();
            }
        }
    }

    void PlayImpactSound()
    {
        networkSoundscape = GameObject.Find(sourceID).transform.GetComponent<Network_Soundscape>();

        if (this.gameObject.tag == THROWINGSWORD_TAG)
        {
            networkSoundscape.PlayNonNetworkedSound(11, 1, 0.2f);
        }

        if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG)
        {
            networkSoundscape.PlayNonNetworkedSound(12, 1, 0.2f);
        }
    }

    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
    {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    public void Die()
    {
        if (!dying)
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<MeleeWeaponTrail>());
            Destroy(GetComponent<BoxCollider>());

            dying = true;
            StartCoroutine(DieNow());
        }
    }

	private IEnumerator DieNow()
    {
		yield return new WaitForSeconds (5f);
		Destroy(gameObject);
	}
}
