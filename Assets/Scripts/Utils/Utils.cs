using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	public static Transform GetSuperParent(Transform tf) {
		Transform parent = tf.parent;
		while (parent != null && parent.parent != null && parent.tag != "Environment") {
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
}
