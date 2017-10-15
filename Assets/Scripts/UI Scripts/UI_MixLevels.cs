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

    [SerializeField]
    Button[] mutedButton;

    [SerializeField]
    Image[] mutedImages;

    [SerializeField]
    Sprite[] mutedSprites;

    UI_MatchMixerLevels matchMixerLevels;

    float volumeLvl;

    bool isMuted;
    bool isMasterMuted;

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

    public void MuteMaster()
    {
        if (!isMuted)
        {
            audioMixers[0].audioMixer.SetFloat("MasterVolume", -40);
            sliderValues[0] = -40;
            audioSliders[0].value = -40;
            mutedImages[0].sprite = mutedSprites[1];
            isMuted = true;
            isMasterMuted = true;
        }

        else
        {
            audioMixers[0].audioMixer.SetFloat("MasterVolume", 0);
            sliderValues[0] = 0;
            audioSliders[0].value = 0;
            mutedImages[0].sprite = mutedSprites[0];
            isMuted = false;
            isMasterMuted = false;
        }
    }

    public void SetMusicLevel(float musicLvl)
    {
        audioMixers[1].audioMixer.SetFloat("MusicVolume", musicLvl);
        sliderValues[1] = musicLvl;
    }

    public void MuteMusic()
    {
        if (!isMuted)
        {
            audioMixers[1].audioMixer.SetFloat("MasterVolume", -40);
            sliderValues[1] = -40;
            audioSliders[1].value = -40;
            mutedImages[1].sprite = mutedSprites[1];
            isMuted = true;
        }

        else
        {
            if (!isMasterMuted)
            {
                audioMixers[1].audioMixer.SetFloat("MasterVolume", 0);
            }

            sliderValues[1] = 0;
            audioSliders[1].value = 0;
            mutedImages[1].sprite = mutedSprites[0];
            isMuted = false;
        }
    }

    public void SetNarrationLevel(float narrationLvl)
    {
        audioMixers[2].audioMixer.SetFloat("NarrationVolume", narrationLvl);
        sliderValues[2] = narrationLvl;
    }

    public void MuteNarration()
    {
        if (!isMuted)
        {
            audioMixers[2].audioMixer.SetFloat("MasterVolume", -40);
            sliderValues[2] = -40;
            audioSliders[2].value = -40;
            mutedImages[2].sprite = mutedSprites[1];
            isMuted = true;
        }

        else
        {
            if (!isMasterMuted)
            {
                audioMixers[2].audioMixer.SetFloat("MasterVolume", 0);
            }

            sliderValues[2] = 0;
            audioSliders[2].value = 0;
            mutedImages[2].sprite = mutedSprites[0];
            isMuted = false;
        }
    }

    public void SetCombatLevel(float combatLvl)
    {
        audioMixers[3].audioMixer.SetFloat("CombatVolume", combatLvl);
        sliderValues[3] = combatLvl;
    }

    public void MuteCombat()
    {
        if (!isMuted)
        {
            audioMixers[3].audioMixer.SetFloat("MasterVolume", -40);
            sliderValues[3] = -40;
            audioSliders[3].value = -40;
            mutedImages[3].sprite = mutedSprites[1];
            isMuted = true;
        }

        else
        {
            if (!isMasterMuted)
            {
                audioMixers[3].audioMixer.SetFloat("MasterVolume", 0);
            }

            sliderValues[3] = 0;
            audioSliders[3].value = 0;
            mutedImages[3].sprite = mutedSprites[0];
            isMuted = false;
        }
    }

    public void SetAtmosLevel(float atmosLvl)
    {
        audioMixers[4].audioMixer.SetFloat("AtmosVolume", atmosLvl);
        sliderValues[4] = atmosLvl;
    }

    public void MuteAtmos()
    {
        if (!isMuted)
        {
            audioMixers[4].audioMixer.SetFloat("MasterVolume", -40);
            sliderValues[4] = -40;
            audioSliders[4].value = -40;
            mutedImages[4].sprite = mutedSprites[1];
            isMuted = true;
        }

        else
        {
            if (!isMasterMuted)
            {
                audioMixers[4].audioMixer.SetFloat("MasterVolume", 0);
            }

            sliderValues[4] = 0;
            audioSliders[4].value = 0;
            mutedImages[4].sprite = mutedSprites[0];
            isMuted = false;
        }
    }

    public void SetUILevel(float uiLvl)
    {
        audioMixers[5].audioMixer.SetFloat("UIVolume", uiLvl);
        sliderValues[5] = uiLvl;
    }

    public void MuteUI()
    {
        if (!isMuted)
        {
            audioMixers[5].audioMixer.SetFloat("MasterVolume", -40);
            sliderValues[5] = -40;
            audioSliders[5].value = -40;
            mutedImages[5].sprite = mutedSprites[1];
            isMuted = true;
        }

        else
        {
            if (!isMasterMuted)
            {
                audioMixers[5].audioMixer.SetFloat("MasterVolume", 0);
            }

            sliderValues[5] = 0;
            audioSliders[5].value = 0;
            mutedImages[5].sprite = mutedSprites[0];
            isMuted = false;
        }
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