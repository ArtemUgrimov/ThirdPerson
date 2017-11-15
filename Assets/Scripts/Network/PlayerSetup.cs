using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	public const string REMOTE_LAYER_NAME = "RemotePlayer";

	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGraphics;

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	GameObject playerUIPrefab;
	[HideInInspector]
	public GameObject playerUIInstance;

	void Start () {
		if (!isLocalPlayer) {
			DisableComponents ();
			AssignRemotePlayer ();
		} else {
			SetLayerRecursively (playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
			SetupUI ();
			GetComponent<Player> ().Setup ();
		}

	}

	void SetLayerRecursively (GameObject playerGraphics, int layer)
	{
		playerGraphics.layer = layer;
		foreach (Transform child in playerGraphics.transform) {
			SetLayerRecursively (child.gameObject, layer);
		}
	}

	void SetupUI () {
		playerUIInstance = Instantiate(playerUIPrefab);
		playerUIInstance.name = playerUIPrefab.name;
	}

	void RemoveUI () {
		Destroy (playerUIInstance);
	}

	public override void OnStartClient () {
		base.OnStartClient ();

		string id = GetComponent<NetworkIdentity> ().netId.ToString();
		Player player = GetComponent<Player> ();

		GameManager.RegisterPlayer (id, player);
	}

	void OnDisable () {
		GameManager.instance.SetSceneCameraActive (true);
		GameManager.UnregisterPlayer (transform.name);
		RemoveUI ();
	}

	void DisableComponents () {
		for (int i = 0; i < componentsToDisable.Length; ++i) {
			componentsToDisable [i].enabled = false;
		}
	}

	void AssignRemotePlayer () {
		gameObject.layer = LayerMask.NameToLayer (REMOTE_LAYER_NAME);
	}
}
