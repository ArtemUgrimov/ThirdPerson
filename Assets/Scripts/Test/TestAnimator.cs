using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AnimatorState {
	None,
	Idle,
	Walking,
	Running,
	Attacking
}

public class TestAnimator : MonoBehaviour {

	AnimatorState state = AnimatorState.None;
	Animator animator;
	IEnumerator lastRoutine;

	string stateMachine = "Sword";
	string currentAnimation = "None";



	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	void Update () {
		HandleInput ();
		PlayAnimation ();
	}

	void HandleInput() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		float magnitude = Mathf.Sqrt (horizontal * horizontal + vertical * vertical);
		if (Input.GetButtonDown ("Fire1")) {
			state = AnimatorState.Attacking;
		} else if (state != AnimatorState.Attacking) {
			if (Mathf.Abs (magnitude) > Mathf.Epsilon) {
				state = AnimatorState.Walking;
			} else {
				state = AnimatorState.Idle;
			}

			animator.SetFloat ("Horizontal", horizontal);
			animator.SetFloat ("Vertical", vertical);
		}
	}

	void PlayAnimation() {
		switch (state) {
		case AnimatorState.Idle:
		case AnimatorState.Walking:
			StartCurrentAnimation ("Locomotion");
			break;
		case AnimatorState.Attacking:
			if (StartCurrentAnimation ("Attack_0")) {
				var routine = StartAfterCurrent ("Locomotion", AnimatorState.Idle);
				StartCoroutine (routine);
			}
			break;
		}
	}

	IEnumerator StartAfterCurrent(string animation, AnimatorState newState) {
		var si = animator.GetCurrentAnimatorStateInfo (0);
		yield return new WaitForSeconds (si.length * (1 - si.normalizedTime) * 0.8f);
		state = newState;
		Debug.Log (newState);
		StartCurrentAnimation (animation);
	}

	bool StartCurrentAnimation(string animation) {
		if (currentAnimation == animation) {
			return false;
		}
		currentAnimation = animation;
		string name = stateMachine + "." + currentAnimation;
		animator.CrossFade (name, 0.2f, 0);
		if (lastRoutine != null) {
			StopCoroutine (lastRoutine);
		}
		lastRoutine = PlayAnim (0.2f, name);
		StartCoroutine (lastRoutine);
		Debug.Log (animator.GetCurrentAnimatorClipInfo (0)[0].clip.name);
		return true;
	}

	IEnumerator PlayAnim(float delay, string name) {
		yield return new WaitForSeconds (delay);
		animator.Play (name, 0);
		lastRoutine = null;
	}
}
