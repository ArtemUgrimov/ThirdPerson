using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RecursiveTagSetter : MonoBehaviour {


	static void SetTagTo(GameObject go, string tag, string ignore) {
		go.tag = tag;
	}
	
	[MenuItem("GameObject/Utils/TagSetter")]
	static void CreateCustomGameObject(MenuCommand menuCommand)
	{
		SetTagTo (Selection.activeGameObject, Selection.activeGameObject.tag, "Weapon");
	}
}
