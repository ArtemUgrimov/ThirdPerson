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

	void Start() {
		currentHealth = maxHealth;
	}

	void GotHit(int amount) {
		if (dead) {
			return;
		}
		currentHealth -= amount;
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
}
