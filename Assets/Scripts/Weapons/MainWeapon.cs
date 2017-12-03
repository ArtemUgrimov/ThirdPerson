using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
	Axe,
	Sword_Shield,
	THSword
}

public class MainWeapon : MonoBehaviour {

	[SerializeField]
	int damage = 25;

	public WeaponType type;

	GameObject superParent = null;

	private bool attacking = false;
	public bool Attacking {
		get { return attacking; }
		set { attacking = value; }
	}

	void OnEnable() {
		Transform superTrans = Utils.GetSuperParent(transform);
		if (superTrans != null) {
			superParent = superTrans.gameObject;
			superParent.SendMessage("UpdateWeapon", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (attacking && other.gameObject != superParent && other.transform.root != superParent.transform && other.gameObject.tag == "Body") {
			other.gameObject.SendMessage ("GotHit", damage, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerExit(Collider other) {
		//if (other.gameObject.tag == "EnemyBody") {
			//if (superParent != null) {
				//superParent.SendMessage("UpdateTarget", other.gameObject);
			//}
		//}
	}
}
