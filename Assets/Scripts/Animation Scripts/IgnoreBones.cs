using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreBones : MonoBehaviour {

	public Transform playerRoot;
	public List<Animation> IgnoreArms;
	public List<Transform> Arms;
	public List<Transform> Legs;

	private void AddChildren(Transform parent, string tag, List<Transform> list)
	{
		foreach (Transform child in parent)
		{
			if (child.gameObject.name.Contains(tag))
			{
				list.Add(child);
			}
			AddChildren(child, tag, list);
		}
	}

	// Use this for initialization
	void Start () {
		AddChildren (playerRoot, "arm", Arms);
		AddChildren (playerRoot, "shoulder", Arms);
		AddChildren (playerRoot, "thigh", Legs);
		AddChildren (playerRoot, "shin", Legs);
		AddChildren (playerRoot, "foot", Legs);
		AddChildren (playerRoot, "toe", Legs);
	}
}
