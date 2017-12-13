using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterControls))]
public class CharacterAnimatorController : Lockable {

	Animator animator;
	NetworkAnimator netAnimator;
    CharacterControls controls;

	int mouseXId = Animator.StringToHash("MouseX");
	int horAngle = Animator.StringToHash("CameraAngle");
	int runningId = Animator.StringToHash("Running");
	int movingId = Animator.StringToHash("Moving");
	int dodgeFeatureId = Animator.StringToHash("Dodge_feature");

	bool isMoving;
	float movingMagnitude;
	float visionAngleDiff;

	IEnumerator dodgeCoroutine = null;

	void Start() {
		animator = GetComponent<Animator>();
		netAnimator = GetComponent<NetworkAnimator>();
        controls = GetComponent<CharacterControls>();

		for (int i = 0; i < animator.parameterCount; ++i) {
			netAnimator.SetParameterAutoSend(i, true);
		}

        EventManager.StartListening(EventsList.EVENT_ANGLE_CHANGED, AngleChanged);
        EventManager.StartListening(EventsList.CAMERA_MOVED, CameraMoved);
	}

    void OnDestroy() {
        EventManager.StopListening(EventsList.EVENT_ANGLE_CHANGED, AngleChanged);
        EventManager.StopListening(EventsList.CAMERA_MOVED, CameraMoved);
    }

	public override void OnStartLocalPlayer() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		HandleMovement();
	}

	void HandleMovement() {
		float horizontal = InputControl.GetAxis("Horizontal");
		float vertical = InputControl.GetAxis("Vertical");
		float mouseX = InputControl.GetAxis("Mouse X");
		float running = InputControl.GetAxis("Shift");

        movingMagnitude = controls.MoveAmount;

		if (!isMoving && movingMagnitude > 0.1f) {
			isMoving = true;
		} else if (isMoving && movingMagnitude < 0.1f) {
			isMoving = false;
			animator.SetBool (runningId, false);
		}

		if (isMoving) {
			if (System.Math.Abs(running) > Mathf.Epsilon) {
				animator.SetBool(runningId, true);
			} else {
				animator.SetBool(runningId, false);
			}
			if (System.Math.Abs(visionAngleDiff) > Mathf.Epsilon) {
				Quaternion targetRot = Quaternion.Euler(0, visionAngleDiff, 0) * transform.rotation;
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 3);
			}
			bool dodge = InputControl.GetButtonDown("Jump");
			if (dodge) {
				animator.SetBool (dodgeFeatureId, true);
				dodgeCoroutine = ResetDodge ();
				StartCoroutine (dodgeCoroutine);
			}
		}
	}

	void AngleChanged(object info) {
        if (!isLocalPlayer || !lockOn)
			return;
		Float val = info as Float;
        //This if for turning inplace with rootmotion
//		animator.SetFloat(horAngle, val.value);
		visionAngleDiff = val.value;
	}

	void CameraMoved(object info) {
        if (!isLocalPlayer || !lockOn)
			return;
		Float angle = info as Float;
        if (System.Math.Abs(movingMagnitude) > Mathf.Epsilon)
            transform.Rotate(Vector3.up, angle);
	}

    protected override void LockOff() {
        visionAngleDiff = 0;
        lockOn = false;
    }

	void IAmDead() {
		netAnimator.SetTrigger("Dead");
		Utils.SetRagdoll(true, gameObject);
	}

	void Respawn() {
		netAnimator.SetTrigger("Respawn");
		Utils.SetRagdoll(false, gameObject);
        UpdateLock(false);
        SendMessage("ResetAll", SendMessageOptions.DontRequireReceiver);
	}

	IEnumerator ResetDodge() {
		yield return new WaitForSeconds (0.5f);
		animator.SetBool (dodgeFeatureId, false);
	}
}
