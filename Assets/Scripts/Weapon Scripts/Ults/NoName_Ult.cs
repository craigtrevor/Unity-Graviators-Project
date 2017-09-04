using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NoName_Ult : NetworkBehaviour {

    private const string ULTCHARGER_TAG = "UltCharger";

    public GameObject player;
    private PlayerController playerScript;
    private PlayerCamera cameraScript;

    public const float DASH_SPEED = 50f;
    public const float MAX_DISTANCE = 40f; 
    public const float TARGET_THRESH = 2f; //distance considered "close enough" to target
    public const float DASH_MAX = 100f;
    public const float DASH_COST = DASH_MAX / 3f;

    [SerializeField]
    private bool isDashing = false;
    [SerializeField]
    private bool isCharging = false;
    [SerializeField]
    private bool onPause = false;
    [SerializeField]
    private bool canDash = false;

    //dash damage storage
    [SerializeField]
    private float dashDamage = 20f;
	public float chargeMax = 100;
	private float chargeAmount = 15f;
    private Collider[] hitColliders;
    private Vector3 attackOffset;
    private float attackRadius;
    private const string PLAYER_TAG = "Player";
    
    float charge = 0; // the amount of charge
    public float passiveCharge; // the amount of charge gained passively;

    // Scripts
    Network_PlayerManager networkPlayerManager;
    Network_Soundscape networkSoundscape;

    public Animator playerAnimator;

    Vector3 startSpot;
    Vector3 target;

    // Use this for initialization
    void Start() {

        playerScript = player.GetComponent<PlayerController>();
        cameraScript = player.GetComponentInChildren<PlayerCamera>();

        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();

        attackRadius = 5;

    }

    // Update is called once per frame
    void Update() {

        DashInput();
        Dashing();

        if (onPause && isDashing) {
            StartCoroutine("Expire");
        } else {
            StopCoroutine("Expire");
        }

        ChargeUlt();

        Debug.DrawLine(startSpot, target);
    }  
    
    void ChargeUlt() { //deals with charging the ult bar
        if (!isDashing) {
            CmdChargeUltimate(passiveCharge, transform.name);
        }

        charge = networkPlayerManager.currentUltimateGain;
    }

	[Client]
	void OnTriggerStay(Collider other) //Ultimate charger - CB
	{
		if (this.gameObject.tag == PLAYER_TAG && other.gameObject.tag == ULTCHARGER_TAG)
		{
			//networkPlayerManager = other.GetComponent<Network_PlayerManager>();
			networkPlayerManager = this.gameObject.GetComponent<Network_PlayerManager>();
			Debug.Log(this.gameObject.name);
			Debug.Log(transform.name);
			CmdUltCharger(this.gameObject.name, chargeMax, transform.name);
		}
	}

	[Command]
	void CmdUltCharger(string _playerID, float _charge, string _sourceID)
	{
		Debug.Log(_playerID + " is charging up teh lazor.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcUltimateCharging(_charge, transform.name);
	}

    //Checks for dash input
    void DashInput() {

        if (charge >= DASH_MAX) {
            canDash = true;
        } else if (!isDashing) {
            canDash = false;
        }

        

        playerScript.isDashing = isDashing;

        if (Input.GetButtonDown("Ultimate") && !isCharging && charge >= DASH_COST && canDash) {
            startSpot = this.transform.position;
            target = cameraScript.raycastPoint;
            isCharging = true;
            onPause = false;
            networkPlayerManager.currentUltimateGain -= DASH_COST;
            if (!isDashing) {
                isDashing = true;
            }
        } else if (!isCharging && charge < DASH_COST) {
            isDashing = false;
            onPause = false;
        }

    }

    //Runs actual dash stuff and damage
    void Dashing() {
		if (isDashing) {
			playerAnimator.SetBool ("UltimateLoop", true);
			if (isCharging) {
				StartCoroutine (Dash (target));
				DashDamaging (dashDamage + 10f);
				playerAnimator.SetTrigger ("StartUltimate");
			} else {
				DashDamaging (dashDamage);
			}
		} else {
			playerAnimator.SetBool ("UltimateLoop", false);
		}
    }


    //The dash coroutine
    IEnumerator Dash(Vector3 thisTarget) {
		this.transform.position = Vector3.MoveTowards(this.transform.position, target, Time.deltaTime * DASH_SPEED);
        yield return new WaitUntil(() => ShouldStop());
        onPause = true;
        isCharging = false;
        yield return null;
    }

    //Exits dash if stays in air for too long between dashes
    IEnumerator Expire() {
        yield return new WaitForSeconds(2f);
        isDashing = false;
        yield return null;
    }

    //Should the player exit current dash? 
    bool ShouldStop() {
        if (isCharging) {
            bool distance = Vector3.Distance(this.transform.position, startSpot) > MAX_DISTANCE; //travelled certain disance from start
            bool reachedTarget = Vector3.Distance(this.transform.position, target) < TARGET_THRESH; //travelled close enough to target

            if (distance || reachedTarget) {
                return true;
            }
        }
        return false;
    }

    //causes damage to players that collide with this player
    [Client]
    public void DashDamaging(float damage) {
        hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

        foreach (Collider hitCol in hitColliders) {
            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG) {
                Debug.Log("Hit Player!");

                CmdTakeDamage(hitCol.gameObject.name, damage, transform.name);
            }
        }
    }

    //actual damage dealing command
    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    //command that modifies the ultimate bar value
    [Command]
    void CmdChargeUltimate(float _ultimatePoints, string _playerID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcUltimateCharging(_ultimatePoints, _playerID);
    }

    /*
    .       _,..wWWw--./+'.            _      ,.                          .															
      ..wwWWWWWWWWW;ooo;++++.        .ll'  ,.++;																                                                            					
       `'"">wW;oOOOOOO;:++\++.      .lll .l"+++'   ,..	                                   ,-.______________,=========,																																							
         ,wwOOOOOOOO,,,++++\+++.    lll',ll'++;  ,++;'                                    [|  )_____________)#######((_
        ,oOOOOOOOO,,,,+++++`'++ll. ;lll ll:+++' ;+++'									   /===============.-.___,--" _\									
       ;OOOOOOOOO,,,'++++++++++lll ;lll ll:++:'.+++'                                       "-._,__,__[JW]____\########/
       OOOO;OOO",,"/;++++,+,++++ll`:llllll++++'+++											         \ (  )) )####O##(									
      OOOO;OO",,'++'+++;###;"-++llX llll`;+++++++'  ,.    .,      _									  \ \___/,.#######\									
    ;O;'oOOO ,'+++\,-:  ###++++llX :l.;;;,--++."-+++++ w":---wWWWWWww-._                               `===="  \#######\
    ;'  /O'"'"++++++' :;";#'+++lllXX,llll;++.+++++++++W,"WWWWWWWWww;""""'`                                      \#######\
       ."     `"+++++'.'"''`;'ll;xXXwllll++;--.++++;wWW;xXXXXXXXXXx"Ww.								             )##O####|
               .+++++++++++';xXXXXX;Wll"+-"++,'---"-.x""`"lllllllxXXxWWw.                                        )####__,"
               "---'++++++-;XXXXXXwWWl"++++,"---++++",,,,,,,,,,;lllXXXxWW,                                       `--""
                 `'""""',+xXXXXX;wWW'+++++++++;;;";;;;;;;;oOo,,,,,llXXX;WW`                     LUL
                       ,+xXXXXXwWW"++.++++-.;;+++<'   `"WWWww;Oo,,,llXXX"Ww
                       +xXXX"wwW"+++++'"--'"'  )+++     `WWW"WwOO,,lllXXXww
                      ,x++++;"+++++++++++`., )  )+++     )W; ,WOO,,lllX:"Ww
                      :++++++++++++++++++++W'"-:++++    .W'  WWOO,,lllX; `w
                      .++++++++++++++++.+++"ww :+++'   ,"   ,WWOO,,lllX;  ;
               ;ll--.-"`.;++++++++++++++.+++;+.;++(         :WWOO,,lllXx
              ,'lllllllll,++++;+++++++++;"++++++++++++-.    :WWOO,,lllXx
              ;llll;;;"';'++++;'"""'''`` `lll;;:+++++++++.  WWOOO,,lllX'
             ,lllll,    ;+++++;            `"lllll.++++++++ WWwO,,,llX;
             lllllll,  ,++++++;               llllll+++++++.:WWw',,llx
            ,llllllll, ;++++++;               :llllll+++++++."WW;,,llx
            ;lllllllllV+++++++;               :lllllll+++++++.`w' `.lx.
            `lllllllll'+++++++;               :lllllll++++++++  `\  `,X\
             "llllll;++++++++;                ;llllll'+++++++++   `-  \X;
              "llll'+++++++++;               ;lllllll"+++++++++        `)
               `-'`+++++++++;'              ,llllllll++++++++++
                 +++++++++++;              ,llllllll'++++++++++
    .           '++++++++++"               `""""""""'+++++++++"           .
    */

}
