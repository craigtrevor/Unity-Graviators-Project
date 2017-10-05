using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_MixLevels : MonoBehaviour
{
    [SerializeField]
    AudioMixerGroup masterMixer;

    [SerializeField]
    AudioMixerGroup musicMixer;

    [SerializeField]
    AudioMixerGroup narrationMixer;

    [SerializeField]
    AudioMixerGroup combatMixer;

    [SerializeField]
    AudioMixerGroup atmosMixer;

    [SerializeField]
    AudioMixerGroup uiMixer;

    public void SetMasterLevel(float masterLvl)
    {
        masterMixer.audioMixer.SetFloat("MasterVolume", masterLvl);
    }

    public void SetMusicLevel(float musicLvl)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", musicLvl);
    }

    public void SetNarrationLevel(float narrationLvl)
    {
        narrationMixer.audioMixer.SetFloat("NarrationVolume", narrationLvl);
    }

    public void SetCombatLevel(float combatLvl)
    {
        combatMixer.audioMixer.SetFloat("CombatVolume", combatLvl);
    }

    public void SetAtmosLevel(float atmosLvl)
    {
        atmosMixer.audioMixer.SetFloat("AtmosVolume", atmosLvl);
    }

    public void SetUILevel(float uiLvl)
    {
        uiMixer.audioMixer.SetFloat("UIVolume", uiLvl);
    }
}