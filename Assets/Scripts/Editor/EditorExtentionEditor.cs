using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EditorExtentionEditor {

	static EditorExtentionEditor() {
		SceneView.onSceneGUIDelegate += HandleKeys;
	}

	static void HandleKeys(SceneView view) {
		Event e = Event.current;

		if (e != null) {
			if (e.isKey && e.type == EventType.KeyUp) {
				if (e.keyCode == KeyCode.Delete) {
					foreach (GameObject go in Selection.gameObjects) {
						Object.DestroyImmediate(go);
					}
				}
			}
		}
	}
}
