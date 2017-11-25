using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {
	[SerializeField]
	GameObject cameraRef;

	void Start () {
		if (!isLocalPlayer) {
			Destroy (cameraRef);
			cameraRef = null;
		}
	}

	public void DeactivateCamera() {
		if (cameraRef != null) {
			cameraRef.SetActive (false);
		}
	}

	public void ActivateCamera() {
		if (cameraRef != null) {
			cameraRef.SetActive (true);
		}
	}
}
