using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {
	public static CameraControls singleton;

	[SerializeField]
	private float followSpeed = 9;
	[SerializeField]
	private float mouseSensitivity = 2;
	[SerializeField]
	private float cameraSpeed = 2;
	[SerializeField]
	private float controllerSpeed = 7;

	public Transform target;
	private Transform pivot;
	private Transform cam;

	private float turnSmoothing = 0.1f;
	private float minAngle = -60.0f;
	private float maxAngle = 80.0f;

	private float smoothX;
	private float smoothY;
	private float smoothXVelocity;
	private float smoothYVelocity;

	private bool lockOn;

    private Float angle = new Float();
    private Float camMoveAngle = new Float();

	[HideInInspector]
	public float LookAngle {
		get;
		private set;
	}

	[HideInInspector]
	public float TiltAngle {
		get;
		private set;
	}

	private void Awake() {
		singleton = this;

		pivot = transform.GetChild(0);
		cam = pivot.GetChild(0);
        transform.parent = null;
	}

	private void FixedUpdate() {
		float h = InputControl.GetAxis("Mouse X");
		float v = InputControl.GetAxis("Mouse Y");

		FollowTarget();
		HandleRotations(v, h);
	}

	private void FollowTarget() {
		Vector3 targetPos = Vector3.Lerp(transform.position, target.position, Time.fixedDeltaTime * followSpeed);
		transform.position = targetPos;
	}

	private void HandleRotations(float vertical, float horizontal) {
		float targetSpeed = mouseSensitivity;
		if (turnSmoothing > 0) {
			smoothX = Mathf.SmoothDamp(smoothX, horizontal, ref smoothXVelocity, turnSmoothing);
			smoothY = Mathf.SmoothDamp(smoothY, vertical, ref smoothYVelocity, turnSmoothing);
		} else {
			smoothX = horizontal;
			smoothY = vertical;
		}

        // get a "forward vector" for each rotation
        Vector3 forwardA = transform.localRotation * Vector3.forward;
        Vector3 forwardB = target.rotation * Vector3.forward;

        // get a numeric angle for each vector, on the X-Z plane (relative to world forward)
        float angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;

        // get the signed difference in these angles
        float _signedAngle = Mathf.DeltaAngle(angleA, angleB);
        angle.value = -_signedAngle;

        camMoveAngle.value = smoothX * mouseSensitivity;
        EventManager.TriggerEvent(EventsList.CAMERA_MOVED, camMoveAngle);
        EventManager.TriggerEvent(EventsList.EVENT_ANGLE_CHANGED, angle);

		LookAngle += smoothX * targetSpeed;
		transform.rotation = Quaternion.Euler(0, LookAngle, 0);
		TiltAngle -= smoothY * targetSpeed;
		TiltAngle = Mathf.Clamp(TiltAngle, minAngle, maxAngle);
		pivot.localRotation = Quaternion.Euler(TiltAngle, 0, 0);
	}
}
