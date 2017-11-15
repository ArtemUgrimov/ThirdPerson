using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	[SerializeField]
	private uint roomSize = 10;
	private string roomName;
	private NetworkManager networkManager;

	void Start () {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker ();
		}
	}

	public void SetRoomName(string rName) {
		if (rName == null || rName.Length == 0) {
			rName = "default";
		}
		roomName = rName;
	}

	public void CreateRoom() {
		if (roomName != null && roomName.Length != 0) {
			Debug.Log ("Creating room " + roomName);
			networkManager.matchMaker.CreateMatch (roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		}
	}
}
