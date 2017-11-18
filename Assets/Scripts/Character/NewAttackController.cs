using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewAttackController : NetworkBehaviour {

	static float CROSSFADE_TIME = 0.2f;
	static int LAYER = 0;
	static string PREFIX = "Base Layer.Sword.Combat.Attack";

	static int combatId = Animator.StringToHash("Combat");
	static int attackAnimIndexId = Animator.StringToHash("AttackAnimIndex");

	//IEnumerator currentCoroutine = null;
	IEnumerator attackStopRoutine = null;

	bool attackDone = true;
	bool attacking = false;

	Animator animator;
	NetworkAnimator netAnimator;

	void Start () {
		animator = GetComponent<Animator> ();
		netAnimator = GetComponent<NetworkAnimator> ();
	}

	void Update() {
		if (isLocalPlayer && Input.GetButtonDown("Fire1") && attackDone) {
			//			if (attackStopRoutine != null) {
			//				StopCoroutine (attackStopRoutine);
			//				attackStopRoutine = null;
			//			}
			//			string anim = GetRandomAttackAnim ();

			//			float crossFadeTime = GetCrossfadeTime ();
			//			animator.CrossFade (anim, crossFadeTime, LAYER);
			//			currentCoroutine = PlayAnim (crossFadeTime, anim);
			//			StartCoroutine (currentCoroutine);

			int index = Random.Range(0, 5);
			animator.SetInteger(attackAnimIndexId, index);
			animator.SetBool(combatId, true);

			attacking = true;
			attackDone = false;
		}
	}

	//IEnumerator PlayAnim(float delta, string name) {
		//yield return new WaitForSeconds (delta);

		//Debug.Log ("Playing " + name + " animation!");
		//animator.Play (name, LAYER);
//
//		float maxWeight = 0;
//		AnimationClip clip = null;
//		foreach (var info in animator.GetCurrentAnimatorClipInfo(0)) {
//			if (info.weight > maxWeight) {
//				maxWeight = info.weight;
//				clip = info.clip;
//			}
//		}
//		if (clip != null) {
//			float len = clip.length;
//			float norm = (len - delta) / len;
//			Debug.Log (norm);
//			animator.Play (name, LAYER, norm);
//		} else {
//			animator.Play (name, LAYER, 0);
//		}
//		currentCoroutine = null;
//	}

	IEnumerator AttackStop(float delta) {
		yield return new WaitForSeconds (delta);
		attacking = false;
		attackStopRoutine = null;
		Debug.Log ("Attack stopped!");
	}

	float GetCrossfadeTime() {
		return attacking ? CROSSFADE_TIME : CROSSFADE_TIME;// * 1.5f;
	}

	float GetStartTime() {
		return attacking ? 0.2f : 0.0f;
	}

	string GetRandomAttackAnim() {
		return PREFIX + Random.Range (1, 6).ToString() + "_0";
	}

	void SendEvent(string ev) {
		if (ev == "toStrike") {
			Debug.Log ("Attack done!");

			attackDone = true;
			var info = animator.GetCurrentAnimatorStateInfo (LAYER);
			float endTime = info.length * (1 - info.normalizedTime);
			attackStopRoutine = AttackStop (endTime);
			StartCoroutine (attackStopRoutine);
			animator.SetBool(combatId, false);
		}
	}

	void WeaponChanged(object info) {
		Debug.Log(gameObject.name + " changing weapon");
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
