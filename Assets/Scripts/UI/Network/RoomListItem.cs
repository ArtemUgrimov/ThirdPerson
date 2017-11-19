using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {

	public delegate void JoinRoomDelegate(MatchInfoSnapshot matchInfo);
	private JoinRoomDelegate joinRoomCallback;

	[SerializeField]
	Text roomNameText;
	MatchInfoSnapshot match;

	public void Setup (MatchInfoSnapshot matchInfo, JoinRoomDelegate joinCallback) {
		Debug.Log("Found match " + matchInfo.name);
		joinRoomCallback = joinCallback;
		match = matchInfo;
		roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
	}

	public void JoinRoom () {
		joinRoomCallback.Invoke (match);
	}
}
