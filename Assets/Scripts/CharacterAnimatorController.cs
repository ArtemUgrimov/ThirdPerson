using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimatorController : MonoBehaviour {

    Animator animator;

    int horizontalId = Animator.StringToHash("Horizontal");
    int verticalId = Animator.StringToHash("Vertical");
    int mouseXId = Animator.StringToHash("MouseX");
    int horAngle = Animator.StringToHash("HorizontalAngle");
    int runningId = Animator.StringToHash("Running");
    int changeWalkId = Animator.StringToHash("ChangeWalk");

    int combatId = Animator.StringToHash("Combat");
    int attackIndexId = Animator.StringToHash("AttackAnimIndex");

    bool isMoving = false;

    int lastAttackIndex = -1;

    void Start()
    {
        animator = GetComponent<Animator>();

        EventManager.StartListening(EventsList.EVENT_ANGLE_CHANGED, AngleChanged);
        EventManager.StartListening(EventsList.CAMERA_MOVED, CameraMoved);
        EventManager.StartListening(EventsList.ATTACK_END, AttackEnded);
    }

    void Update()
    {
        HandleMovement();
        HandleCombat();
    }

    void HandleCombat()
    {
        if (Input.GetButtonDown("Fire1")) {
            int newIndex = -1;
            while (newIndex == lastAttackIndex)
                newIndex = Random.Range(0, 4);
            lastAttackIndex = newIndex;
            animator.SetInteger(attackIndexId, newIndex);
            animator.SetTrigger(combatId);
            isMoving = false;
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float movingMagnitude = Mathf.Sqrt(horizontal * horizontal + vertical * vertical);
        float mouseX = Input.GetAxis("Mouse X");
        float running = Input.GetAxis("Shift");

        if (!isMoving && movingMagnitude > 0.1f)
        {
            isMoving = true;
            animator.SetTrigger(changeWalkId);
        }
        else if (isMoving && movingMagnitude < 0.1f)
        {
            isMoving = false;
            animator.SetBool(runningId, false);
            animator.SetTrigger(changeWalkId);
        }

        if (isMoving)
        {
            if (System.Math.Abs(running) > Mathf.Epsilon)
            {
                animator.SetBool(runningId, true);
            }
            else
            {
                animator.SetBool(runningId, false);
            }
        }

        animator.SetFloat(horizontalId, horizontal);
        animator.SetFloat(verticalId, vertical);
        animator.SetFloat(mouseXId, mouseX);
    }


    float GetMovingAngle(float horizontal, float vertical, float movingMagnitude) {
        return Mathf.Rad2Deg * Mathf.Atan2(horizontal, vertical);
    }

    public void LeftLegDown(string ev) {
        //leftFoot = false;
        //rightFoot = true;
    }

    public void RightLegDown(string ev) {
        //leftFoot = true;
        //rightFoot = false;
    }

    void AngleChanged(object info) {
        Float val = info as Float;
        animator.SetFloat(horAngle, val.value);
    }

    void CameraMoved(object info) {
        Float angle = info as Float;
        if (isMoving)
            transform.Rotate(Vector3.up, angle);
    }

    void AttackEnded(object info)
    {
        Debug.Log("AttackEnded");
        //animator.SetTrigger();
    }
}
