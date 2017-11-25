using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackController : NetworkBehaviour {

	static int combatId = Animator.StringToHash("Combat");
	static int kickId = Animator.StringToHash("Kick");
	static int attackAnimIndexId = Animator.StringToHash("AttackAnimIndex");

	MainWeapon mainWeapon = null;

	bool attackDone = true;

	Animator animator;

	IEnumerator kickRoutine = null;
	IEnumerator endCoroutine = null;

	void Awake () {
		animator = GetComponent<Animator> ();
	}

	void Update() {
		if (isLocalPlayer && InputControl.GetButtonDown("Fire1") && attackDone) {
			int index = 0;//Random.Range(0, 5);
			animator.SetInteger(attackAnimIndexId, index);
			if (NeedKick ()) {
				animator.SetBool (kickId, true);
			} else {
				animator.SetBool (combatId, true);
				mainWeapon.Attacking = true;
				attackDone = false;
			}
			if (endCoroutine != null) {
				StopCoroutine(endCoroutine);
			}
			endCoroutine = AttackDone(0.50f);
			StartCoroutine(endCoroutine);
		}
	}

	IEnumerator AttackDone(float time) {
		yield return new WaitForSeconds(time);
		animator.SetBool(combatId, false);
		animator.SetBool (kickId, false);
		attackDone = true;
		endCoroutine = null;
		mainWeapon.Attacking = false;
	}

	bool NeedKick() {
		float distance = 1;
		int layer = 0;
		layer |= (1 << LayerMask.NameToLayer ("Enemy"));
		RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.up, transform.forward, out hit, distance, layer)) {
			kickRoutine = Kick (0, hit.collider.gameObject, hit.normal);
			return true;
		}
		return false;
	}

	IEnumerator Kick(float timeout, GameObject go, Vector3 dir) {
		yield return new WaitForSeconds (timeout);
		go.SendMessage ("GotKick", dir * -50.0f);
	}

	void SendEvent(string ev) {
		if (ev == "toStrike") {
			attackDone = true;
			animator.SetBool (combatId, false);
		} else if (ev == "kick") {
			if (kickRoutine != null) {
				StartCoroutine (kickRoutine);
				kickRoutine = null;
			}
		}
	}

	void UpdateWeapon(MainWeapon weapon) {
		mainWeapon = weapon;

		if (!isLocalPlayer)
			return;
		switch (mainWeapon.type) {
		case WeaponType.Axe:
			animator.SetBool("Axe", true);
			animator.SetBool("Sword", false);
			break;
		case WeaponType.Sword_Shield:
			animator.SetBool("Axe", false);
			animator.SetBool("Sword", true);
			break;
		case WeaponType.THSword:
			break;
		}
	}
}
