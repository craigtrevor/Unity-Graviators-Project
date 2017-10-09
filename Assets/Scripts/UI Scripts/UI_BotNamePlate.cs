using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BotNamePlate : MonoBehaviour {

	[SerializeField]
	private RectTransform healthBarFill;

	[SerializeField]
	private RectTransform healthBarFill2;

	[SerializeField]
	private Network_Bot player;

	// Update is called once per frame
	void Update () {
		healthBarFill.sizeDelta = new Vector2 ((player.health * 6),200);
		healthBarFill2.sizeDelta = new Vector2 ((player.health * 6),200);

	}
}