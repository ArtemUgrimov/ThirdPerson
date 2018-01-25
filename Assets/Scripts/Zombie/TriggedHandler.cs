using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class TriggedHandler : MonoBehaviour {

	Transform parent;
	int distanceToParent = 0;

	void Awake() {
		GetComponent<Rigidbody>().isKinematic = true;

		parent = Utils.GetSuperParent(transform, out distanceToParent);
		//ai = parent.GetComponent<ZombieAI>();
	}

//	void OnTriggerEnter(Collider other) {
//		if (parent)
//			parent.SendMessage("OnTriggerEnter", other);
//	}
//
//	void OnTriggerExit(Collider other) {
//		if (parent)
//			parent.SendMessage("OnTriggerExit", other);
//	}

	void GotHit(HitInfo info) {
		if (parent) {
			info.damage = distanceToParent == 0 ? info.damage : info.damage / distanceToParent;
			parent.gameObject.SendMessage("GotHit", info);
		}
	}

	void GotKick(Vector3 worldDirection) {
		if (parent) {
			parent.gameObject.SendMessage("GotKick", worldDirection);
		}
	}
}
