using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	public GameObject nullParticle;
	public GameObject particleObject;

	[System.Serializable]
	public struct particleListSetup {
		public string name;
		public GameObject pObject;
	}

	[SerializeField]
	public particleListSetup[] particleSetup;

	public Dictionary<string, GameObject> particleList = new Dictionary<string, GameObject> ();

	void Start () {
		for (int i = 0; i < particleSetup.Length; i++) {
			particleList.Add (particleSetup[i].name, particleSetup[i].pObject);
		}
	}

	public GameObject GetParticle (string particleString) {
		if (!particleList.TryGetValue (particleString, out particleObject)) {
			return nullParticle;
		} else {
			particleList.TryGetValue (particleString, out particleObject);
			return particleObject;
		}
	}
}
