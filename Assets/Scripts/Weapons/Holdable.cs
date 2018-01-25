using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Holdable : MonoBehaviour
{
	[Header("Holdable Attributes")]
	[SerializeField]
	protected PositionRotation transformActive;
	[SerializeField]
	protected PositionRotation transformInactive;

	[Header("Sounds")]
	[SerializeField]
	private List<AudioClip> equipSounds = new List<AudioClip> ();
	[SerializeField]
	private List<AudioClip> unequipSounds = new List<AudioClip> ();

	protected GameObject superParent = null;

	virtual public void Equip(Transform holder, bool withSound = true) {
		transform.parent = holder;
		transform.localPosition = transformActive.position;
		transform.localRotation = Quaternion.Euler(transformActive.rotation);

		Transform sParent = Utils.GetSuperParent (transform);
		superParent = sParent.gameObject;

		if (withSound && equipSounds.Count > 0) {
			AudioSource source = superParent.GetComponent<AudioSource> ();
			source.PlayOneShot (equipSounds [Random.Range (0, equipSounds.Count)]);
		}
	}

	virtual public void Unequip(Transform holder, bool withSound = true) {
		if (withSound && unequipSounds.Count > 0) {
			AudioSource source = superParent.GetComponent<AudioSource> ();
			source.PlayOneShot (unequipSounds [Random.Range (0, unequipSounds.Count)]);
		}

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
