using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AddTriggerScript)), CanEditMultipleObjects]
public class AddTriggerScriptEditor : Editor {
	public override void OnInspectorGUI() {
		EditorGUILayout.LabelField("TriggerHandler adder");
		if (GUILayout.Button("Add TriggerHandler")) {
			AddTriggerScript go = target as AddTriggerScript;

			AddTriggerHandlerTo(go.transform);
		}
	}

	void AddTriggerHandlerTo(Transform tf) {
		Rigidbody rb = tf.GetComponent<Rigidbody>();
		if (rb) {
			TriggedHandler th = tf.GetComponent<TriggedHandler>();
			tf.gameObject.tag = "Enemy";
			if (!th)
				tf.gameObject.AddComponent<TriggedHandler>();
		}

		for (int i = 0; i < tf.childCount; ++i) {
			AddTriggerHandlerTo(tf.GetChild(i));
		}
	}
}
