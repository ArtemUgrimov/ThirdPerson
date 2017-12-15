using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControls : Lockable {

	[Header("Inputs")]
	[SerializeField]
	private float moveSpeed = 3.0f;
	[SerializeField]
	private float crouchSpeed = 1.5f;
	[SerializeField]
	private float runSpeed = 5.0f;
	[SerializeField]
	private float rotateSpeed = 5.0f;
    [SerializeField]
    private float lockonCoeff = 0.6f;

    int horizontalId = Animator.StringToHash("Horizontal");
    int verticalId = Animator.StringToHash("Vertical");

	private float horizontal;
	private float vertical;
	private bool run;
	private bool crouch;
    private bool leftMouse;
    private bool rightMouse;

	private GameObject activeModel;
	private Animator anim;
	private Rigidbody body;

	private CameraControls cam;
	private Vector3 moveDir;
	private int ignoreLayers = ~(1 << 8);

    #region properties
    public float MoveAmount {
        get;
        private set;
    }

	public bool Grounded {
		get;
		private set;
	}

    public bool CanMove {
		get;
		private set;
    }

	public bool Crouch {
		get {
			if (lockOn || run) {
				return false;
			}
			return crouch;
		}
		private set {
			crouch = value;
		}
	}
    #endregion

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
		run = InputControl.GetButton ("Shift");
        leftMouse = InputControl.GetButtonDown("Fire1");
        rightMouse = InputControl.GetButtonDown("Fire2");
		run = InputControl.GetButton("Shift");
		Crouch = InputControl.GetButton ("Crouch");
	}

	private void FixedUpdate() {
		UpdateMovement();
		HandleMovementAnimations();
		UpdateAnimator();
	}

	private void UpdateMovement() {
		Vector3 v = vertical * cam.transform.forward;
		Vector3 h = horizontal * cam.transform.right;
		moveDir = (v + h).normalized;
		MoveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
		Grounded = IsGrounded ();

		body.drag = (MoveAmount > Mathf.Epsilon || !Grounded ? 0 : 4);

        CanMove = anim.GetBool("CanMove");

        float targetSpeed = moveSpeed * (lockOn ? lockonCoeff : 1);
		if (run) {
			targetSpeed = runSpeed * (lockOn ? lockonCoeff : 1);
			Crouch = false;
		} else if (Crouch) {
			targetSpeed = crouchSpeed;
			UpdateLock (false);
		}

        if (Grounded && CanMove) {
            float y = body.velocity.y;
            Vector3 move = moveDir * (targetSpeed * MoveAmount);
            move.y = y;
            body.velocity = move;
            anim.SetFloat(horizontalId, horizontal);
            anim.SetFloat(verticalId, vertical);
		}

        if (!lockOn && CanMove) {
            Vector3 targetDir = moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero) {
                targetDir = transform.forward;
            }
            Quaternion rotation = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * MoveAmount * rotateSpeed);
            transform.rotation = targetRotation;
        }

        if (!CanMove) {
            anim.applyRootMotion = true;
        } else {
            anim.applyRootMotion = false;
        }
	}


	private bool IsGrounded() {
		Vector3 origin = transform.position + Vector3.up;
		Vector3 dir = Vector3.down;
		float distance = 1.01f;
		RaycastHit hit;
		if (Physics.Raycast (origin, dir, out hit, distance, ignoreLayers)) {
			return true;
		}
		return false;
	}

	private void HandleMovementAnimations() {
		anim.SetFloat("MoveAmount", MoveAmount);//, 0.1f, Time.fixedDeltaTime);
	}

	private void UpdateAnimator() {
		anim.SetBool("onGround", Grounded);
	}
}
