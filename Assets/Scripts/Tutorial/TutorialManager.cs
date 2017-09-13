using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	public SP_CompactHud compactHud; // to turn off the effects

	private string str;
	public Text textObject;
	public bool textNotDone = false;

	public GameObject chatBox;

	public GameObject indicator;
	public GameObject target1;
	public GameObject target2;
	public GameObject target3;

	public GameObject chump1;
	public GameObject chump2;
	public GameObject chump3;
	public GameObject chump4;

	public GameObject champ1;
	public GameObject champ2;
	public GameObject champ3;

	public GameObject camDrone1;
	public GameObject camDrone2;
	public GameObject camDrone3;
	public GameObject camDrone4;
	public GameObject camDrone5;

	public GameObject slowTrap;
	public GameObject spikeTrap;
	public GameObject healthPad;
	public GameObject ultPad;



	public Image gravArrow;
	public Image healthArrow;
	public Image ultArrow;
	public Image rangeArrow;
	private float flashtime = 0.25f;

	public bool slowTrapTouched;
	public bool spikeTrapTouched;
	public bool healthPadTouched;
	public bool ultPadTouched;


	public bool clashBounced;

	public int botsMurdered = 0;

	public int tutProgression = 1;
	public int overallProgression = 1;

	void Start() {
		indicator.SetActive (false);
		//tutProgression = 1;
		//overallProgression = 1;
	}

	// overallProgression
	// 0 finished
	// 1 Movement - done
	// 2 Melee attacks - done
	// 3 clashes
	// 4 Ranged attacks - done
	// 5 Traps
	// 6 ultimates
	void Update() {
		//update the chatbox
		textObject.text = str;

		// overall progression triggers
		if (overallProgression == 0) {
			tutFinish ();
		}

		if (overallProgression == 1) {
			tutMovement ();
		}

		if (overallProgression == 2) {
			tutMelee ();
		}

		if (overallProgression == 3) {
			//tutClash ();
			overallProgression = 4;
		}

		if (overallProgression == 4) {
			tutRanged ();
		}

		if (overallProgression == 5) {
			tutTraps ();
		}

		if (overallProgression == 6) {
			tutUlt ();
		}

		//movement progression triggers
		if (overallProgression == 1 && tutProgression == 3) {
			if (Input.GetKey ("w") || Input.GetKey ("s") || Input.GetKey ("a") || Input.GetKey ("d")) {
				tutProgression = 4;
			}
		}

		if (overallProgression == 1 && tutProgression == 5) {
			if (Input.GetKey ("left shift")) {
				tutProgression = 6;
			}
		}

		if (overallProgression == 1 && tutProgression == 7) {
			if (Input.GetKey ("left shift")) {
				if (Input.GetKey ("w") || Input.GetKey ("s") || Input.GetKey ("a") || Input.GetKey ("d") || Input.GetKey ("space")) {
					tutProgression = 8;
				}
			}
		}

		//Clash check
		if (clashBounced && tutProgression == 2 && overallProgression == 3) {
			tutProgression = 3;
		}

		//trap progression triggers
		if (healthPadTouched && tutProgression == 4 && overallProgression == 5) {
			tutProgression = 5;
		}
		if (ultPadTouched && tutProgression == 5 && overallProgression == 5) {
			tutProgression = 6;
		}
		if (spikeTrapTouched && tutProgression == 6 && overallProgression == 5) {
			tutProgression = 7;
		}
		if (slowTrapTouched && tutProgression == 7 && overallProgression == 5) {
			tutProgression = 8;
		}
	}

	//StartCoroutine( AnimateText(textToWrite));
	IEnumerator AnimateText(string strComplete){
		textNotDone = true;
		int i = 0;
		str = "";
		while( i < strComplete.Length ){
			str += strComplete[i++];
			yield return new WaitForSeconds(0.03F);
		}
		yield return new WaitForSeconds(3F);
		textNotDone = false;
	}

	void tutMovement () {

		//Basic movement
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("Okay rustbucket, lets get those circuits moving!"));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("To start, look around with the mouse, and move with W,A,S,D and space to jump"));
			tutProgression = 3;
		}

		//Gravity shifting
		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("Huh, looks like you're not broken, ok, now to get tricky, hold Shift to bring up your gravity ring"));
			tutProgression = 5;
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("Hold shift, and then either W,S,A,D or space will switch you to a new plane of gravity"));
			tutProgression = 7;
		}

		//Reach the targets x 3
		if (tutProgression == 8 && !textNotDone) {
			indicator.SetActive (true);
			target1.SetActive (true);
			StartCoroutine (AnimateText ("Weird right? Grind those gears and see if you can reach the target that has appeared"));
		}

		if (tutProgression == 9 && !textNotDone) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = target2;
			target2.SetActive (true);
			StartCoroutine (AnimateText ("I knew you weren't ready for the scrap heap yet, one more now"));
		}

		if (tutProgression == 10 && !textNotDone) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = target3;
			target3.SetActive (true);
			StartCoroutine (AnimateText ("Okay, one more and you'll be ready to fight again"));
		}

		//Next tut progression
		if (tutProgression == 11 && !textNotDone) {
			StartCoroutine (AnimateText ("Looks like you're good to go four arms, lets see now..."));
			tutProgression = 12;
			//overallProgression = 2;
			//tutProgression = 1;
			indicator.SetActive (false);
		}
		if (tutProgression == 12 && !textNotDone) {
			StartCoroutine (AnimateText ("Oh, one more thing. You cant keep changing gravity forever"));
			tutProgression = 13;
		}
		if (tutProgression == 13 && !textNotDone) {
			StartCoroutine(flashingarrow(gravArrow));
			StartCoroutine (AnimateText ("You have charges that regenerate overtime, thats them on your hud"));
			tutProgression = 14;
		}
		if (tutProgression == 14 && !textNotDone) {
			StartCoroutine(flashingarrow(gravArrow));
			StartCoroutine (AnimateText ("When you are out of charges you cant change your gravity. You will be a sitting duck for you enemies"));
			overallProgression = 2;
			tutProgression = 1;
		}
	}

	void tutMelee () {
		//Melee combat
		if (tutProgression == 1 && !textNotDone) {
			chump1.SetActive (true);
			indicator.SetActive (true);
			indicator.GetComponent<DirectionIndicator> ().targetObject = chump1;
			StartCoroutine (AnimateText ("Ahh! Perfect timing, this chump is headed for the scrap heap."));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("Run over there and left click to finish the job"));
			tutProgression = 3;
		}

		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine (AnimateText ("While the gravity wheel is active, you can see your enemies through walls"));
			tutProgression = 4;
		}

		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("So don't think you can go and hide. your enemies will find you"));
			tutProgression = 5;
		}

		if (tutProgression == 5 && !textNotDone && botsMurdered == 1) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = chump3;
			chump2.SetActive (true);
			chump3.SetActive (true);
			chump4.SetActive (true);
			StartCoroutine (AnimateText ("Yes! Yes! Do it again! Kill these 3 as well"));
			tutProgression = 6;
		}
		//didn't trigger
		if (tutProgression == 6 && !textNotDone && botsMurdered == 4) {
			StartCoroutine (AnimateText ("Ah...Relaxing"));
			overallProgression = 3;
			tutProgression = 1;
			botsMurdered = 0;
			indicator.SetActive (false);
		}
	}

	void tutClash () {
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("Of course, real opponents aren't just going to stand and take it, lets see,"));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			champ1.SetActive (true);
			indicator.SetActive (true);
			indicator.GetComponent<DirectionIndicator> ().targetObject = champ1;
			StartCoroutine (AnimateText ("This guy seems to have a bit of fight left in him, try and take him down"));
		}
		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine (AnimateText ("Like before, run over and left click to attack"));
		}
		//didn't trigger the rest of statments in this function
		if (botsMurdered == 0 && clashBounced && !textNotDone) {
			StartCoroutine (AnimateText ("Well that didn't work, looks like you were attacking at the same speed, and just bounced off each other"));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("See the trails your weapons are leaving? They change colour the faster you go, green, yellow, then red"));
			tutProgression = 5;
		}
		if (tutProgression == 5 && !textNotDone) {
			StartCoroutine (AnimateText ("Running, that'll only ever be slow, but falling, thats where it's at. You will do more damage the higher speed you are moving at"));
			tutProgression = 6;
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("So, using your gravity switching powers, fall at him and attack with speed!"));
		}


		//if they kill before bouncing
		if (botsMurdered == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("You're either better at this than you let or, you are very lucky"));
			tutProgression = 7;
		}

		if (botsMurdered == 1 && clashBounced && !textNotDone) {
			champ2.SetActive (true);
			champ3.SetActive (true);
			StartCoroutine (AnimateText ("Haha, yeah! Okay, two more! Remember, falling will do more damage!"));
			tutProgression = 7;
		}

		if (botsMurdered == 7 && tutProgression == 7 && !textNotDone ) {
			StartCoroutine (AnimateText ("Nice job twinkle toes... Just remember, move faster than your opponent to hurt them good"));
			overallProgression = 4;
			tutProgression = 1;
		}
	}

	void tutRanged () {
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("I think that's enough socializing for now. Lets try for something with some distance"));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			camDrone1.SetActive (true);
			camDrone2.SetActive (true);
			camDrone3.SetActive (true);
			camDrone4.SetActive (true);
			camDrone5.SetActive (true);
			StartCoroutine (AnimateText ("See these little camera drones? They'd love to hear from you,"));
			tutProgression = 3;
		}
		//next two don't trigger
		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("line 'em up in your sights and Right Click to deliver your message!"));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone && botsMurdered >= 1) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("That'll teach those little sneaks, finish off the rest so my dressing room is safe again"));
			tutProgression = 5;
		}
		if (tutProgression == 5 && !textNotDone) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("So, this will only stun a proper fighter, but it should delay them long enough for you"));
			tutProgression = 6;
		}
		//doesn't trigger
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("to get up in their face and deliver the finishing blow"));
			tutProgression = 7;
		}

		if (tutProgression == 7 && !textNotDone) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("There are still some drones left, go hunt them down"));
			tutProgression = 8;
		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==1) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("4 left"));
			//tutProgression = 9;
		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==2) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("3 left"));
			//tutProgression = 9;
		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==3) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("2 left"));
			//tutProgression = 9;
		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==4) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("1 left"));
			tutProgression = 9;
		}
	
		if (tutProgression == 9 && !textNotDone && botsMurdered == 5) {
			StartCoroutine (AnimateText ("I love the smell of burning drones in the morning"));
			overallProgression = 5;
			tutProgression = 1;
			botsMurdered = 0;
		}
	}

	void tutTraps () {
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("I can't belive I'm saying this, but, thats enough killing for now"));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("There are a few things I need to show you, so make sure you don't die"));
			tutProgression = 3;
		}
		if (tutProgression == 3 && !textNotDone) {
			//turn on pads and indicate to them
			indicator.SetActive (true);
			indicator.GetComponent<DirectionIndicator> ().targetObject = healthPad;
			healthPad.SetActive (true);
			ultPad.SetActive (true);
			spikeTrap.SetActive (true);
			slowTrap.SetActive (true);
			StartCoroutine (AnimateText ("Waddle on over to the pads that have just appeared, and touch them when I say so"));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("There are 4 types of pads, the small green one will heal you, touch it now."));
		}
		if (tutProgression == 5 && !textNotDone) {
			StartCoroutine (AnimateText ("The blue one will charge your ultimate attack quicker so you can do some serious damage"));
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("the big red one will hurt you, you can't die from it here, but in a real match it'll kill you dead, definitely touch this one though!"));
		}
		if (tutProgression == 7 && !textNotDone) {
			StartCoroutine (AnimateText ("the big green one will slow you down, annoying, but not fatal, so touch it too"));
		}
		if (tutProgression == 8 && !textNotDone) {
			StartCoroutine (AnimateText ("So watch out for these pad, some will help, some will harm, but that is up to you to find out"));
			StartCoroutine (WaitForTraps ());
		}

	}

	public IEnumerator WaitForTraps() {
		yield return new WaitForSeconds (5f);
		overallProgression = 6;
		tutProgression = 1;
		healthPad.SetActive (false);
		ultPad.SetActive (false);
		spikeTrap.SetActive (false);
		slowTrap.SetActive (false);
		indicator.SetActive (false);
		compactHud.onHealthPad = false;
		compactHud.onUltPad = false;
		compactHud.onSpikeTrap = false;
		compactHud.onSlowTrap = false;
	}

	void tutUlt () {
		overallProgression = 0;
	}



	public void tutFinish() {
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("Well, thats all we have time for today! I'll leave some guys in here for you so"));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("you can run around here all you like, wrecking up the place, or you press Escape and play a real game"));
			tutProgression = 3;
		}
		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine (AnimateText ("See you next time, graviator!"));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone) {
			//turn everything on for muckabout
			chatBox.SetActive (false);
			chump1.SetActive (true);
			chump1.GetComponent<Bot_Script> ().respawnEnabled = true;
			chump2.SetActive (true);
			chump2.GetComponent<Bot_Script> ().respawnEnabled = true;
			chump3.SetActive (true);
			chump3.GetComponent<Bot_Script> ().respawnEnabled = true;
			chump4.SetActive (true);
			chump4.GetComponent<Bot_Script> ().respawnEnabled = true;
			camDrone1.SetActive (true);
			camDrone1.GetComponent<Drone_bot> ().respawn = true;
			camDrone2.SetActive (true);
			camDrone2.GetComponent<Drone_bot> ().respawn = true;
			camDrone3.SetActive (true);
			camDrone3.GetComponent<Drone_bot> ().respawn = true;
			camDrone4.SetActive (true);
			camDrone4.GetComponent<Drone_bot> ().respawn = true;
			camDrone5.SetActive (true);
			camDrone5.GetComponent<Drone_bot> ().respawn = true;
			healthPad.SetActive (true);
			ultPad.SetActive (true);
			spikeTrap.SetActive (true);
			slowTrap.SetActive (true);
			tutProgression = 5;
		}
	}

	IEnumerator flashingarrow(Image arrowtoFlash)
	{
		for (int i= 0; i<=10;i++)
		{
		arrowtoFlash.enabled = true;
		yield return new WaitForSeconds (flashtime);
		arrowtoFlash.enabled = false;
		yield return new WaitForSeconds (flashtime);
		}
	}
}
