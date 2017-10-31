using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovementController : MonoBehaviour {

    Animator animator;
    Quaternion moveDir = Quaternion.identity;

    void Start()
    {
        animator = GetComponent<Animator>();

        /*
            public static string AGGRO = "aggro";
            public static string WALK = "walk";
            public static string RUN = "run";
            public static string ATTACK = "attack";
            public static string MOVE_DIR = "move_dir";
            public static string STOP_WALK = "stop_walk";
         */
        EventManager.StartListening(ZombieEvents.AGGRO, Aggressive);
        EventManager.StartListening(ZombieEvents.WALK, Walk);
        EventManager.StartListening(ZombieEvents.ATTACK, Attack);
        EventManager.StartListening(ZombieEvents.MOVE_DIR, UpdateWalkDir);
        EventManager.StartListening(ZombieEvents.STOP_WALK, Stop);
        EventManager.StartListening(ZombieEvents.UPDATE_TARGET_DISTANCE, UpdateTargetDistance);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(ZombieEvents.AGGRO, Aggressive);
        EventManager.StopListening(ZombieEvents.WALK, Walk);
        EventManager.StopListening(ZombieEvents.ATTACK, Attack);
        EventManager.StopListening(ZombieEvents.MOVE_DIR, UpdateWalkDir);
        EventManager.StopListening(ZombieEvents.STOP_WALK, Stop);
        EventManager.StopListening(ZombieEvents.UPDATE_TARGET_DISTANCE, UpdateTargetDistance);
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, moveDir, Time.deltaTime * 20.0f);
    }

    void UpdateWalkDir(object direction)
    {
        moveDir = Quaternion.Euler(new Vector3(0.0f, direction as Float, 0.0f));
    }

    void UpdateTargetDistance(object direction)
    {
        animator.SetFloat("TargetDistance", direction as Float);
    }

    void Walk(object direction)
    {
        moveDir = Quaternion.Euler(new Vector3(0.0f, direction as Float, 0.0f));
        animator.SetBool("Walking", true);
    }

    void Stop(object temp)
    {
        animator.SetBool("Walking", false);
    }

    void Attack(object temp)
    {
        animator.SetTrigger("Attack");
    }

    void Aggressive(object temp)
    {
        animator.SetBool("Aggressive", temp != null);
    }

    void Die(object temp)
    {
        animator.SetInteger("DeathType", Random.Range(0, 2));
        animator.SetTrigger("Death");
    }
}
