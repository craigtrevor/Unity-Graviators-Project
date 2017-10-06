using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_MixLevels : MonoBehaviour
{
    [SerializeField]
    AudioMixerGroup[] audioMixers;

    [SerializeField]
    Slider[] audioSliders;

    public float[] sliderValues;

    UI_MatchMixerLevels matchMixerLevels;

    float volumeLvl;

    private void Start()
    {
        matchMixerLevels = GameObject.Find("Scene Checker").GetComponent<UI_MatchMixerLevels>();
        UpdateAudioMixers();
    }

    public void SetMasterLevel(float masterLvl)
    {
        audioMixers[0].audioMixer.SetFloat("MasterVolume", masterLvl);
        sliderValues[0] = masterLvl;
    }

    public void SetMusicLevel(float musicLvl)
    {
        audioMixers[1].audioMixer.SetFloat("MusicVolume", musicLvl);
        sliderValues[1] = musicLvl;

    }

    public void SetNarrationLevel(float narrationLvl)
    {
        audioMixers[2].audioMixer.SetFloat("NarrationVolume", narrationLvl);
        sliderValues[2] = narrationLvl;

    }

    public void SetCombatLevel(float combatLvl)
    {
        audioMixers[3].audioMixer.SetFloat("CombatVolume", combatLvl);
        sliderValues[3] = combatLvl;

    }

    public void SetAtmosLevel(float atmosLvl)
    {
        audioMixers[4].audioMixer.SetFloat("AtmosVolume", atmosLvl);
        sliderValues[4] = atmosLvl;

    }

    public void SetUILevel(float uiLvl)
    {
        audioMixers[5].audioMixer.SetFloat("UIVolume", uiLvl);
        sliderValues[5] = uiLvl;
    }

    public void SendMessage()
    {
        matchMixerLevels.SendMessage("UpdateValues", sliderValues);
    }

    void UpdateAudioMixers()
    {
        if (matchMixerLevels.mixerVolume[0] != 0)
        {
            volumeLvl = matchMixerLevels.mixerVolume[0];
            audioSliders[0].value = volumeLvl;
        }

        if (matchMixerLevels.mixerVolume[1] != 0)
        {
            volumeLvl = matchMixerLevels.mixerVolume[1];
            audioSliders[1].value = volumeLvl;
        }

        if (matchMixerLevels.mixerVolume[2] != 0)
        {
            volumeLvl = matchMixerLevels.mixerVolume[2];
            audioSliders[2].value = volumeLvl;
        }

        if (matchMixerLevels.mixerVolume[3] != 0)
        {
            volumeLvl = matchMixerLevels.mixerVolume[3];
            audioSliders[3].value = volumeLvl;
        }

        if (matchMixerLevels.mixerVolume[4] != 0)
        {
            volumeLvl = matchMixerLevels.mixerVolume[4];
            audioSliders[4].value = volumeLvl;
        }

        if (matchMixerLevels.mixerVolume[5] != 0)
        {
            volumeLvl = matchMixerLevels.mixerVolume[5];
            audioSliders[5].value = volumeLvl;
        }
    }
}