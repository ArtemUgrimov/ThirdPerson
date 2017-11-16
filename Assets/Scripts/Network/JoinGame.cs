using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

	List<GameObject> roomList = new List<GameObject>();
	NetworkManager networkManager;

	[SerializeField]
	Text status;
	[SerializeField]
	GameObject roomListItemPrefab;
	[SerializeField]
	Transform roomListParent;

	void Start () {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker ();
		}
		RefreshRoomList ();
	}

	public void RefreshRoomList () {
		networkManager.matchMaker.ListMatches (0, 20, "", false, 0, 0, OnMatchList);
		status.text = "Loading...";
		ClearRoomList ();
	}

	void OnMatchList (bool b, string s, List<MatchInfoSnapshot> matchList) {
		status.text = "";
		if (matchList == null) {
			status.text = "Error. Cannot get room list";
			return;
		}

		foreach (MatchInfoSnapshot matchInfo in matchList) {
			GameObject roomListItemGO = Instantiate (roomListItemPrefab);
			roomListItemGO.transform.SetParent(roomListParent);
			RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem> ();
			if (roomListItem != null) {
				roomListItem.Setup (matchInfo, JoinRoom);
			}


			//have a component that will take care of setting name/amount of users
			roomList.Add (roomListItemGO);
		}

		if (matchList.Count == 0) {
			status.text = "No rooms at the moment";
		}
	}

	void ClearRoomList () {
		foreach (GameObject go in roomList) {
			Destroy (go);
		}
		roomList.Clear ();
	}

	void JoinRoom (MatchInfoSnapshot matchInfo) {
		networkManager.matchMaker.JoinMatch (matchInfo.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
	}
}
