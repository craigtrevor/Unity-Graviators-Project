using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MatchMixerLevels : MonoBehaviour {

    UI_MixLevels uiMixLevels;
    UI_PostProcessing postProccessingScript;

    public float[] mixerVolume;
    public bool bloomActivated = true;

    // Use this for initialization
    void Start ()
    {
        if (Network_SceneManager.instance.sceneName == "Game_Settings")
        {
            bloomActivated = true;
            uiMixLevels = GetComponent<UI_MixLevels>();
            postProccessingScript = GetComponent<UI_PostProcessing>();
        }
    }

    void UpdateMixValues(float[] sliderValues)
    {
        mixerVolume[0] = sliderValues[0];
        mixerVolume[1] = sliderValues[1];
        mixerVolume[2] = sliderValues[2];
        mixerVolume[3] = sliderValues[3];
        mixerVolume[4] = sliderValues[4];
        mixerVolume[5] = sliderValues[5];
    }

    void UpdatePPValue(bool bloomValue)
    {
        Debug.Log(bloomValue);
        bloomActivated = bloomValue;
    }
}
