using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScreen : MonoBehaviour {

	Network_Manager netManager;
	public int charSelection;

	public Animator noNameAnim;
	public Animator sparkusAnim;
	public Animator d1Anim;

	public Text noNameInfo;
	public Text sparkusInfo;
	public Text d1Info;

	public Text noNameUltInfo;
	public Text sparkusUltInfo;
	public Text d1UltInfo;

	public Vector3 rotateTo;

	public Color tempColor;
	public Color noNameColor;
	public Color sparkusColor;
	public Color d1Color;

	// Use this for initialization
	void Start () {
		tempColor = Color.white;
		tempColor.a = 0;
		noNameInfo.color = tempColor;
		sparkusInfo.color = tempColor;
		d1Info.color = tempColor;

		noNameUltInfo.color = tempColor;
		sparkusUltInfo.color = tempColor;
		d1UltInfo.color = tempColor;

		charSelection = 1;
		netManager = GameObject.FindGameObjectWithTag ("NetManager").GetComponent<Network_Manager> ();

		UpdateCharacter ();
	}

	void Update() {
		if (this.transform.eulerAngles != rotateTo) {
			this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotateTo), 250f * (Time.deltaTime));	
		}
	}

	// Update is called once per frame
	void UpdateCharacter () {
		if (charSelection == 1) { 
			netManager.CharacterSelector ("ErrNoName_btn");
			noNameAnim.SetTrigger ("Flourish");

			rotateTo = new Vector3 (0, -90, 0);
			StartCoroutine(FadeIn (noNameInfo, 0.5f, noNameColor));
			StartCoroutine(FadeOut (sparkusInfo, 0.5f, sparkusColor));
			StartCoroutine(FadeOut (d1Info, 0.5f, d1Color));

			StartCoroutine(FadeIn (noNameUltInfo, 0.5f, noNameColor));
			StartCoroutine(FadeOut (sparkusUltInfo, 0.5f, sparkusColor));
			StartCoroutine(FadeOut (d1UltInfo, 0.5f, d1Color));
		}
		if (charSelection == 2) { 
			netManager.CharacterSelector ("Sparkus_btn");
			sparkusAnim.SetTrigger ("Flourish");
			rotateTo = new Vector3 (0, 27, 0);
			StartCoroutine(FadeOut (noNameInfo, 0.5f, noNameColor));
			StartCoroutine(FadeIn (sparkusInfo, 0.5f, sparkusColor));
			StartCoroutine(FadeOut (d1Info, 0.5f, d1Color));

			StartCoroutine(FadeOut (noNameUltInfo, 0.5f, noNameColor));
			StartCoroutine(FadeIn (sparkusUltInfo, 0.5f, sparkusColor));
			StartCoroutine(FadeOut (d1UltInfo, 0.5f, d1Color));
		}
		if (charSelection == 3) { 
			netManager.CharacterSelector ("UnitD1_btn");
			d1Anim.SetTrigger ("Flourish");
			rotateTo = new Vector3 (0, 150, 0);
			StartCoroutine(FadeOut (noNameInfo, 0.5f, noNameColor));
			StartCoroutine(FadeOut (sparkusInfo, 0.5f, sparkusColor));
			StartCoroutine(FadeIn (d1Info, 0.5f, d1Color));

			StartCoroutine(FadeOut (noNameUltInfo, 0.5f, noNameColor));
			StartCoroutine(FadeOut (sparkusUltInfo, 0.5f, sparkusColor));
			StartCoroutine(FadeIn (d1UltInfo, 0.5f, d1Color));
		}
	}

	public void ChangeCharUp() {
		if (charSelection == 3) {
			charSelection = 1;
		} else {
			charSelection++;
		}
		UpdateCharacter ();
	}

	public void ChangeCharDown() {
		if (charSelection == 1) {
			charSelection = 3;
		} else {
			charSelection--;
		}
		UpdateCharacter ();
	}

	IEnumerator FadeIn(Text text, float time, Color color) {
		for (int i = 0; i < 10; i++) {
			color = text.color;
			color.a += 0.1f;
			text.color = color;
			yield return new WaitForSeconds (time / 10f);
		}
		color.a = 1;
		text.color = color;
	}

	IEnumerator FadeOut(Text text, float time, Color color) {
		for (int i = 0; i < 10; i++) {
			color = text.color;
			color.a -= 0.1f;
			text.color = color;
			yield return new WaitForSeconds (time / 10f);
		}
		color.a = 0;
		text.color = color;
	}
}