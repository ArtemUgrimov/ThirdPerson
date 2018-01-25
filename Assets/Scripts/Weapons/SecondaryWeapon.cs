using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryWeapon : Holdable {

	void NotifyPlayer() {
		Transform superTrans = Utils.GetSuperParent(transform);
		if (superTrans != null) {
			superParent = superTrans.gameObject;
			superParent.SendMessage("UpdateSecondary", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnEnable() {
		NotifyPlayer();
	}
}
