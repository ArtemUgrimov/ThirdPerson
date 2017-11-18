using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {
	[SerializeField]
	GameObject cameraRef;

	void Start () {
		if (!isLocalPlayer) {
			Destroy (cameraRef);
		}
	}

	void IAmDead() {
		Debug.Log("DEEEEEEAD");
		cameraRef.SetActive (false);
	}

	void Respawn() {
		cameraRef.SetActive (true);
	}
}
