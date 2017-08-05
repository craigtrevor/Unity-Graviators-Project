using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_Soundscape : NetworkBehaviour {

    //Player Audio Source

   [SerializeField]
    private AudioSource[] playerAudioSources;

    //Player Audio Clips

   [SerializeField]
    private AudioClip[] playerAudioClips;

    //Booleans
    private bool soundPlayed = false;

    public void PlaySound(int clipID, int audioSourceID, float soundDealy)
    {
        if (clipID >= 0 && audioSourceID >= 0 && clipID < playerAudioClips.Length && audioSourceID < playerAudioSources.Length)
        {
            CmdSendServerSoundID(clipID, audioSourceID, soundDealy);
        }
    }

    [Command]
    void CmdSendServerSoundID(int clipID, int audioSourceID, float soundDealy)
    {
        RpcSendSoundIDToClient(clipID, audioSourceID, soundDealy);
    }

    [ClientRpc]
    void RpcSendSoundIDToClient(int clipID, int audioSourceID, float soundDealy)
    {
        if (!soundPlayed)
        {
            playerAudioSources[audioSourceID].PlayOneShot(playerAudioClips[clipID]);
            StartCoroutine(WaitForSound(soundDealy));
        }
    }

    private IEnumerator WaitForSound(float soundDealy)
    {
        soundPlayed = true;
        yield return new WaitForSeconds(soundDealy);
        soundPlayed = false;
    }
}
