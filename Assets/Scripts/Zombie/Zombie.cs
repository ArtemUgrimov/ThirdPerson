using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Zombie : NetworkBehaviour
{
	[SerializeField]
	int maxHealth = 100;
	[SyncVar]
	int currentHealth;
	[SyncVar]
	bool dead;
	[SerializeField]
	GameObject bloodPrefab;

	void Start() {
		currentHealth = maxHealth;
	}

	void GotHit(HitInfo info) {
		if (dead) {
			return;
		}

		if (bloodPrefab != null) {
			GameObject blood = (GameObject)Instantiate (bloodPrefab, info.transform.position, info.transform.rotation);
			StartCoroutine (RemoveBlood (1.0f, blood));
		}

		currentHealth -= info.damage;
		if (currentHealth <= 0) {
			dead = true;
			SendMessage("IAmDead");
		}
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState) {
		try {
			base.OnDeserialize (reader, initialState);
		} catch (System.Exception e) {
			Debug.LogError (e);
		}
	}

	IEnumerator RemoveBlood(float dt, GameObject go) {
		yield return new WaitForSeconds (dt);
		Destroy (go);
	}
}
