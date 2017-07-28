using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour {

    private const string PLAYER_TAG = "Player";
    PlayerController playerController;

    [SerializeField]
    private int reducedWalkSpeed = 6;
    [SerializeField]
    private int normalWalkSpeed = 12;

    [SerializeField]
    private int reducedJumpSpeed = 7;
    [SerializeField]
    private int normalJumpSpeed = 15;

	void OnTriggerEnter(Collider other)
    {
		if (other.tag == PLAYER_TAG)
        {
            playerController = other.GetComponentInChildren<PlayerController>();
            playerController.moveSettings.forwardVel = reducedWalkSpeed;
            playerController.moveSettings.rightVel = reducedWalkSpeed;
            playerController.moveSettings.jumpVel = reducedJumpSpeed;
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            playerController = other.GetComponentInChildren<PlayerController>();
            playerController.moveSettings.forwardVel = normalWalkSpeed;
            playerController.moveSettings.rightVel = normalWalkSpeed;
            playerController.moveSettings.jumpVel = normalJumpSpeed;
        }
    }
}
