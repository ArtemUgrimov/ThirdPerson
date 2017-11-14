using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewAttackController : NetworkBehaviour {

	static float CROSSFADE_TIME = 0.2f;
	static int LAYER = 0;
	static string PREFIX = "Base Layer.Sword.Combat.Attack";

	IEnumerator currentCoroutine = null;
	IEnumerator attackStopRoutine = null;

	bool attackDone = true;
	bool attacking = false;

	Animator animator;

	void Start () {
		animator = GetComponent<Animator> ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer && Input.GetButtonDown ("Fire1") && attackDone) {
			if (attackStopRoutine != null) {
				StopCoroutine (attackStopRoutine);
				attackStopRoutine = null;
			}

			attacking = true;
			string anim = GetRandomAttackAnim ();
			Debug.Log (anim);
			animator.CrossFade (anim, GetCrossfadeTime(), LAYER);
			currentCoroutine = PlayAnim (CROSSFADE_TIME, anim);
			StartCoroutine (currentCoroutine);
		}
	}

	IEnumerator PlayAnim(float delta, string name) {
		yield return new WaitForSeconds (delta);
		animator.Play (name, LAYER, GetStartTime());
		currentCoroutine = null;
		attackDone = false;
	}

	IEnumerator AttackStop(float delta) {
		yield return new WaitForSeconds (delta);
		attacking = false;
		attackStopRoutine = null;
	}

	float GetCrossfadeTime() {
		return attacking ? CROSSFADE_TIME : CROSSFADE_TIME * 2.5f;
	}

	float GetStartTime() {
		return attacking ? 0.2f : 0.0f;
	}

	string GetRandomAttackAnim() {
		return PREFIX + Random.Range (1, 6).ToString() + "_0";
	}

	void SendEvent(string ev) {
		if (ev == "toStrike") {
			attackDone = true;
			var info = animator.GetCurrentAnimatorStateInfo (LAYER);
			float endTime = info.length * (1 - info.normalizedTime);
			attackStopRoutine = AttackStop (endTime);
			StartCoroutine (attackStopRoutine);
		}
	}
}
