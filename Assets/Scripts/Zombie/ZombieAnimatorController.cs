using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimatorController : MonoBehaviour {

    Animator animator;
    Quaternion moveDir = Quaternion.identity;

    int idle_id = Animator.StringToHash("Idle");
    int attack_id = Animator.StringToHash("Attack");
    int walk_id = Animator.StringToHash("Walk");
    int run_id = Animator.StringToHash("Run");
    int scream_id = Animator.StringToHash("Scream");
    int death_id = Animator.StringToHash("Death");
    int got_hit_id = Animator.StringToHash("GotHit");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, moveDir, Time.deltaTime * 5.0f);
    }

    void Idle()
    {
        animator.SetBool(idle_id, true);
        animator.SetBool(attack_id, false);
        animator.SetBool(walk_id, false);
        animator.SetBool(run_id, false);
        animator.SetBool(scream_id, false);
        animator.SetBool(got_hit_id, false);
    }

    void Walk()
    {
        animator.SetBool(idle_id, false);
        animator.SetBool(attack_id, false);
        animator.SetBool(walk_id, true);
        animator.SetBool(run_id, false);
        animator.SetBool(scream_id, false);
        animator.SetBool(got_hit_id, false);
    }

    void Run()
    {
        animator.SetBool(idle_id, false);
        animator.SetBool(attack_id, false);
        animator.SetBool(walk_id, false);
        animator.SetBool(run_id, true);
        animator.SetBool(scream_id, false);
        animator.SetBool(death_id, false);
        animator.SetBool(got_hit_id, false);
    }

    void Attack()
    {
        animator.SetBool(idle_id, false);
        animator.SetBool(attack_id, true);
        animator.SetBool(walk_id, false);
        animator.SetBool(run_id, false);
        animator.SetBool(scream_id, false);
        animator.SetBool(got_hit_id, false);
    }

    void MoveDir(object param)
    {
        moveDir = Quaternion.Euler(new Vector3(0.0f, param as Float, 0.0f));
    }

    void Scream()
    {
        animator.SetBool(idle_id, false);
        animator.SetBool(attack_id, false);
        animator.SetBool(walk_id, false);
        animator.SetBool(run_id, false);
        animator.SetBool(scream_id, true);
        animator.SetBool(got_hit_id, false);
    }

    void Death()
    {
        animator.SetBool(idle_id, false);
        animator.SetBool(attack_id, false);
        animator.SetBool(walk_id, false);
        animator.SetBool(run_id, false);
        animator.SetBool(scream_id, false);
        animator.SetBool(got_hit_id, false);
        animator.SetBool(death_id, true);
    }

    void Hit()
    {
        animator.SetBool(idle_id, false);
        animator.SetBool(attack_id, false);
        animator.SetBool(walk_id, false);
        animator.SetBool(run_id, false);
        animator.SetBool(scream_id, false);
        animator.SetBool(got_hit_id, true);
    }

}
