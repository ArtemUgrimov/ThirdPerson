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

	private int ignoreLayers = ~(1 << 8);

	public bool Grounded {
		get;
		private set;
	}

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
	}

	private void FixedUpdate() {
		UpdateMovement();
		HandleMovementAnimations();
	}

	private void UpdateMovement() {
		Vector3 v = vertical * cam.transform.forward;
		Vector3 h = horizontal * cam.transform.right;
		moveDir = (v + h).normalized;
		moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
		Grounded = IsGrounded ();

		body.drag = (moveAmount > Mathf.Epsilon || !Grounded ? 0 : 4);

		bool run = InputControl.GetButtonDown("Shift");
		float targetSpeed = moveSpeed;
		if (run) {
			targetSpeed = runSpeed;
		}

		if (Grounded) {
			body.velocity = moveDir * (targetSpeed * moveAmount);
		}

		Vector3 targetDir = moveDir;
		targetDir.y = 0;
		if (targetDir == Vector3.zero) {
			targetDir = transform.forward;
		}
		Quaternion rotation = Quaternion.LookRotation(targetDir);
		Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * moveAmount * rotateSpeed);
		transform.rotation = targetRotation;
	}


	private bool IsGrounded() {
		Vector3 origin = transform.position + Vector3.up;
		Vector3 dir = Vector3.down;
		float distance = 1.0f;
		RaycastHit hit;
		if (Physics.Raycast (origin, dir, out hit, distance, ignoreLayers)) {
			return true;
		}
		return false;
	}

	private void HandleMovementAnimations() {
		anim.SetFloat("Vertical", moveAmount, 0.4f, Time.fixedDeltaTime);
	}
}
