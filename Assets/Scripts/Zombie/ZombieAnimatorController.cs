using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimatorController : MonoBehaviour {

	public Animator animator;

	int idle_id = Animator.StringToHash("Idle");
	int attack_id = Animator.StringToHash("Attack");
	int walk_id = Animator.StringToHash("Walk");
	int run_id = Animator.StringToHash("Run");
	int scream_id = Animator.StringToHash("Scream");
	int death_id = Animator.StringToHash("Death");
	int got_hit_id = Animator.StringToHash("GotHit");

	void Start() {
		animator = GetComponent<Animator>();
	}

	void OnDestroy() {
	}

	public void Idle() {
		animator.SetBool(idle_id, true);
		animator.SetBool(attack_id, false);
		animator.SetBool(walk_id, false);
		animator.SetBool(run_id, false);
		animator.SetBool(scream_id, false);
		animator.SetBool(got_hit_id, false);
	}

	public void Walk() {
		animator.SetBool(idle_id, false);
		animator.SetBool(attack_id, false);
		animator.SetBool(walk_id, true);
		animator.SetBool(run_id, false);
		animator.SetBool(scream_id, false);
		animator.SetBool(got_hit_id, false);
	}

	public void Run() {
		animator.SetBool(idle_id, false);
		animator.SetBool(attack_id, false);
		animator.SetBool(walk_id, false);
		animator.SetBool(run_id, true);
		animator.SetBool(scream_id, false);
		animator.SetBool(death_id, false);
		animator.SetBool(got_hit_id, false);
	}

	public void Attack() {
		animator.SetBool(idle_id, false);
		animator.SetBool(attack_id, true);
		animator.SetBool(walk_id, false);
		animator.SetBool(run_id, false);
		animator.SetBool(scream_id, false);
		animator.SetBool(got_hit_id, false);
	}

	public void Scream() {
		animator.SetBool(idle_id, false);
		animator.SetBool(attack_id, false);
		animator.SetBool(walk_id, false);
		animator.SetBool(run_id, false);
		animator.SetBool(scream_id, true);
		animator.SetBool(got_hit_id, false);
	}

	public void Death() {
		animator.SetBool(idle_id, false);
		animator.SetBool(attack_id, false);
		animator.SetBool(walk_id, false);
		animator.SetBool(run_id, false);
		animator.SetBool(scream_id, false);
		animator.SetBool(got_hit_id, false);
		animator.SetBool(death_id, true);
		Invoke("Reset", 0.01f);
	}

	public void Hit() {
		animator.SetBool(idle_id, false);
		animator.SetBool(attack_id, false);
		animator.SetBool(walk_id, false);
		animator.SetBool(run_id, false);
		animator.SetBool(scream_id, false);
		animator.SetBool(got_hit_id, true);
	}

	public void Reset() {
		animator.SetBool(idle_id, false);
		animator.SetBool(attack_id, false);
		animator.SetBool(walk_id, false);
		animator.SetBool(run_id, false);
		animator.SetBool(scream_id, false);
		animator.SetBool(got_hit_id, false);
		animator.SetBool(death_id, false);
	}
}
