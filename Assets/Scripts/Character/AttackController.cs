using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackController : NetworkBehaviour {

	static int combatId = Animator.StringToHash("Combat");
	static int attackAnimIndexId = Animator.StringToHash("AttackAnimIndex");

	bool attackDone = true;

	Animator animator;
	IEnumerator endCoroutine = null;

	void Awake () {
		animator = GetComponent<Animator> ();
	}

	void Update() {
		if (isLocalPlayer && InputControl.GetButtonDown("Fire1") && attackDone) {
			int index = 0;//Random.Range(0, 5);
			animator.SetInteger(attackAnimIndexId, index);
			animator.SetBool(combatId, true);

			if (endCoroutine != null) {
				StopCoroutine(endCoroutine);
			}
			endCoroutine = AttackDone(0.50f);
			StartCoroutine(endCoroutine);

			attackDone = false;
		}
	}

	IEnumerator AttackDone(float time) {
		yield return new WaitForSeconds(time);
		animator.SetBool(combatId, false);
		attackDone = true;
		endCoroutine = null;
	}

	void SendEvent(string ev) {
		if (ev == "toStrike") {
			attackDone = true;
			animator.SetBool(combatId, false);
		}
	}

	void WeaponChanged(object info) {
		if (!isLocalPlayer)
			return;
		Debug.Log("Weapon changed");
		WeaponType type = (WeaponType)info;
		switch (type) {
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
