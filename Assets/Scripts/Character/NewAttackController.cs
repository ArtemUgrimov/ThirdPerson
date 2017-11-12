using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackController : MonoBehaviour {

	static float CROSSFADE_TIME = 0.2f;
	static int LAYER = 0;
	static string PREFIX = "Base Layer.Sword.Combat.Attack";

	Animator animator;

	void Start () {
		animator = GetComponent<Animator> ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			string anim = GetRandomAttackAnim ();
			Debug.Log (anim);
			animator.CrossFade (anim, CROSSFADE_TIME, LAYER);
			var routine = PlayAnim (CROSSFADE_TIME, anim);
			StartCoroutine (routine);
		}
	}

	IEnumerator PlayAnim(float delta, string name) {
		yield return new WaitForSeconds (delta);
		animator.Play (name, LAYER);
	}

	string GetRandomAttackAnim() {
		return PREFIX + Random.Range (1, 5).ToString() + "_0";
	}
}
