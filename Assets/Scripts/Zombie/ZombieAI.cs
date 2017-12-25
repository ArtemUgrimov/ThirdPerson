using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class ZombieAI : NetworkBehaviour {

	NavMeshAgent agent;
	Transform target;

	void Awake() {
		agent = GetComponent<NavMeshAgent> ();
		agent.isStopped = true;
	}

	public void OnTriggerEnter(Collider other) {
		if (target == null && other.gameObject.tag == "Player") {
			Debug.Log ("Targeting " + other.gameObject.name);
			target = other.gameObject.transform;
			agent.SetDestination (target.position);
			agent.isStopped = false;
		}
	}

	public void OnTriggerStay(Collider other) {
		if (target == null && other.gameObject.tag == "Player") {
			target = other.gameObject.transform;
		}
		if (target != null) {
			agent.SetDestination (target.position);
		}
	}

	public void OnTriggerExit(Collider other) {
		if (target != null && other.gameObject.tag == "Player") {
			Debug.Log ("Leaving target " + other.gameObject.name);
			target = null;
			agent.isStopped = true;
		}
	}
}
