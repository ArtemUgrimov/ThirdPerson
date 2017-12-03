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
	}

	private void Update() {
		float h = InputControl.GetAxis("Mouse X");
		float v = InputControl.GetAxis("Mouse Y");

		FollowTarget();
		HandleRotations(v, h);
	}

	private void FollowTarget() {
		Vector3 targetPos = Vector3.Lerp(transform.position, target.position, Time.deltaTime * followSpeed);
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

		if (lockOn) {
			
		}

		LookAngle += smoothX * targetSpeed;
		transform.rotation = Quaternion.Euler(0, LookAngle, 0);
		TiltAngle -= smoothY * targetSpeed;
		TiltAngle = Mathf.Clamp(TiltAngle, minAngle, maxAngle);
		pivot.localRotation = Quaternion.Euler(TiltAngle, 0, 0);
	}
}
