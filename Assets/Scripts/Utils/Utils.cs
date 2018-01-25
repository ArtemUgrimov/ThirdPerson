using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosRot {
	public Vector3 position;
	public Quaternion rotation = Quaternion.identity;
}

public static class Utils {
	public static Transform GetSuperParent(Transform tf) {
		Transform parent = tf.parent;
		while (parent != null && parent.parent != null) {
			parent = parent.parent;
		}
		return parent;
	}

	public static Transform GetSuperParent(Transform tf, out int distance) {
		Transform parent = tf.parent;
		distance = 0;
		while (parent != null && parent.parent != null) {
			++distance;
			parent = parent.parent;
		}
		return parent;
	}

	public static void SetKinematic(Transform tf, bool val, string tag = "Body") {
		Rigidbody rb = tf.GetComponent<Rigidbody>();
		Collider col = tf.GetComponent<Collider>();
		if (rb && col && tf.tag == tag) {
			rb.isKinematic = val;
			col.isTrigger = val;
		}

		for (int i = 0; i < tf.childCount; ++i) {
			SetKinematic(tf.GetChild(i), val, tag);
		}
	}

	public static void SetRagdoll(bool val, GameObject go, string tag = "Body") {
		SetKinematic(go.transform, !val, tag);

		Collider col = go.GetComponent<Collider>();
		if (col != null)
			col.isTrigger = val;

		Rigidbody rb = go.GetComponent<Rigidbody>();
		if (rb != null)
			rb.isKinematic = val;

		Animator animator = go.GetComponent<Animator>();
		if (animator != null)
			animator.enabled = !val;
	}
}
