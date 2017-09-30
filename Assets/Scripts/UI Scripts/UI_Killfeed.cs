using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Killfeed : MonoBehaviour {

    [SerializeField]
    GameObject killfeedItemPrefab;

	// Use this for initialization
	void Start () {

        Network_GameManager.instance.onPlayerKilledCallback += onKill;		
	}

    public void onKill (string player, string source)
    {
        if (this.gameObject.activeSelf)
        {
            GameObject go = (GameObject)Instantiate(killfeedItemPrefab, this.transform);
            go.GetComponent<UI_KillfeedItem>().Setup(player, source);

            Destroy(go, 4f);
        }
    }	
}
