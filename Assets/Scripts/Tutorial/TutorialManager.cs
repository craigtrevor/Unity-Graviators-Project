using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	
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

	public GameObject camDrone1;
	public GameObject camDrone2;
	public GameObject camDrone3;
	public GameObject camDrone4;
	public GameObject camDrone5;

	public GameObject slowTrap;
	public GameObject spikeTrap;
	public GameObject healthPad;
	public GameObject ultPad;

	public bool slowTrapTouched;
	public bool spikeTrapTouched;
	public bool healthPadTouched;
	public bool ultPadTouched;

	public int botsMurdered = 0;

	public int tutProgression = 1;
	public int overallProgression = 1;

	void Start() {
		indicator.SetActive (false);
		tutProgression = 1;
		overallProgression = 2;
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
			StartCoroutine (AnimateText ("Okay rustbucket, lets get those servos moving!"));
			tutProgression = 2;
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("To start, look around with the mouse, and move with W,A,S,D and space to jump"));
			tutProgression = 3;
		}

		//Gravity shifting
		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("Huh, looks like your not broken, ok, now to get tricky, hold Shift to bring up your gravity ring"));
			tutProgression = 5;
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("Okay, heres the tricky bit. Hold shift, and then either W,S,A,D or space will switch you to a new plane of gravity"));
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
			StartCoroutine (AnimateText ("I knew you wern't ready for the scrap heap yet, one more now"));
		}

		if (tutProgression == 10 && !textNotDone) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = target3;
			target3.SetActive (true);
			StartCoroutine (AnimateText ("Okay, one more and you'll be ready to murder again"));
		}

		//Next tut progression
		if (tutProgression == 11 && !textNotDone) {
			StartCoroutine (AnimateText ("Looks like your good to go four arms, lets see now..."));
			overallProgression = 2;
			tutProgression = 1;
			indicator.SetActive (false);
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

		if (tutProgression == 3 && !textNotDone && botsMurdered == 1) {
			indicator.GetComponent<DirectionIndicator> ().targetObject = chump3;
			chump2.SetActive (true);
			chump3.SetActive (true);
			chump4.SetActive (true);
			StartCoroutine (AnimateText ("Yes! Yes! Do it again!"));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone && botsMurdered == 4) {
			StartCoroutine (AnimateText ("Ah...Relaxing"));
			overallProgression = 3;
			tutProgression = 1;
			botsMurdered = 0;
			indicator.SetActive (false);
		}
	}

	void tutClash () {
		overallProgression = 4;
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
		if (tutProgression == 3 && !textNotDone) {
			StartCoroutine (AnimateText ("line 'em up in your sights and Right Click to deliver your message!"));
			tutProgression = 4;
		}
		if (tutProgression == 4 && !textNotDone && botsMurdered >= 1) {
			StartCoroutine (AnimateText ("That'll teach those little sneaks, finish off the rest so my dressing room is safe again"));
			tutProgression = 5;
		}
		if (tutProgression == 5 && !textNotDone) {
			StartCoroutine (AnimateText ("So, this will only stun a proper fighter, but it should delay them long enough for you"));
			tutProgression = 6;
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("to get up in their face and deliver the finishing blow"));
			tutProgression = 7;
		}
	
		if (tutProgression == 7 && !textNotDone && botsMurdered == 5) {
			StartCoroutine (AnimateText ("I love the smell of buring drones in the morning"));
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
			StartCoroutine (AnimateText ("There are a few I need to show you, so you dont kill yourself"));
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
			StartCoroutine (AnimateText ("There are 4 types of pads, the small green one will heal you, touch it now"));
		}
		if (tutProgression == 5 && !textNotDone) {
			StartCoroutine (AnimateText ("The blue one will charge your ultimate attack, which is doesnt work at the moment, but all the same, tooouch it!"));
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("the big red one will hurt you, you can't die from it here, but in a real match it'll kill you dead, definately touch this one!"));
		}
		if (tutProgression == 7 && !textNotDone) {
			StartCoroutine (AnimateText ("the big green one will slow you down, annoying, but not fatal, so touch it too"));
		}
		if (tutProgression == 8 && !textNotDone) {
			StartCoroutine (AnimateText ("So watch out for these, some will help, some will harm"));
			overallProgression = 6;
			tutProgression = 0;
			healthPad.SetActive (false);
			ultPad.SetActive (false);
			spikeTrap.SetActive (false);
			slowTrap.SetActive (false);
			indicator.SetActive (false);
		}

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
			StartCoroutine (AnimateText ("you can run around here all you like, wrecking up the place, or you can go and play a real game"));
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
}
