using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

	[SyncVar]
	private bool isDead = false;
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

	public void Setup () {
		GameManager.instance.SetSceneCameraActive (false);
		GetComponent<PlayerSetup> ().playerUIInstance.SetActive (true);

		CmdBroadcastNewPlayerSetup ();
	}

	[Command]
	private void CmdBroadcastNewPlayerSetup () {
		RpcSetupPlayerOnAllClients ();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients () {
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled [i] = disableOnDeath [i].enabled;
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

		if (isLocalPlayer) {
			GameManager.instance.SetSceneCameraActive (true);
			GetComponent<PlayerSetup> ().playerUIInstance.SetActive (false);
		}

		Debug.Log (transform.name + " now dead");
		StartCoroutine (Respawn ());
	}

	IEnumerator Respawn () {
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);

		SetDefaults ();
		GameManager.instance.SetSceneCameraActive (false);
		GetComponent<PlayerSetup> ().playerUIInstance.SetActive (true);

		Transform spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;
		Debug.Log (transform.name + " respawned");
	}
}
