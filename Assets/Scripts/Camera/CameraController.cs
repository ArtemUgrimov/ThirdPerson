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

	public void DeactivateCamera() {
		cameraRef.SetActive (false);
	}

	public void ActivateCamera() {
		cameraRef.SetActive (true);
	}
}
