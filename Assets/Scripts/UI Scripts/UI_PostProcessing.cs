using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class UI_PostProcessing : MonoBehaviour {

	public PostProcessingProfile ppProfile;
    bool bloomValue;
    UI_MatchMixerLevels matchMixerLevels;

    [SerializeField]
    Toggle bloomToggle;


    private void Start()
    {
        matchMixerLevels = GameObject.Find("Scene Checker").GetComponent<UI_MatchMixerLevels>();
        bloomToggle.GetComponent<Toggle>();
        bloomValue = matchMixerLevels.bloomActivated;
        bloomToggle.isOn = matchMixerLevels.bloomActivated;
    }

    public void ToggleChanged(bool isBloom)
	{
		if (isBloom)
		{
            //copy current bloom settings from the profile into a temporary variable
            BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
			//change the intensity in the temporary settings variable
			bloomSettings.bloom.intensity = 0.1f;
			//set the bloom settings in the actual profile to the temp settings with the changed value
			ppProfile.bloom.settings = bloomSettings;

			Debug.Log ("pp on");
            bloomValue = true;

        }

		else if (!isBloom)
		{
			//copy current bloom settings from the profile into a temporary variable
			BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
			//change the intensity in the temporary settings variable
			bloomSettings.bloom.intensity = 0f;
			//set the bloom settings in the actual profile to the temp settings with the changed value
			ppProfile.bloom.settings = bloomSettings;

			Debug.Log("pp off");
            bloomValue = false;
        }
	}

    public void SendMessage()
    {
        matchMixerLevels.SendMessage("UpdatePPValue", bloomValue);
    }
}
