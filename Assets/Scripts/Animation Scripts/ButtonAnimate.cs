using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimate : MonoBehaviour {

	public Image thisImage;

	public Sprite normalButton;
	public Sprite hoverButton;
	public Sprite clickButton;

	public void OnPointerClick () {
		thisImage.sprite = clickButton;
	}

	public void OnPointerEnter () {
		thisImage.sprite = hoverButton;
	}

	public void OnPointerExit () {
		thisImage.sprite = normalButton;
	}
}
