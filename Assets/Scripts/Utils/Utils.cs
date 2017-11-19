﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public static void SetKinematic(Transform tf, bool val) {
		Rigidbody rb = tf.GetComponent<Rigidbody>();
		Collider col = tf.GetComponent<Collider>();
		if (rb && col) {
			rb.isKinematic = val;
			col.isTrigger = val;
		}

		for (int i = 0; i < tf.childCount; ++i) {
			SetKinematic(tf.GetChild(i), val);
		}
	}

	public static void SetRagdoll(bool val, GameObject go) {
		SetKinematic(go.transform, !val);

		Collider col = go.GetComponent<Collider>();
		col.isTrigger = val;

		Rigidbody rb = go.GetComponent<Rigidbody>();
		rb.isKinematic = val;

		Animator animator = go.GetComponent<Animator>();
		animator.enabled = !val;
	}
}
