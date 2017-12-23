using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour {
	[SerializeField]
	private List<AudioClip> footsteps = new List<AudioClip>();

	public void FootDown(Vector3 position) {
		AudioSource.PlayClipAtPoint(footsteps [Random.Range (0, footsteps.Count)], position);
	}
}
