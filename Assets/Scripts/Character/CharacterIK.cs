using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterIK : NetworkBehaviour {

//	Animator animator;
//	public Transform lookObject;
//
//	public float offsetY = 0.1f;
//
//	Transform leftFoot;
//	Transform rightFoot;
//
//	Vector3 leftFootPosition;
//	Vector3 rightFootPosition;
//
//	Quaternion leftFootRotation;
//	Quaternion rightFootRotation;
//
//	bool canMove = false;
//	float magnitude = 0.0f;
//	bool IKEnabled = false;
//
//	IEnumerator routine = null;
//
//	private void Start() {
//		animator = GetComponent<Animator>();
//
//		leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
//		rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
//
//		leftFootRotation = leftFoot.rotation;
//		rightFootRotation = rightFoot.rotation;
//	}
//
//	private void Update() {
//		int layerMask = ~(1 << 8);
//		{
//			RaycastHit leftHit;
//			Vector3 lPos = leftFoot.position;
//			if (Physics.Raycast(lPos + Vector3.up * 0.5f, Vector3.down, out leftHit, 1, layerMask)) {
//				leftFootPosition = Vector3.Lerp(lPos, leftHit.point + Vector3.up * offsetY, Time.deltaTime * 10.0f);
//				leftFootRotation = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
//				Debug.DrawLine(leftFoot.position, leftFootPosition, Color.red);
//			}
//		}
//
//		{
//			RaycastHit rightHit;
//			Vector3 rPos = rightFoot.position;
//			if (Physics.Raycast(rPos + Vector3.up * 0.5f, Vector3.down, out rightHit, 1, layerMask)) {
//				rightFootPosition = Vector3.Lerp(rPos, rightHit.point + Vector3.up * offsetY, Time.deltaTime * 10.0f);
//				rightFootRotation = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
//				Debug.DrawLine(rightFoot.position, rightFootPosition, Color.red);
//			}
//		}
//
//		magnitude = animator.GetFloat ("MoveAmount");
//		canMove = animator.GetBool ("CanMove");
//
//		bool canIK = CheckIK ();
//		if (!IKEnabled && canIK && routine == null) {
//			routine = SetIK (1.0f);
//			StartCoroutine (routine);
//		} else if (IKEnabled) {
//			IKEnabled = canIK;
//		}
//	}
//
//	IEnumerator SetIK(float dt) {
//		yield return new WaitForSeconds (dt);
//		routine = null;
//		IKEnabled = CheckIK();
//	}
//
//	bool CheckIK() {
//		return (canMove ? (Mathf.Abs (magnitude) > Mathf.Epsilon ? false : true) : false);
//	}
//
//    void OnAnimatorIK() {
//		if (!IKEnabled)
//			return;
//
//		if (lookObject != null && isLocalPlayer) {
//			animator.SetLookAtWeight(1, 0.7f, 0.9f, 1.0f);
//			animator.SetLookAtPosition(lookObject.position);
//		}
//
//		float weight = 1;
//
//		animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
//		animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPosition);
//
//		animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
//		animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPosition);

//		animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
//		animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
//
//		animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
//		animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
//	}
}
