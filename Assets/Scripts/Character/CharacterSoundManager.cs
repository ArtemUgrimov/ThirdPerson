using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour {

	[SerializeField]
	List<AudioClip> footsteps = new List<AudioClip>();
	Animator animator;

	void Start() {
		animator = GetComponent<Animator> ();
	}

	void FootDown() {
//		foreach (var clip in animator.GetCurrentAnimatorClipInfo(0)) {
//			if (clip.weight > 0.5f) {
//				AudioSource.PlayClipAtPoint (footsteps [Random.Range (0, footsteps.Count)], transform.position);
//			}
//		}
	}
}
