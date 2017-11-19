using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponType {
	None,
	Sword_Shield,
	THSword,
	Axe
}

[System.Serializable]
public enum AttackState {
	None,
	Attacking
}

public class CombatHandler : MonoBehaviour {

	WeaponType currentWeapon = WeaponType.None;
	AttackState attackState;

	int damage = 25;

	void Start() {
		changeWeapon(WeaponType.Sword_Shield);
	}

	void OnDestroy() {
	}

	void UpdateDamage(object arg) {
		damage = (int)arg;
	}

	void UpdateTarget(object arg) {
		if (arg == null)
			return;
		
		GameObject target = arg as GameObject;
		var super = Utils.GetSuperParent (target.transform);
		if (attackState == AttackState.Attacking && super != gameObject) {
			target.SendMessage("GotHit", damage);
		}
	}

	void SendEvent(string eventName) {
		//if (eventName == "toStrike") {
		//	if (target != null) {
		//		target.SendMessage("GotHit", damage);
		//	}
		//}
	}

	void AttackBegin() {
		attackState = AttackState.Attacking;
	}

	void AttackEnd() {
		attackState = AttackState.None;
	}

	void changeWeapon(WeaponType to) {
		if (to == currentWeapon)
			return;
		currentWeapon = to;
		SendMessage("WeaponChanged", currentWeapon, SendMessageOptions.DontRequireReceiver);
	}
}