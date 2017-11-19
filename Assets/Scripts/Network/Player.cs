using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
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
			GameManager.instance.SetSceneCameraActive (false);
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

		if (isLocalPlayer) {
			GameManager.instance.SetSceneCameraActive (true);
		}

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

		//yield return new WaitForSeconds (0.1f);
		Setup ();

		Debug.Log (transform.name + " respawned");
	}
}
