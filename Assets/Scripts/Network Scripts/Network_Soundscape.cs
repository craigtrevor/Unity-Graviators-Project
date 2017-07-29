using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_Soundscape : NetworkBehaviour {

    #region 
    //[SerializeField]
    //private AudioSource[] playerMasterSource;
    //[SerializeField]
    //private AudioSource playerMovementSource;
    //[SerializeField]
    //private AudioSource playerCombatSource;
    //[SerializeField]
    //private AudioSource playerFallSource;

    //[SerializeField]
    //private AudioClip[] playerMetalFootsteps;
    //[SerializeField]
    //private AudioClip[] playerWoodFootsteps;
    //[SerializeField]
    //private AudioClip[] playerGravity;
    //[SerializeField]
    //private AudioClip[] playerWeapon;
    //[SerializeField]
    //private AudioClip[] playerHit;
    //[SerializeField]
    //private AudioClip[] playerJump;

    #endregion

    // Player Audio Source

    [SerializeField]
    private AudioSource playerAudioSource;

    // Player Audio Clips

    [SerializeField]
    private AudioClip[] playerAudioClips;

    public void PlaySound(int id)
    {
        if (id >= 0 && id < playerAudioClips.Length)
        {
            CmdSendServerSoundID(id);
        }
    }

    [Command]
    void CmdSendServerSoundID(int id)
    {
        RpcSendSoundIDToClient(id);
    }

    [ClientRpc]
    void RpcSendSoundIDToClient(int id)
    {
        playerAudioSource.PlayOneShot(playerAudioClips[id]);
    }
}
