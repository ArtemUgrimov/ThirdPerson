using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class TriggedHandler : MonoBehaviour {

	ZombieAI ai;
	int distanceToParent = 0;

	void Awake() {
		GetComponent<Rigidbody>().isKinematic = true;

		Transform parent = Utils.GetSuperParent(transform, out distanceToParent);
		ai = parent.GetComponent<ZombieAI>();
	}

	void OnTriggerEnter(Collider other) {
		if (ai)
			ai.OnTriggerEnter(other);
	}

	void OnTriggerExit(Collider other) {
		if (ai)
			ai.OnTriggerExit(other);
	}

	void GotHit(int damage) {
		if (ai) {
			int dmg = distanceToParent == 0 ? damage : damage / distanceToParent;
			ai.gameObject.SendMessage("GotHit", dmg);
		}
	}
}
