using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(ZombieAnimatorController))]
[RequireComponent(typeof(CapsuleCollider))]
public class Zombie : NetworkBehaviour
{
	enum State {
		Idle,
		Walking,
		Screaming,
		Attacking,
		Running,
		Hit,
		Dead,
		Stun
	}

	[SerializeField]
	int maxHealth = 100;
	[SerializeField]
	float speed = 3.0f;
	[SerializeField]
	float rotationSpeed = 12.0f;

	[Tooltip("Time between zombie got hit and can move")]
	[SerializeField]
	float hitPause = 1.0f;
	[SerializeField]
	float attackDistance = 1.2f;

	[Tooltip("Coeff to Increace/Decreace trigger collider size")]
	[SerializeField]
	float chaseAreaCoeff = 3.0f;
	[SerializeField]
	GameObject bloodPrefab;

	MainWeapon mainWeapon;
	Transform target;
	NavMeshAgent agent;
	ZombieAnimatorController animatorController;
	BoxCollider trigger;

	[SyncVar]
	int currentHealth;
	[SyncVar]
	bool dead;
	[SyncVar]
	State state = State.Idle;

	void Awake() {
		currentHealth = maxHealth;
		animatorController = GetComponent<ZombieAnimatorController> ();
		trigger = GetComponent<BoxCollider> ();
		SetupAgent ();
	}

	void SetupAgent() {
		agent = GetComponent<NavMeshAgent> ();
		agent.stoppingDistance = attackDistance;
		agent.speed = speed;
		agent.angularSpeed = rotationSpeed * 10;

		CapsuleCollider capsule = GetComponent<CapsuleCollider> ();
		agent.height = capsule.height;
		agent.radius = capsule.radius;
	}

	void Update() {
		if (state == State.Dead)
			return;

		CheckDestinationReached ();
		CheckAttack ();
	}

	void CheckDestinationReached() {
		if (target != null && !Disabled()) {
			float distanceToTarget = Vector3.SqrMagnitude (transform.position - target.position);
			if (distanceToTarget < attackDistance * attackDistance) {
				animatorController.Attack ();
				switchState (State.Attacking);
			} else {
				animatorController.Run ();
				switchState (State.Running);
			}
			Vector3 direction = target.position - transform.position;
			direction.y = 0;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * rotationSpeed);
		}
	}

	void CheckAttack() {
		bool attacking = animatorController.animator.GetBool ("Attacking");
		mainWeapon.Attacking = attacking;
	}

	void UpdateWeapon(MainWeapon weapon) {
		mainWeapon = weapon;
	}

	public void OnTriggerEnter(Collider other) {
		if (Disabled ())
			return;
		if (target == null && other.gameObject.tag == "Player") {
			target = other.gameObject.transform;

			trigger.size *= chaseAreaCoeff;

			agent.SetDestination (target.position);
//			agent.isStopped = false;
			tryAttackOrChaseTarget();
		}
	}

	public void OnTriggerStay(Collider other) {
		if (Disabled ())
			return;

		if (target == null && other.gameObject.tag == "Player") {
			target = other.gameObject.transform;
			agent.SetDestination (target.position);
//			agent.isStopped = false;
			tryAttackOrChaseTarget();
		}
		if (target != null) {
			agent.SetDestination (target.position);
		}
	}

	public void OnTriggerExit(Collider other) {
		if (state == State.Dead)
			return;
		if (other.gameObject.tag == "Player") {
			target = null;

			trigger.size /= chaseAreaCoeff;

//			agent.isStopped = true;
			animatorController.Idle ();
			switchState(State.Idle);
		}
	}

	public bool Disabled() {
		return state == State.Dead || state == State.Hit || state == State.Stun;
	}

	float getTargetDistanceSqr() {
		if (target != null) {
			return Vector3.SqrMagnitude (transform.position - target.position);
		}
		return float.MaxValue;
	}

	float getTargetDistance() {
		if (target != null) {
			return Vector3.Distance(target.position, transform.position);
		}
		return float.MaxValue;
	}

	void tryAttackOrChaseTarget() {
		if (getTargetDistanceSqr () <= attackDistance * attackDistance) {
			animatorController.Attack ();
			switchState (State.Attacking);
		} else if (target != null) {
			animatorController.Run ();
			switchState (State.Running);
		} else {
			animatorController.Idle ();
			switchState (State.Idle);
		}
	}

	void switchState(State to) {
		state = to;
	}

	void IAmDead() {
		if (state == State.Dead)
			return;
		KillZombie();
	}

	void KillZombie() {
		switchState(State.Dead);
		Utils.SetRagdoll(true, gameObject);
		animatorController.Death ();
		GetComponent<BoxCollider>().enabled = false;
		GetComponent<ZombieAnimatorController>().enabled = false;
		GetComponent<CapsuleCollider>().enabled = false;
		GetComponent<RagdollHelper> ().enabled = false;
		GetComponent<NetworkAnimator> ().enabled = false;
		this.enabled = false;
		agent.enabled = false;
	}

	void GotKick(Vector3 worldDirection) {
		if (state == State.Dead)
			return;
		
		GetComponent<CapsuleCollider>().enabled = false;
		Utils.SetRagdoll(true, gameObject);

		GetComponent<RagdollHelper> ().ragdolled = true;
		Animator anim = GetComponent<Animator> ();
		anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>().AddForce (worldDirection, ForceMode.Impulse);

		StartCoroutine (WakeUp (2.0f));
		state = State.Stun;
	}

	IEnumerator WakeUp(float delay) {
		yield return new WaitForSeconds (delay);
		if (state != State.Dead) {
			Debug.Log ("Woke UP!");
			GetComponent<CapsuleCollider> ().enabled = true;
			Utils.SetRagdoll (false, gameObject);
			GetComponent<RagdollHelper> ().ragdolled = false;
		}
	}

	void GotHit(HitInfo info) {
		if (state == State.Dead)
			return;

		if (bloodPrefab != null) {
			GameObject blood = (GameObject)Instantiate (bloodPrefab, info.transform.position, info.transform.rotation);
			StartCoroutine (RemoveBlood (1.0f, blood));
		}

		currentHealth -= info.damage;
		if (currentHealth <= 0) {
			IAmDead ();
		} else {
			switchState(State.Hit);
			animatorController.Hit ();
			Invoke ("tryAttackOrChaseTarget", hitPause);
		}
	}

	IEnumerator RemoveBlood(float dt, GameObject go) {
		yield return new WaitForSeconds (dt);
		Destroy (go);
	}
}
