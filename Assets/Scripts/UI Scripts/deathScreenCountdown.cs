using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathScreenCountdown : MonoBehaviour {

	float timeLeft = 5.00f;

	public Text countdownText;

	void Update()
	{
		timeLeft -= Time.deltaTime;
		countdownText.text = "" + Mathf.Round(timeLeft);
		if(timeLeft < 0)
		{
			countdownText.text = "Respawning";
            timeLeft = 5.00f;
		}
	}
}
