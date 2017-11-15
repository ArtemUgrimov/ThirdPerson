using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public MatchSettings matchSettings;

	[SerializeField]
	GameObject sceneCamera;

	void Awake () {
		if (instance != null) {
			Debug.LogError ("More than one GameManager in scene");
		} else {
			instance = this;
		}
	}

	public void SetSceneCameraActive(bool isActive) {
		if (sceneCamera != null) {
			sceneCamera.SetActive (isActive);
		}
	}

	#region Player Tracking
	const string PLAYER_ID_PREFIX = "Player ";
	static Dictionary<string, Player> players = new Dictionary<string, Player>();

	public static void RegisterPlayer (string netID, Player player) {
		string playerID = PLAYER_ID_PREFIX + netID;
		players.Add (playerID, player);
		player.transform.name = playerID;
	}

	public static void UnregisterPlayer(string playerID) {
		players.Remove (playerID);
	}

	public static Player GetPlayer(string playerID) {
		return players [playerID];
	}
	#endregion

//	void OnGUI () {
//		GUILayout.BeginArea (new Rect (200, 200, 200, 500));
//		GUILayout.BeginVertical ();
//		foreach (var player in players) {
//			GUILayout.Label (player.Key + "_" + player.Value.name);
//		}
//		GUILayout.EndVertical ();
//		GUILayout.EndArea ();
//	}
}
