using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
	public bool isPickedUp = false;
	public bool canPickUp;
	[SerializeField] private Vector3 offset;
	[SerializeField] private float smoothFactor = 10;
	[SerializeField] private Vector3 customOffset;
	private Bounds objectBounds;

	private Rigidbody rb;

	public event Action OnPickUp;
	public event Action OnDrop;



	private void Start() {

		offset = CalculateOffset() + customOffset;
		rb = GetComponent<Rigidbody>();
		if (rb == null) {
			Debug.LogError("Interactive Object does not have a RigidBody");
		}
		canPickUp = true;
	}



	public void PickUp() {
		if (GameManager.Instance.IsGamePaused) return;

		isPickedUp = true;

		//reset rotation to 0
		//transform.rotation = Quaternion.identity;
		transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

		OnPickUp?.Invoke();
		
		if (rb != null) {
			//rb.useGravity = false;
			rb.constraints = RigidbodyConstraints.FreezeRotation;
		}

	}

    public void Drop() {
		if (GameManager.Instance.IsGamePaused) return;

		isPickedUp = false;

		OnDrop?.Invoke();

		if (rb != null) {
			//rb.useGravity = true;
			rb.constraints = RigidbodyConstraints.None;
			
			//Fix to stop gravity from accellerating if object didn't stop before being picked up again
			rb.velocity = Vector3.zero;
		}
	}

	public void UpdatePosition(Vector3 newPosition) {
		if (GameManager.Instance.IsGamePaused) return;

		if (isPickedUp) {
			Vector3 targetPosition = newPosition + offset;
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothFactor / rb.mass);
		}
	}

	private Vector3 CalculateOffset() {
		Collider objectCollider = GetComponent<Collider>();
		if (objectCollider != null) {
			objectBounds = objectCollider.bounds;
			return new Vector3(0f, Mathf.Abs(objectBounds.extents.y), 0f);
		} else {
			Debug.LogError(this.name + ": Object must have a collider for offset calculation");
			return Vector3.zero;
		}
	}



}
