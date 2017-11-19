using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimatorController : NetworkBehaviour {

	Animator animator;
	NetworkAnimator netAnimator;

	int horizontalId = Animator.StringToHash("Horizontal");
	int verticalId = Animator.StringToHash("Vertical");
	int mouseXId = Animator.StringToHash("MouseX");
	int horAngle = Animator.StringToHash("HorizontalAngle");
	int runningId = Animator.StringToHash("Running");
	int movingId = Animator.StringToHash("Moving");
	int dodgeId = Animator.StringToHash("Dodge");
	int dodgeFeatureId = Animator.StringToHash("Dodge_feature");

	bool isMoving;
	float movingMagnitude;
	float visionAngleDiff;

	IEnumerator dodgeCoroutine = null;

	void Start() {
		animator = GetComponent<Animator>();
		netAnimator = GetComponent<NetworkAnimator>();

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
		EventManager.TriggerEvent(EventsList.UPDATE_CAMERA_TARGET, transform);
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

		movingMagnitude = Mathf.Sqrt(horizontal * horizontal + vertical * vertical);
		animator.ResetTrigger(dodgeId);

		if (!isMoving && movingMagnitude > 0.1f) {
			isMoving = true;
			animator.SetBool (movingId, isMoving);
		} else if (isMoving && movingMagnitude < 0.1f) {
			isMoving = false;
			animator.SetBool (runningId, false);
			animator.SetBool (movingId, isMoving);
			animator.SetBool (dodgeFeatureId, false);
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
				netAnimator.SetTrigger(dodgeId);
				animator.SetBool (dodgeFeatureId, true);
				dodgeCoroutine = ResetDodge ();
				StartCoroutine (dodgeCoroutine);
			}
		}

		animator.SetFloat(horizontalId, horizontal);
		animator.SetFloat(verticalId, vertical);
		animator.SetFloat(mouseXId, mouseX);
	}

	void AngleChanged(object info) {
		if (!isLocalPlayer)
			return;
		Float val = info as Float;
		animator.SetFloat(horAngle, val.value);
		visionAngleDiff = val.value;
	}

	void CameraMoved(object info) {
		if (!isLocalPlayer)
			return;
		Float angle = info as Float;
		if (System.Math.Abs(movingMagnitude) > Mathf.Epsilon)
			transform.Rotate(Vector3.up, angle);
	}

	void IAmDead() {
		netAnimator.SetTrigger("Dead");
		Utils.SetRagdoll(true, gameObject, "Weapon");
	}

	void Respawn() {
		netAnimator.SetTrigger("Respawn");
		Utils.SetRagdoll(false, gameObject, "Weapon");
	}

	IEnumerator ResetDodge() {
		yield return new WaitForSeconds (1.0f);
		animator.SetBool (dodgeFeatureId, false);
	}


}
