using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class UI_PostProcessing : MonoBehaviour {

	public PostProcessingProfile ppProfile;

	bool bloom;

	void Start() {
		
	}

	public void ToggleChanged(bool bloom)
	{
		if (!bloom)
		{
			//copy current bloom settings from the profile into a temporary variable
			BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
			//change the intensity in the temporary settings variable
			bloomSettings.bloom.intensity = 0.1f;
			//set the bloom settings in the actual profile to the temp settings with the changed value
			ppProfile.bloom.settings = bloomSettings;

			bloom = true;
			Debug.Log ("pp on");
		}

		else
		{
			//copy current bloom settings from the profile into a temporary variable
			BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
			//change the intensity in the temporary settings variable
			bloomSettings.bloom.intensity = 0f;
			//set the bloom settings in the actual profile to the temp settings with the changed value
			ppProfile.bloom.settings = bloomSettings;

			bloom = false;
			Debug.Log("pp off");
		}
	}
}
