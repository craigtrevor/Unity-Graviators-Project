using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouseHover: IPointerEnterHandler {
	public GameObject characterBio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		characterBio.SetActive (true);
	}
}
