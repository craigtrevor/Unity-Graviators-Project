using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonNetworked_Soundscape : MonoBehaviour {

    //Player Audio Source

    [SerializeField]
    private AudioSource[] playerAudioSources;

    //Player Audio Clips

    [SerializeField]
    private AudioClip[] playerAudioClips;

    //Booleans
    private bool soundPlayed = false;

    void Awake()
    {
        AutoAssignAudio();
    }

    public void PlayNonNetworkedSound(int clipID, int audioSource, float audioVolume)
    {
        playerAudioSources[audioSource].PlayOneShot(playerAudioClips[clipID]);
    }

    public void StopNonNetworkedSound(int clipID, int audioSource)
    {
        playerAudioSources[audioSource].Stop();
    }

    private IEnumerator WaitForSound(int clipID, int audioSourceID, float audioVolume, float soundDealy)
    {
        soundPlayed = true;
        yield return new WaitForSeconds(soundDealy);
        playerAudioSources[audioSourceID].volume = audioVolume;
        playerAudioSources[audioSourceID].PlayOneShot(playerAudioClips[clipID]);
        soundPlayed = false;
    }

    void AutoAssignAudio()
    {
        // Foley Audio
        playerAudioClips[0] = (AudioClip)Resources.Load("Foley_PlayerFootstep_Metal1");
        playerAudioClips[1] = (AudioClip)Resources.Load("Foley_PlayerFootstep_Metal2");
        playerAudioClips[2] = (AudioClip)Resources.Load("Foley_PlayerFootstep_Metal1");
        playerAudioClips[3] = (AudioClip)Resources.Load("Foley_PlayerFootstep_Metal2");
        playerAudioClips[4] = (AudioClip)Resources.Load("Foley_PlayerJump");

        // Attack Audio
        playerAudioClips[5] = (AudioClip)Resources.Load("ErrorNoName's Melee");
        playerAudioClips[6] = (AudioClip)Resources.Load("Unit-D1's Melee");
        playerAudioClips[7] = (AudioClip)Resources.Load("Sparkus' Melee");
        playerAudioClips[8] = (AudioClip)Resources.Load("ErrorNoName's Ranged Attack");
        playerAudioClips[9] = (AudioClip)Resources.Load("Sparkus' Ranged Attack");
        playerAudioClips[10] = (AudioClip)Resources.Load("UnitD1's Ranged Attack");
        playerAudioClips[11] = (AudioClip)Resources.Load("ErrorNoName's Ranged Impact");
        playerAudioClips[12] = (AudioClip)Resources.Load("UnitD1's Ranged Impact");
        playerAudioClips[13] = (AudioClip)Resources.Load("Range Attack Reload");
        playerAudioClips[14] = (AudioClip)Resources.Load("WeaponHit");

        // Atmos Audio
        playerAudioClips[15] = (AudioClip)Resources.Load("Atmos_Indoor");
        playerAudioClips[16] = (AudioClip)Resources.Load("Atmos_OutdoorWind");
        playerAudioClips[17] = (AudioClip)Resources.Load("Atmos_SpookyWind");

        // Narration Audio
        playerAudioClips[18] = (AudioClip)Resources.Load("Respawn Messages-01");
        playerAudioClips[19] = (AudioClip)Resources.Load("Respawn Messages-02");
        playerAudioClips[20] = (AudioClip)Resources.Load("Die Message 1");
        playerAudioClips[21] = (AudioClip)Resources.Load("Die Message 2");
        playerAudioClips[22] = (AudioClip)Resources.Load("Die Message 3");
        playerAudioClips[23] = (AudioClip)Resources.Load("Win Messages-noname");
        playerAudioClips[24] = (AudioClip)Resources.Load("Win Messages-sparkus");
        playerAudioClips[25] = (AudioClip)Resources.Load("Win Messages-D1");
    }
}
