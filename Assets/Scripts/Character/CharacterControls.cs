using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControls : MonoBehaviour {

	[Header("Inputs")]
	[SerializeField]
	private float moveSpeed = 3.0f;
	[SerializeField]
	private float runSpeed = 5.0f;
	[SerializeField]
	private float rotateSpeed = 5.0f;

	private float horizontal;
	private float vertical;

	private GameObject activeModel;
	private Animator anim;
	private Rigidbody body;

	private CameraControls cam;
	private Vector3 moveDir;
	private float moveAmount;


	private void Start() {
		SetupAnimator();
		SetupPhysics();
		cam = CameraControls.singleton;
	}

	private void SetupAnimator() {
		anim = GetComponentInChildren<Animator>();
		if (anim != null) {
			activeModel = anim.gameObject;
			anim.applyRootMotion = false;
		}
	}

	private void SetupPhysics() {
		body = GetComponent<Rigidbody>();
		body.angularDrag = 999;
		body.drag = 4;
		body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}

	private void Update() {
		horizontal = InputControl.GetAxis("Horizontal");
		vertical = InputControl.GetAxis("Vertical");

		Vector3 v = vertical * cam.transform.forward;
		Vector3 h = horizontal * cam.transform.right;
		moveDir = (v + h).normalized;
		float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
		moveAmount = Mathf.Clamp01(m);

		UpdateMovement();
	}

	private void UpdateMovement() {
		body.drag = (moveAmount > Mathf.Epsilon ? 0 : 4);

		bool run = InputControl.GetButtonDown("Shift");
		float targetSpeed = moveSpeed;
		if (run) {
			targetSpeed = runSpeed;
		}

		body.velocity = moveDir * (targetSpeed * moveAmount);

		Vector3 targetDir = moveDir;
		targetDir.y = 0;
		if (targetDir == Vector3.zero) {
			targetDir = transform.forward;
		}
		Quaternion rotation = Quaternion.LookRotation(targetDir);
		Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * moveAmount * rotateSpeed);
		transform.rotation = targetRotation;

		HandleMovementAnimations();
	}

	private void HandleMovementAnimations() {
		anim.SetFloat("Vertical", moveAmount, 0.4f, Time.deltaTime);
	}
}
