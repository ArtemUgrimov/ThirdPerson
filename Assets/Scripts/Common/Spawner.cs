using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[SerializeField]
	private GameObject spawnablePrefab;
	[SerializeField]
	private float intervalSec = 1.0f;
	[SerializeField]
	private int packCount = 1;

	private float lastSpawnTime;

	void Update () {
		if (Time.timeSinceLevelLoad - lastSpawnTime > intervalSec) {
			for (int i = 0; i < packCount; ++i) {
				Instantiate (spawnablePrefab, transform.position, transform.rotation);
			}
			lastSpawnTime = Time.timeSinceLevelLoad;
		}
	}
}
