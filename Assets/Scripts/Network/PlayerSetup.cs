using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterAnimatorController))]
[RequireComponent(typeof(CameraController))]
public class PlayerSetup : NetworkBehaviour {

	public const string REMOTE_LAYER_NAME = "RemotePlayer";

	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGraphics;

	[SerializeField]
	Behaviour[] componentsToDisable;

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

	void SetLayerRecursively (GameObject graphics, int layer)
	{
		graphics.layer = layer;
		foreach (Transform child in graphics.transform) {
			SetLayerRecursively (child.gameObject, layer);
		}
	}

	void SetupUI () {
		PlayerUIController.Instance().ShowInGameUI();
	}

	void RemoveUI () {
		PlayerUIController.Instance().ShowMenuUI();
	}

	public override void OnStartClient () {
		base.OnStartClient ();

		string id = GetComponent<NetworkIdentity> ().netId.ToString();
		Player player = GetComponent<Player> ();

		GameManager.RegisterPlayer (id, player);
	}

	void OnDisable () {
		if (isLocalPlayer) {
			RemoveUI ();
			GameManager.instance.SetSceneCameraActive (true);
		}
		GameManager.UnregisterPlayer (transform.name);
	}

	void DisableComponents () {
		for (int i = 0; i < componentsToDisable.Length; ++i) {
			componentsToDisable [i].enabled = false;
		}
	}

	void AssignRemotePlayer () {
//		gameObject.layer = LayerMask.NameToLayer (REMOTE_LAYER_NAME);
		//TODO
	}
}
