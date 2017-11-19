using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSyncronizer : MonoBehaviour {

	WheelCollider wc;

	void Start () {
		wc = GetComponent<WheelCollider> ();
	}
	
	void Update () {
		Transform visual = transform.GetChild (0);
		if (visual == null) {
			return;
		}
		Vector3 pos;
		Quaternion rot;
		wc.GetWorldPose (out pos, out rot);
		visual.position = pos;
		visual.rotation = rot * Quaternion.Euler(0, 90, 0);
	}
}
