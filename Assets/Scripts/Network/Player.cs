using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CameraController))]
public class Player : NetworkBehaviour {

	[SyncVar]
	private bool isDead;
	public bool IsDead {
		get { return isDead; }
		protected set { isDead = value; }
	}

	[SerializeField]
	int maxHealth = 100;
	[SyncVar]
	int currentHealth;

	[SerializeField]
	Behaviour[] disableOnDeath;
	bool[] wasEnabled;

	bool firstSetup = true;

	public void Setup () {
		if (isLocalPlayer) {
			SetPlayerCamera ();
			PlayerUIController.Setup ();
		}

		CmdBroadcastNewPlayerSetup ();
	}

	[Command]
	private void CmdBroadcastNewPlayerSetup () {
		RpcSetupPlayerOnAllClients ();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients () {
		if (firstSetup) {
			wasEnabled = new bool[disableOnDeath.Length];
			for (int i = 0; i < wasEnabled.Length; i++) {
				wasEnabled [i] = disableOnDeath [i].enabled;
			}
			firstSetup = false;
		}
		SetDefaults ();
	}

	public void SetDefaults () {
		IsDead = false;
		currentHealth = maxHealth;
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = wasEnabled [i];
		}
		Collider col = GetComponent<Collider> ();
		if (col != null) {
			col.enabled = true;
		}
	}

	void GotHit(int amount) {
		CmdGotHit(amount);
	}

	[Command]
	void CmdGotHit(int amount) {
		RpcTakeDamage(amount);
	}

	[ClientRpc]
	public void RpcTakeDamage (int amount) {
		if (isDead) {
			return;
		}

		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
			Die ();
		} else {
			Debug.Log (transform.name + " now has " + currentHealth + " health");
		}
	}

	void Die () {
		IsDead = true;
		SendMessage ("IAmDead");

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = false;
		}
		Collider col = GetComponent<Collider> ();
		if (col != null) {
			col.enabled = false;
		}

		Invoke ("SetSceneCamera", GameManager.instance.matchSettings.respawnTime - 2.0f);

		Debug.Log (transform.name + " now dead");
		StartCoroutine (RespawnPlayer ());
	}

	IEnumerator RespawnPlayer () {
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);

		SendMessage ("Respawn");
		Transform spawnPoint = NetworkManager.singleton.GetStartPosition ();
		Debug.Log(spawnPoint.position);
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		yield return new WaitForSeconds (0.05f);
		Setup ();

		Debug.Log (transform.name + " respawned");
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState) {
		try {
			base.OnDeserialize (reader, initialState);
		} catch (System.Exception e) {
			Debug.LogError (e);
		}
	}

	void SetPlayerCamera() {
		GameManager.instance.SetSceneCameraActive (false);
		GetComponent<CameraController> ().ActivateCamera ();
	}

	void SetSceneCamera() {
		GameManager.instance.SetSceneCameraActive (true);
		GetComponent<CameraController> ().DeactivateCamera ();
	}
}
