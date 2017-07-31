﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class Network_RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomNameText;

    private MatchInfoSnapshot match;

    public void Setup (MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
    {
        transform.localScale = new Vector3(1, 1, 1);

        match = _match;
        joinRoomCallback = _joinRoomCallback;

        roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }

    public void JoinRoom()
    {
        joinRoomCallback.Invoke(match);
    }
}
