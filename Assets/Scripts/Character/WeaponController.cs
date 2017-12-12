using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour {

    [SerializeField]
    Transform weaponHolder;
    [SerializeField]
    Transform oneHandHolder;
    [SerializeField]
    Transform twoHandsHolder;

    Animator animator;
    static int combatId = Animator.StringToHash("Combat");
    static int kickId = Animator.StringToHash("Kick");
    static int attackAnimIndexId = Animator.StringToHash("AttackAnimIndex");

    MainWeapon mainWeapon = null;
    bool attackDone = true;
    bool lockOn = false;

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
    }

    void FindHolder(Transform holder, string holderName) {
        if (holder == null) {
            holder = transform.Find("holderName");
            if (holder == null) {
                Debug.LogError("Place " + holderName + " to character " + gameObject.name);
            }
        }
    }

	void Update() {
		if (isLocalPlayer) {
			if (InputControl.GetButtonDown ("Fire1") && attackDone) {
				int index = Random.Range(0, 6);
				animator.SetInteger (attackAnimIndexId, index);
				animator.SetBool (combatId, true);
				mainWeapon.Attacking = true;
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
            if (InputControl.GetButtonDown("Equip")) {
                if (!lockOn) {
                    EquipWeapon();
                    animator.SetTrigger("LockOn");
                    SendMessage("LockOn", SendMessageOptions.DontRequireReceiver);
                } else {
                    UnequipWeapon();
                    animator.SetTrigger("LockOff");
                    SendMessage("LockOff", SendMessageOptions.DontRequireReceiver);
                }
                lockOn = !lockOn;
            }
            if (lockOn && InputControl.GetButton("Shift")) {
                animator.SetTrigger("LockOff");
                SendMessage("LockOff", SendMessageOptions.DontRequireReceiver);
                lockOn = false;
            }
		}
	}

    void EquipWeapon() {
        mainWeapon.Equip(weaponHolder);
    }

    void UnequipWeapon() {
        switch (mainWeapon.type) {
            case WeaponType.TwoHands:
                mainWeapon.Unequip(twoHandsHolder);
                break;
            case WeaponType.OneHand:
                mainWeapon.Unequip(oneHandHolder);
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

		if (!isLocalPlayer)
			return;
        animator.SetInteger("WeaponType", (int)mainWeapon.type);
	}
}
