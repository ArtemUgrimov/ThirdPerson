using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
	None = 0,
	OneHand,
	TwoHands
}

[System.Serializable]
public struct PositionRotation {
    public Vector3 position;
    public Vector3 rotation;
}

public class MainWeapon : MonoBehaviour {

    [SerializeField]
    private PositionRotation transformActive;
    [SerializeField]
    private PositionRotation transformInactive;
	[SerializeField]
	private int damage = 25;

	public WeaponType type;

	GameObject superParent = null;

	private bool attacking = false;
	public bool Attacking {
		get { return attacking; }
		set { attacking = value; }
	}

    public void Equip(Transform holder) {
        transform.parent = holder;
        transform.localPosition = transformActive.position;
        transform.localRotation = Quaternion.Euler(transformActive.rotation);
    }

    public void Unequip(Transform holder) {
        transform.parent = holder;
        transform.localPosition = transformInactive.position;
        transform.localRotation = Quaternion.Euler(transformInactive.rotation);
    }

    void NotifyPlayer() {
        Transform superTrans = Utils.GetSuperParent(transform);
        if (superTrans != null) {
            superParent = superTrans.gameObject;
            superParent.SendMessage("UpdateWeapon", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnEnable() {
        NotifyPlayer();
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
