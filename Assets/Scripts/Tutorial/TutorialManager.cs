using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	
	private string str;
	public Text textObject;
	public bool textNotDone = false;

	public GameObject indicator;
	public GameObject target1;
	public GameObject target2;
	public GameObject target3;

	public int botsMurdered = 0;


	public int tutProgression = 1;

	void Start() {
		indicator.SetActive (false);
		target1.SetActive (false);
		tutProgression = 1;
	}
		
	void Update() {
		textObject.text = str;	
		tutProgress ();

		if (tutProgression == 3) {
			if (Input.GetKey ("w") || Input.GetKey ("s") || Input.GetKey ("a") || Input.GetKey ("d")) {
				tutProgression = 4;
			}
		}

		if (tutProgression == 5) {
			if (Input.GetKey ("left shift")) {
				tutProgression = 6;
			}
		}

		if (tutProgression == 7) {
			if (Input.GetKey ("w") || Input.GetKey ("s") || Input.GetKey ("a") || Input.GetKey ("d") || Input.GetKey ("space")) {
				tutProgression = 8;
			}
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
		tutProgression += 1;
	}

	void tutProgress () {

		//Basic movement
		if (tutProgression == 1 && !textNotDone) {
			StartCoroutine (AnimateText ("Okay rustbucket, lets get those servos moving!"));
		}
		if (tutProgression == 2 && !textNotDone) {
			StartCoroutine (AnimateText ("To start, look around with the mouse, and move with W,A,S,D and space to jump"));
		}

		//Gravity shifting
		if (tutProgression == 4 && !textNotDone) {
			StartCoroutine (AnimateText ("Huh, looks like your not broken, ok, now to get tricky, hold Shift to bring up your gravity ring"));
		}
		if (tutProgression == 6 && !textNotDone) {
			StartCoroutine (AnimateText ("Okay, heres the tricky bit. Hold shift, and then either W,S,A,D or space will switch you to a new plane of gravity"));
		}

		//Reach the targets x 3
		if (tutProgression == 8 && !textNotDone) {
			indicator.SetActive (true);
			target1.SetActive (true);
			StartCoroutine (AnimateText ("Weird right? Grind those gears and see if you can reach the target that has appeared"));
		}

		if (tutProgression == 9 && !textNotDone) {
			indicator.GetComponent<DirectionIndicator>().DirectionIndicatorObject = target2;
			target2.SetActive (true);
			StartCoroutine (AnimateText ("I knew you wern't ready for the scrap heap yet, one more now"));
		}

		if (tutProgression == 10 && !textNotDone) {
			indicator.GetComponent<DirectionIndicator>().DirectionIndicatorObject = target3;
			target3.SetActive (true);
			StartCoroutine (AnimateText ("Okay, one more and you'll be ready to murder again"));
		}

		//Melee combat
		if (tutProgression == 11 && !textNotDone) {
			StartCoroutine (AnimateText ("Looks like your good to go four arms, lets see now..."));
		}
		if (tutProgression == 12 && !textNotDone) {
			StartCoroutine (AnimateText ("Ahh! Perfect timing, this chump is scheulded for the scrap heap. Run over there and left click to finish the job"));
		}

		if (tutProgression == 12 && !textNotDone && botsMurdered == 1) {
			StartCoroutine (AnimateText ("Yes! Yes! Do it again!"));
		}
	}
}
