using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_PlayerStats : MonoBehaviour {

    public Text killCount;
    public Text deathCount;

	// Use this for initialization
	void Start () {

        if (UI_UserAccountManager.IsLoggedIn)
            UI_UserAccountManager.instance.GetData(OnReceivedData);	
	}

    void OnReceivedData (string data)
    {
        if (killCount == null || deathCount == null)
            return;

        killCount.text = Network_DataTranslator.DataToKills(data).ToString() + " KILLS";
        deathCount.text = Network_DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
    }
}
