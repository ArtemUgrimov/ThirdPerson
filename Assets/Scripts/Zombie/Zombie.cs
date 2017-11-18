using UnityEngine;
using UnityEngine.Networking;

public class Zombie : NetworkBehaviour
{
	[SyncVar]
	int currentHealth;
	[SyncVar]
	bool dead;

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
}
