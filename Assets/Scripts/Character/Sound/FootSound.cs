using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound : MonoBehaviour {

	private CharacterSoundManager manager;
	private GameObject superParent;
	private float lastPlayTime = 0;
	private float DELAY = 0.4f;

	void Start() {
		superParent = Utils.GetSuperParent(transform).gameObject;
		manager = superParent.GetComponent<CharacterSoundManager>();
	}

	void OnTriggerEnter(Collider col) {
		string col_tag = col.gameObject.tag;
		if (Time.timeSinceLevelLoad - lastPlayTime > DELAY && col_tag != "Body" && col_tag != "Player") {
			lastPlayTime = Time.timeSinceLevelLoad;
			manager.FootDown(transform.position);
//			Debug.Log(name);
		}
	}
}
