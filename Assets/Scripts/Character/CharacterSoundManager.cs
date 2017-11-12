using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour {

	[SerializeField]
	List<AudioClip> footsteps = new List<AudioClip>();

	void FootDown() {
		AudioSource.PlayClipAtPoint(footsteps[Random.Range(0, footsteps.Count)], transform.position);
	}
}
