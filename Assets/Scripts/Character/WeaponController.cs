using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : Lockable {

    [SerializeField]
    Transform weaponHolder;
    [SerializeField]
    Transform oneHandHolder;
    [SerializeField]
    Transform twoHandsHolder;
	[SerializeField]
	Transform secondaryHolder;

    Animator animator;
    static int combatId = Animator.StringToHash("Combat");
    static int kickId = Animator.StringToHash("Kick");
    static int attackAnimIndexId = Animator.StringToHash("AttackAnimIndex");

    MainWeapon mainWeapon = null;
	SecondaryWeapon secondaryWeapon = null;
    bool attackDone = true;
	bool equipping = false;

	IEnumerator kickRoutine = null;
	IEnumerator endCoroutine = null;

	void Awake () {
		animator = GetComponent<Animator> ();
        FindHolders();
	}

    void FindHolders() {
        FindHolder(weaponHolder, "WeaponHolder");
        FindHolder(oneHandHolder, "OneHandHolder");
        FindHolder(twoHandsHolder, "TwoHandsHolder");
		FindHolder(secondaryHolder, "SecondaryHolder");
    }

    void FindHolder(Transform holder, string holderName) {
        if (holder == null) {
            holder = transform.Find(holderName);
            if (holder == null) {
                Debug.LogError("Place " + holderName + " to character " + gameObject.name);
			} else {
				Debug.LogWarning(holderName + " were set automatically. Set it manually. " + gameObject.name);
			}
        }
    }

	void Update() {
		if (isLocalPlayer) {
			if (lockOn && InputControl.GetButtonDown ("Fire1") && attackDone) {
				int index = Random.Range(0, 6);
				animator.SetInteger (attackAnimIndexId, index);
				animator.SetBool (combatId, true);
				mainWeapon.Attacking = true;
				mainWeapon.Attack ();
				attackDone = false;
				if (endCoroutine != null) {
					StopCoroutine (endCoroutine);
				}
				endCoroutine = AttackDone (0.50f);
				StartCoroutine (endCoroutine);
			}
			if (InputControl.GetButtonDown ("Fire2")) {
				if (NeedKick ()) {
					animator.SetBool (kickId, true);
					StartCoroutine (KickDone(0.2f));
				}
			}
			if (!equipping && InputControl.GetButtonDown("Equip")) {
                UpdateLock(!lockOn);
                UpdateEquip();
            }

            animator.SetBool("Locked", lockOn);
		}
	}

    void UpdateEquip() {
        if (!lockOn) {
            StartCoroutine(EquipLater(0.5f, false));
			StartCoroutine(EquipDone(1.5f));
        } else {
            StartCoroutine(EquipLater(0.2f, true));
			StartCoroutine(EquipDone(1.0f));
        }
		equipping = true;
    }

    IEnumerator EquipLater(float dt, bool equip) {
        yield return new WaitForSeconds(dt);
        if (equip) {
            EquipWeapon();
        } else {
            UnequipWeapon();
        }
    }

	IEnumerator EquipDone(float dt) {
		yield return new WaitForSeconds(dt);
		equipping = false;
	}

    void EquipWeapon() {
        mainWeapon.Equip(weaponHolder);
		if (secondaryWeapon != null) {
			secondaryWeapon.Equip(secondaryHolder);
		}
    }

	void UnequipWeapon(bool withSound = true) {
        switch (mainWeapon.type) {
            case WeaponType.TwoHands:
				mainWeapon.Unequip(twoHandsHolder, withSound);
                break;
            case WeaponType.OneHand:
				mainWeapon.Unequip(oneHandHolder, withSound);
				if (secondaryWeapon != null) {
					secondaryWeapon.Unequip(twoHandsHolder, withSound);
				}
                break;
            case WeaponType.None:
                break;
        }
    }

	IEnumerator KickDone(float time) {
		yield return new WaitForSeconds(time);
		animator.SetBool (kickId, false);
	}

	IEnumerator AttackDone(float time) {
		yield return new WaitForSeconds(time);
		animator.SetBool(combatId, false);
		animator.SetBool (kickId, false);
		attackDone = true;
		endCoroutine = null;
		mainWeapon.Attacking = false;
	}

	bool NeedKick() {
		float distance = 1;
		int layer = 0;
		layer |= (1 << LayerMask.NameToLayer ("Enemy"));
		RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.up, transform.forward, out hit, distance, layer)) {
			kickRoutine = Kick (0, hit.collider.gameObject, hit.normal);
			return true;
		}
		return false;
	}

	IEnumerator Kick(float timeout, GameObject go, Vector3 dir) {
		yield return new WaitForSeconds (timeout);
		go.SendMessage ("GotKick", dir * -50.0f);
	}

	void SendEvent(string ev) {
		if (ev == "toStrike") {
			attackDone = true;
			animator.SetBool (combatId, false);
		} else if (ev == "kick") {
			if (kickRoutine != null) {
				StartCoroutine (kickRoutine);
				kickRoutine = null;
			}
		}
	}

	void UpdateWeapon(MainWeapon weapon) {
		mainWeapon = weapon;
        animator.SetInteger("WeaponType", (int)mainWeapon.type);
	}

	void RemoveWeapon() {
		mainWeapon = null;
		animator.SetInteger("WeaponType", 0);
	}

	void UpdateSecondary(SecondaryWeapon secondary) {
		secondaryWeapon = secondary;
	}

	void RemoveSecondary() {
		secondaryWeapon = null;
	}

    void ResetAll() {
        UnequipWeapon(false);
    }
}
