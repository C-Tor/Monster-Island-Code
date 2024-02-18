using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
	private NavMeshAgent agent;
	private Monster monster;
	[SerializeField] GameObject targetDestinationObject;
	[SerializeField] float rotateToFaceSpeed;
	[SerializeField] float waterBaseOffset;
	[SerializeField] float landBaseOffset;
	Vector3 targetDestinationPosition;
	private float originalSpeed;

	public bool IsInWater;

	[SerializeField] private LayerMask waterLayer;
	[SerializeField] private LayerMask terrainLayer;


	private void Awake() {
		agent = GetComponent<NavMeshAgent>();
		monster = GetComponent<Monster>();
	}

	private void Start() {
		originalSpeed = agent.speed;
	}

	private void Update() {
		HandleFollowTarget();
		agent.speed = originalSpeed*monster.Scale;

		IsInWater = CheckIsInWater();

		if (IsInWater) {
			agent.baseOffset = waterBaseOffset;
		} else {
			agent.baseOffset = landBaseOffset;
		}
	}

	// Function to check if the character is in a water area type
	public bool CheckIsInWater() {

		Vector3 raycastOrigin = new Vector3(transform.position.x, transform.position.y + monster.GetHeight(), transform.position.z);
		
		RaycastHit hit;

		// Shoot a ray downwards
		if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, monster.GetHeight() + 20f, terrainLayer)) {
			// Check if the hit object is in the water layer

			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")) {
				return true;
			}
		}

		return false;
	}


	private void HandleFollowTarget() {
		if (targetDestinationObject != null) {
			agent.destination = targetDestinationObject.transform.position;

			if (agent.isStopped || !IsMoving()) {
				// Face the object
				FaceTarget();
			}
		}
	}

	private void FixedUpdate() {
		if (targetDestinationObject != null) {
			Collider targetCollider = targetDestinationObject.GetComponent<Collider>();
			if (targetCollider != null && IsCloseEnoughToInteract(targetCollider)) {
				agent.isStopped = true;
			}
		}
	}

	private bool IsCloseEnoughToInteract(Collider targetCollider) {
		// Calculate the distance between the monster and the closest point on the target collider
		float distanceToCollider = Vector3.Distance(transform.position, targetCollider.ClosestPoint(transform.position));

		// Adjust the stopping condition as needed
		return distanceToCollider <= monster.interactRadius - 0.1f;
	}

	public void SetDestinationPosition(Vector3 position) {
		targetDestinationObject = null;
		agent.destination = position;
		agent.isStopped = false;
	}

	public void SetDestinationObject(GameObject targetObject) {
		targetDestinationObject = targetObject;
		if (targetDestinationObject != null) {
			agent.destination = targetDestinationObject.transform.position;
		} else {
			agent.destination = transform.position;
		}
		agent.isStopped = false;
	}

	public bool IsMoving() {
		float minMovementCondition = 0.1f;
		return agent.velocity.magnitude > minMovementCondition;
	}
	
	private void FaceTarget() {
		if (targetDestinationObject != null) {
			Vector3 directionToTarget = (targetDestinationObject.transform.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0f, directionToTarget.z));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateToFaceSpeed);

		}
	}

	//private float CalculateWaterOffset() {
	//}

	public void StopAgent() {
		agent.isStopped = true;
	}

	public void ResumeAgent() {
		agent.isStopped = false;
	}

	public float GetBaseOffset() {
		return agent.baseOffset;
	}

}
