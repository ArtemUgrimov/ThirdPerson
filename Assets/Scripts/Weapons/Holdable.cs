using UnityEngine;
using System.Collections;

public class Holdable : MonoBehaviour
{
	[Header("Holdable Attributes")]
	[SerializeField]
	protected PositionRotation transformActive;
	[SerializeField]
	protected PositionRotation transformInactive;

	protected GameObject superParent = null;

	virtual public void Equip(Transform holder) {
		transform.parent = holder;
		transform.localPosition = transformActive.position;
		transform.localRotation = Quaternion.Euler(transformActive.rotation);
	}

	virtual public void Unequip(Transform holder) {
		transform.parent = holder;
		transform.localPosition = transformInactive.position;
		transform.localRotation = Quaternion.Euler(transformInactive.rotation);
	}

	virtual public void Drop() {
		transform.parent = null;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Collider>().isTrigger = false;
	}

	virtual public void PickUp() {
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Collider>().isTrigger = true;
	}
}
