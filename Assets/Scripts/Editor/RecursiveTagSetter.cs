using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RecursiveTagSetter : MonoBehaviour {


	static void SetTagTo(GameObject go, string tag, string ignore) {
		go.tag = tag;
		foreach (Transform child in go.transform) {
			SetTagTo (child.gameObject, tag, ignore);
		}
	}
	
	[MenuItem("GameObject/Utils/TagSetter")]
	static void CreateCustomGameObject(MenuCommand menuCommand)
	{
		SetTagTo (Selection.activeObject as GameObject, Selection.activeGameObject.tag, "Weapon");
	}
}
