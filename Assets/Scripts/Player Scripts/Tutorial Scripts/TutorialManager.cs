using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	public SP_CompactHud compactHud; // to turn off the effects

	private string str;
	public Text textObject;
	public bool textNotDone = false;
	public bool Scoreboardshown = false;
	public bool TextSaid = false; // is false text has not bee said, if true text has been said

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
			tutClash ();
			//overallProgression = 4;
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
		if (overallProgression == 7) 
		{
			tabtut ();
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
		if (spikeTrapTouched && tutProgression == 4 && overallProgression == 5) {
			tutProgression = 5;
		}
		if (healthPadTouched && tutProgression == 5 && overallProgression == 5) {
			tutProgression = 6;
		}
		if (ultPadTouched && tutProgression == 6 && overallProgression == 5) {
			tutProgression = 7;
		}
		if (slowTrapTouched && tutProgression == 7 && overallProgression == 5) {
			tutProgression = 8;
		}
	}

	//StartCoroutine( AnimateText(textToWrite));
	IEnumerator AnimateText(string strComplete){
		TextSaid = true;
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
			StartCoroutine (AnimateText ("Okay you old rustbucket, let’s get those circuits warmed up.  Look around and change your zoom with the mouse."));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("First of all let’s check your movement system still work. Use W,A,S,D to move around and spacebar to jump."));
			tutProgression = 3;
		}

		//Gravity shifting
		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("Well, looks like you aren’t broken. Let’s check that switch core out. Press Shift to bring up your gravity ring."));
			tutProgression = 5;
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("Holding shift, press W,A,S,D or spacebar to switch to a new plane of gravity."));
			tutProgression = 7;
			TextSaid = false;
		}

		//Reach the targets x 3
		if (tutProgression == 8 && !textNotDone && TextSaid == false) {
			indicator.SetActive (true);
			target1.SetActive (true);
			StartCoroutine (AnimateText ("Oh good, Gravita will be pleased. Grind those gears and see if you can reach the target that has appeared."));
		}

		if (tutProgression == 9 && !textNotDone && TextSaid == false) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = target2;
			target2.SetActive (true);
			StartCoroutine (AnimateText ("I knew you weren’t quite ready for the scrap heap yet. Try and reach the next target now."));
		}

		if (tutProgression == 10 && !textNotDone && TextSaid == false) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = target3;
			target3.SetActive (true);
			StartCoroutine (AnimateText ("Okay one more, and then we can test your combat systems."));
		}

		//Next tut progression
		if (tutProgression == 11 && !textNotDone) {
			StartCoroutine (AnimateText ("Looks like you're good to go four arms, let's see now..."));
			overallProgression = 2;
				tutProgression = 1;
			//tutProgression = 12;
			indicator.SetActive (false);
		}
//		if (tutProgression == 12 && !textNotDone) {
//			StartCoroutine (AnimateText ("Oh, just to let you know. You cant keep changing gravity forever"));
//			tutProgression = 13;
//		}
//		if (tutProgression == 13 && !textNotDone) {
//			StartCoroutine(flashingarrow(gravArrow));
//			StartCoroutine (AnimateText ("You have charges that regenerate overtime, thats them on your hud"));
//			tutProgression = 14;
//		}
//		if (tutProgression == 14 && !textNotDone) {
//			StartCoroutine(flashingarrow(gravArrow));
//			StartCoroutine (AnimateText ("When you are out of charges you cant change your gravity. You will be a sitting duck for you enemies"));
//			overallProgression = 2;
//			tutProgression = 1;
//		}
	}

	void tutMelee () {
		//Melee combat
		if (tutProgression == 1 && !textNotDone) {
			chump1.SetActive (true);
			indicator.SetActive (true);
			indicator.GetComponent<DirectionIndicator> ().targetObject = chump1;
			StartCoroutine (AnimateText ("Ahh! Perfect timing, this chump here is headed for the scrap heap."));
			tutProgression = 2;
		}

		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("Run over there and left click to finish the job."));
			tutProgression = 3;
		}
        /*
		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine (AnimateText ("So don't think you can go and hide. your enemies will find you"));
			tutProgression = 4;
		}

		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("Run over there and left click to finish the job"));
			tutProgression = 5;
		}*/

		if (tutProgression == 3 && !textNotDone && botsMurdered == 1) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = chump3;
			chump2.SetActive (true);
			chump3.SetActive (true);
			chump4.SetActive (true);
			StartCoroutine (AnimateText ("Yes! Yes! Do it again! Kill these 3 as well."));
			tutProgression = 4;
		}

        if (tutProgression == 4 && !textNotDone) {
            StartCoroutine(AnimateText("Of course, real opponents aren’t just going to stand and take it, you will have to hunt them down and avoid their attacks."));
            tutProgression = 5;
        }
        if (tutProgression == 5 && !textNotDone) {
            StartCoroutine(AnimateText("While your gravity wheel is active, you can see your enemies through walls. So don’t think you can go and hide. Your enemies WILL find you."));
            tutProgression = 6;
        }


        if (tutProgression == 6 && !textNotDone && botsMurdered == 4) {
			StartCoroutine (AnimateText ("You look like you are almost ready to entertain us again."));
			overallProgression = 3;
			tutProgression = 1;
			botsMurdered = 0;
			indicator.SetActive (false);
		}
	}

	void tutClash () {
		overallProgression = 4;
//		if (tutProgression == 1 && !textNotDone) {
//			StartCoroutine (AnimateText ("Of course, real opponents aren't just going to stand and take it, lets see,"));
//			tutProgression = 2;
//		}
//		if (tutProgression == 2 && !textNotDone) {
//			champ1.SetActive (true);
//			indicator.SetActive (true);
//			indicator.GetComponent<DirectionIndicator> ().targetObject = champ1;
//			StartCoroutine (AnimateText ("This guy seems to have a bit of fight left in him, try and take him down"));
//			tutProgression = 3;
//		}
//		if (tutProgression == 3 && !textNotDone) {
//			StartCoroutine (AnimateText ("Like before, run over and left click to attack"));
//			tutProgression = 4;
//		}
//		//didn't trigger the rest of statments in this function
//		if (botsMurdered == 0 && clashBounced && !textNotDone && tutProgression == 4) {
//			StartCoroutine (AnimateText ("Well that didn't work, looks like you were attacking at the same speed, and just bounced off each other"));
//			tutProgression = 5;
//		}
//		if (tutProgression == 5 && !textNotDone) {
//			StartCoroutine (AnimateText ("See the trails your weapons are leaving? They change colour the faster you go, green, yellow, then red"));
//			tutProgression = 6;
//		}
//		if (tutProgression == 6 && !textNotDone) {
//			StartCoroutine (AnimateText ("Running, that'll only ever be slow, but falling, thats where it's at. You will do more damage the higher speed you are moving at"));
//			tutProgression = 7;
//		}
//		if (tutProgression == 7 && !textNotDone) {
//			StartCoroutine (AnimateText ("So, using your gravity switching powers, fall at him and attack with speed!"));
//			tutProgression = 8;
//		}
//		if (botsMurdered == 1 && !textNotDone && tutProgression == 8) 
//		{
//			champ2.SetActive (true);
//			champ3.SetActive (true);
//			StartCoroutine (AnimateText ("Haha, yeah! Okay, two more! Remember, falling will do more damage!"));
//			tutProgression = 9;
//		}
//		if(botsMurdered == 3 && !textNotDone && tutProgression == 9)
//		{
//			StartCoroutine (AnimateText ("Nice job twinkle toes... Just remember, move faster than your opponent to hurt them good"));
//			botsMurdered = 0;
//			overallProgression = 4;
//			tutProgression = 1;
//			
//		}
//
//		// if they kill but still bounced and caanot go into next section
//		if (botsMurdered == 1 && clashBounced && !textNotDone && tutProgression == 4) {
//			StartCoroutine (AnimateText ("You're either better at this than you let or, you are very lucky"));
//			tutProgression = 8;
//		}
//
//		//if they kill before bouncing
//		if (botsMurdered == 1 && !textNotDone && tutProgression != 8 && !clashBounced) {
//			StartCoroutine (AnimateText ("You're either better at this than you let or, you are very lucky"));
//			tutProgression = 8;
//		}
	}

	void tutRanged () {
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("Just a few more tests before we get started. Let's try something with some distance."));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			camDrone1.SetActive (true);
			camDrone2.SetActive (true);
			camDrone3.SetActive (true);
			camDrone4.SetActive (true);
			camDrone5.SetActive (true);
			StartCoroutine (AnimateText ("See those little camera drones? They'd love to hear from you,"));
			tutProgression = 3;
		}
		//next two don't trigger
		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("go ahead and line them up in your sights and Right Click to show them who's boss!"));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone && botsMurdered >= 1) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("That'll teach those little sneaks, finish off the rest so my dressing room is safe again."));
			tutProgression = 5;
		}
		if (tutProgression == 5 && !textNotDone) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("Of course a proper fighter won't be so easy, but don't forget this will stun them,"));
			tutProgression = 6;
		}
		//doesn't trigger
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("giving you enough time to get up in their face and deliver the finishing blow."));
			tutProgression = 7;
		}

		if (tutProgression == 7 && !textNotDone && botsMurdered == 5) { // if they destory bots to quickly this will allow them to move on
			tutProgression = 8;
		}

		if (tutProgression == 7 && !textNotDone && botsMurdered != 5) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("There are still some drones left, go hunt them down."));
			tutProgression = 8;			
			TextSaid = false;
		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==1 && TextSaid == false) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("4 left"));

		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==2 && TextSaid == false) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("3 left"));

		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==3 && TextSaid == false) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("2 left"));

		}
		if (tutProgression == 8 && !textNotDone && botsMurdered ==4 && TextSaid == false) {
			StartCoroutine(flashingarrow(rangeArrow));
			StartCoroutine (AnimateText ("1 left, come on, quickly now!"));
		}
	
		if (tutProgression == 8 && !textNotDone && botsMurdered == 5) {
			StartCoroutine (AnimateText ("Ohh, I do love the smell of burning drones in the morning."));
			overallProgression = 5;
			tutProgression = 1;
			botsMurdered = 0;
		}
	}

	void tutTraps () {
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("I can't believe I'm saying this, but, that's enough killing for now."));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("There are still a few things I need to show you to properly test those memory banks so, let's hurry now."));
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
			StartCoroutine (AnimateText ("Hop on over to the pads that have just appeared, and touch them when I say so."));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("Here are some of the types of items in the arena; traps, heal pad, ultimate charge pad and movement modifiers."));
			tutProgression = 5;
			TextSaid = false;
		}
		if (tutProgression == 5 && !textNotDone && TextSaid == false) {
			StartCoroutine (AnimateText ("The grey pad is a spike trap. It will kill you in a real match but not in here, so go ahead and touch it."));
			//the big red one will hurt you, you can't die from it here, but in a real match it'll kill you dead, definitely touch this one though
		}
		if (tutProgression == 6 && !textNotDone && TextSaid == false) {
			StartCoroutine(flashingarrow(healthArrow));
			StartCoroutine (AnimateText ("The small green one will heal you, touch it now."));
		}
		if (tutProgression == 7 && !textNotDone && TextSaid == false) {
			StartCoroutine(flashingarrow(ultArrow));
			StartCoroutine (AnimateText ("The blue one will charge your ultimate attack quicker so you can do some serious damage faster."));
		}
		if (tutProgression == 8 && !textNotDone && TextSaid == false) {
			StartCoroutine (AnimateText ("The slime spill will slow you down, annoying, but not fatal, make sure you touch it too."));
		}
		if (tutProgression == 9 && !textNotDone && TextSaid == false) {
			StartCoroutine (AnimateText ("Watch out for these pads and traps. Some will help, some will harm, and the rest is up to you to find out."));
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
		overallProgression = 7;
	}

	void tabtut()
	{
		
		if (tutProgression == 1 && !textNotDone) {
			Scoreboardshown = false;
			StartCoroutine (AnimateText ("Before I finish with you, you can see other fighters in your game by holding tab."));
			tutProgression = 2;
		}

		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("Hold tab now to bring up the scoreboard."));
			tutProgression = 3;
		}

		if(tutProgression == 3 && !textNotDone && Scoreboardshown == true)
		{
			StartCoroutine (AnimateText ("This scoreboard will give you up to date info on the current state of the match."));
			tutProgression = 4;
		} 

		if(tutProgression == 4 && !textNotDone)
		{
			StartCoroutine (AnimateText ("Since there's no match running it wont show anything right now, but be sure to check it in a real game."));
			tutProgression = 1;
			overallProgression = 0;
		} 
	}



	public void tutFinish() 
	{
		indicator.SetActive (false);	
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("Well, that's all we have time for today! I'll leave some guys in here for you so."));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("you can run around here all you like, wrecking up the place, or press Escape and play a real game."));
			tutProgression = 3;
		}
		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine (AnimateText ("See you next time, Graviator!"));
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
			compactHud.onSlowTrap = false;// prevent the player being slowed when the traps respawn
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
