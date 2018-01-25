using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInfo {
	public PosRot transform = new PosRot();
	public int damage;
}

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

public class MainWeapon : Holdable {

	[SerializeField]
	private List<AudioClip> attackSounds = new List<AudioClip> ();
	[SerializeField]
	private List<AudioClip> hitSounds = new List<AudioClip> ();

	[SerializeField]
	private int damage = 25;

	public WeaponType type;

	private bool attacking = false;
	public bool Attacking {
		get { return attacking; }
		set { attacking = value; }
	}

	public void Attack() {
		if (superParent != null && attackSounds.Count > 0) {
			AudioSource source = superParent.GetComponent<AudioSource> ();
			source.PlayOneShot (attackSounds [Random.Range (0, attackSounds.Count)]);
		}
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

			Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

			HitInfo info = new HitInfo();
			info.damage = damage;
			info.transform.position = pos;

			other.gameObject.SendMessage ("GotHit", info, SendMessageOptions.DontRequireReceiver);

			if (superParent != null && hitSounds.Count > 0) {
				AudioSource source = superParent.GetComponent<AudioSource> ();
				source.PlayOneShot (hitSounds [Random.Range (0, hitSounds.Count)]);
			}
		}
	}
}
