using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeapon : MonoBehaviour {

	[SerializeField]
	int damage = 25;

	GameObject superParent = null;

	void OnEnable() {
		Transform superTrans = Utils.GetSuperParent(transform);
		if (superTrans != null) {
			superParent = superTrans.gameObject;
			superParent.SendMessage("UpdateDamage", damage);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			if (superParent != null) {
				superParent.SendMessage("UpdateTarget", other.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		//if (other.gameObject.tag == "Enemy") {
		//	if (superParent != null) {
		//		superParent.SendMessage("UpdateTarget", null);
		//	}
		//}
	}
}
