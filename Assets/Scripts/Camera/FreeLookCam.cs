using System;
using UnityEngine;

public class FreeLookCam : PivotBasedCameraRig {
	// This script is designed to be placed on the root object of a camera rig,
	// comprising 3 gameobjects, each parented to the next:

	// 	Camera Rig
	// 		Pivot
	// 			Camera

	[SerializeField] private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
	[Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
	[SerializeField] private float m_TurnSmoothing = 0.0f;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
	[SerializeField] private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
	[SerializeField] private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
	[SerializeField] private bool m_VerticalAutoReturn = false;           // set wether or not the vertical axis should auto return

	private float m_LookAngle;                    // The rig's y axis rotation.
	private float m_TiltAngle;                    // The pivot's x axis rotation.
	private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
	private Vector3 m_PivotEulers;
	private Quaternion m_PivotTargetRot;
	private Quaternion m_TransformTargetRot;

	private Float angle = new Float();
	private Float camMoveAngle = new Float();

	protected override void Awake() {

		base.Awake();
		m_PivotEulers = m_Pivot.rotation.eulerAngles;

		m_PivotTargetRot = m_Pivot.transform.localRotation;
		m_TransformTargetRot = transform.localRotation;

		m_Target = transform.parent;
		transform.parent = null;

		EventManager.StartListening(EventsList.UPDATE_CAMERA_TARGET, UpdateTarget);
	}

	private void OnDestroy() {
		EventManager.StopListening(EventsList.UPDATE_CAMERA_TARGET, UpdateTarget);
	}

	void UpdateTarget(object o) {
		if (o != null) {
			m_Target = (UnityEngine.Transform)o;
		}
	}

	protected void Update() {
		HandleRotationMovement();
	}


	private void OnDisable() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}


	protected override void FollowTarget(float deltaTime) {
		if (m_Target == null) return;
		// Move the rig towards target position.
		transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime * m_MoveSpeed);
	}


	private void HandleRotationMovement() {
		if (Time.timeScale < float.Epsilon || m_Target == null)
			return;

		// Read the user input
		var x = InputControl.GetAxis("Mouse X");
		var y = InputControl.GetAxis("Mouse Y");

		// Adjust the look angle by an amount proportional to the turn speed and horizontal input.
		m_LookAngle += x * m_TurnSpeed;

		// Rotate the rig (the root object) around Y axis only:
		m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

		if (m_VerticalAutoReturn) {
			// For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
			// on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
			// we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
			m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
		} else {
			// on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
			m_TiltAngle -= y * m_TurnSpeed;
			// and make sure the new value is within the tilt range
			m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
		}

		// Tilt input around X is applied to the pivot (the child of this object)
		m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y, m_PivotEulers.z);

		if (m_TurnSmoothing > 0) {
			m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
		} else {
			m_Pivot.localRotation = m_PivotTargetRot;
			transform.localRotation = m_TransformTargetRot;
		}

		// get a "forward vector" for each rotation
		Vector3 forwardA = transform.rotation * Vector3.forward;
		Vector3 forwardB = m_Target.rotation * Vector3.forward;

		// get a numeric angle for each vector, on the X-Z plane (relative to world forward)
		float angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
		float angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;

		// get the signed difference in these angles
		float _signedAngle = Mathf.DeltaAngle(angleA, angleB);
		angle.value = -_signedAngle;

		camMoveAngle.value = x * m_TurnSpeed;
		EventManager.TriggerEvent(EventsList.CAMERA_MOVED, camMoveAngle);
		EventManager.TriggerEvent(EventsList.EVENT_ANGLE_CHANGED, angle);
	}
}

