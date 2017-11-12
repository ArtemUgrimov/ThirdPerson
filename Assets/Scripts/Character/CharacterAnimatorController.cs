using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimatorController : NetworkBehaviour {

    Animator animator;

    int horizontalId = Animator.StringToHash("Horizontal");
    int verticalId = Animator.StringToHash("Vertical");
    int mouseXId = Animator.StringToHash("MouseX");
    int horAngle = Animator.StringToHash("HorizontalAngle");
    int runningId = Animator.StringToHash("Running");
    int movingId = Animator.StringToHash("Moving");

    int combatId = Animator.StringToHash("Combat");
    int attackIndexId = Animator.StringToHash("AttackAnimIndex");

    bool isMoving;
    float movingMagnitude;
    int lastAttackIndex = -1;
    float visionAngleDiff;

    void Start()
    {
        animator = GetComponent<Animator>();

        EventManager.StartListening(EventsList.EVENT_ANGLE_CHANGED, AngleChanged);
        EventManager.StartListening(EventsList.CAMERA_MOVED, CameraMoved);
    }

    void OnDestroy()
    {
        EventManager.StopListening(EventsList.EVENT_ANGLE_CHANGED, AngleChanged);
        EventManager.StopListening(EventsList.CAMERA_MOVED, CameraMoved);
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        HandleMovement();
        HandleCombat();
    }

    void HandleCombat()
    {
        if (Input.GetButtonDown("Fire1")) {
            int newIndex = Random.Range(0, 7);
            while (newIndex == lastAttackIndex)
                newIndex = Random.Range(0, 7);
            lastAttackIndex = newIndex;
            animator.SetInteger(attackIndexId, newIndex);
            animator.SetTrigger(combatId);
            if (isMoving)
            {
                isMoving = false;
                animator.SetTrigger(movingId);
            }
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float running = Input.GetAxis("Shift");

		bool dodge = Input.GetButtonDown("Jump");

		movingMagnitude = Mathf.Sqrt(horizontal * horizontal + vertical * vertical);

        if (!isMoving && movingMagnitude > 0.1f)
        {
            isMoving = true;
			animator.ResetTrigger("Dodge");
            animator.SetBool(movingId, isMoving);
        }
        else if (isMoving && movingMagnitude < 0.1f)
        {
            isMoving = false;
			animator.ResetTrigger("Dodge");
            animator.SetBool(runningId, false);
            animator.SetBool(movingId, isMoving);
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
            if (System.Math.Abs(visionAngleDiff) > Mathf.Epsilon) {
                Quaternion targetRot = Quaternion.Euler(0, visionAngleDiff, 0) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 3);
            }
        }

		if (dodge) {
			animator.SetTrigger("Dodge");
		}

        animator.SetFloat(horizontalId, horizontal);
        animator.SetFloat(verticalId, vertical);
        animator.SetFloat(mouseXId, mouseX);
    }

    void AngleChanged(object info)
    {
        Float val = info as Float;
        animator.SetFloat(horAngle, val.value);
        visionAngleDiff = val.value;
    }

    void CameraMoved(object info)
    {
        Float angle = info as Float;
        if (System.Math.Abs(movingMagnitude) > Mathf.Epsilon)
            transform.Rotate(Vector3.up, angle);
    }

    void WeaponChanged(object info)
    {
        WeaponType type = (WeaponType)info;
        switch (type)
        {
            case WeaponType.Axe:
                animator.SetBool("Axe", true);
                animator.SetBool("Sword", false);
                break;
            case WeaponType.Sword_Shield:
                animator.SetBool("Axe", false);
                animator.SetBool("Sword", true);
                break;
            case WeaponType.THSword:
                break;
        }      
    }
}
