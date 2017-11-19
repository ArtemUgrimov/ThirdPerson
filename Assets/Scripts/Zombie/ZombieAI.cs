﻿using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ZombieAnimatorController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class ZombieAI : NetworkBehaviour {

	enum State {
		Idle,
		Walking,
		Screaming,
		Attacking,
		Running,
		Hit,
		Dead
	}

	[SyncVar]
	State state;

	[SerializeField]
	float attackDistance = 0.2f;

	[SyncVar]
	Transform target;
	Float angle = new Float();

	double currentStateTime = 0;

	void Start() {
		state = State.Idle;

		Utils.SetRagdoll(false, gameObject);
	}

	void Update() {
		if (state == State.Dead)
			return;

		if (target != null) {
			Vector3 direction = target.position - transform.position;
			direction.y = 0;
			Quaternion toRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 360);

			angle.value = toRotation.eulerAngles.y;
			SendMessage("MoveDir", angle);
		}

		currentStateTime += Time.deltaTime;

		switch (state) {
			case State.Idle:
				if (currentStateTime >= 5) {
					switchState(State.Walking);
					SendMessage("Walk");
				}
				break;
			case State.Walking:
				if (currentStateTime >= 5) {
					switchState(State.Idle);
					SendMessage("Idle");
				} else {
					updateMoveDirection();
				}
				break;
			case State.Screaming:
				if (currentStateTime >= 1) {
					runToTarget();
				}
				break;
			case State.Running:
				tryAttackTarget();
				break;
			case State.Attacking:
				attackOrChase();
				break;
			case State.Hit:
				if (currentStateTime >= 0.5f) {
					attackOrChase();
				}
				break;
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (state == State.Dead)
			return;
		if (other.gameObject.tag == "Player") {
			target = other.gameObject.transform;
			tryAttackOrChaseTarget();
		}
	}

	public void OnTriggerExit(Collider other) {
		if (state == State.Dead)
			return;
		if (other.gameObject.tag == "Player") {
			target = null;
			SendMessage("Walk");
			switchState(State.Walking);
		}
	}

	float getTargetDistance() {
		if (target != null) {
			return Vector3.Distance(target.position, transform.position);
		}
		return float.MaxValue;
	}

	void switchState(State to) {
		state = to;
		currentStateTime = 0;
	}

	void tryAttackOrChaseTarget() {
		if (getTargetDistance() <= attackDistance) {
			SendMessage("Attack");
			switchState(State.Attacking);
		} else {
			SendMessage("Scream");
			switchState(State.Screaming);
		}
	}

	void runToTarget() {
		if (target) {
			if (getTargetDistance() <= attackDistance) {
				SendMessage("Attack");
				switchState(State.Attacking);
			} else {
				SendMessage("Run");
				switchState(State.Running);
			}
		}
	}

	void updateMoveDirection() {
		if (Random.Range(1, 101) == 1) {
			Vector3 direction = new Vector3(Random.Range(-1000, 1000), 0, Random.Range(-1000, 1000));
			Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);

			angle.value = toRotation.eulerAngles.y;
			SendMessage("MoveDir", angle);
		}
	}

	void tryAttackTarget() {
		if (getTargetDistance() <= attackDistance) {
			SendMessage("Attack");
			switchState(State.Attacking);
		}
	}

	void attackOrChase() {
		if (getTargetDistance() <= attackDistance) {
			SendMessage("Attack");
			switchState(State.Attacking);
		} else {
			SendMessage("Run");
			switchState(State.Running);
		}
	}

	void IAmDead() {
		if (state == State.Dead)
			return;
		RpcKillZombie();
	}

	//[ClientRpc]
	void RpcKillZombie() {
		switchState(State.Dead);
		SendMessage("Death");
		GetComponent<ZombieAnimatorController>().enabled = false;
		GetComponent<CapsuleCollider>().enabled = false;
		Utils.SetRagdoll(true, gameObject);
	}

	void GotHit(int amount) {
		if (state == State.Dead)
			return;
		switchState(State.Hit);
		SendMessage("Hit");
	}
}
