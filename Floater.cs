using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
	private Rigidbody rb;
	[SerializeField] private float depthBeforeSubmerged = 1f;
	private float displacementAmount = 3f;
	private bool isInWater;
	private float waterHeight;
	[SerializeField] private float waterDrag;
	[SerializeField] private float waterAngularDrag;

	public int floaterCount = 1;
	InteractiveObject interactiveObject;
	private void Start() {
		interactiveObject = GetComponentInParent<InteractiveObject>();
		rb = GetComponentInParent<Rigidbody>();

		interactiveObject.OnPickUp += InteractiveObject_OnPickUp;
		interactiveObject.OnDrop += InteractiveObject_OnDrop;
	}

	private void FixedUpdate() {
		if (interactiveObject != null && !interactiveObject.isPickedUp) {
			rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
			if (isInWater) {
				if (transform.position.y < waterHeight) {
					float displacementMultiplier = Mathf.Clamp01((waterHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
					rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
					rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
					rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("WaterVolume")) {
			isInWater = true;
			//code here to find the y value of top face of water volume
			waterHeight = other.transform.position.y + other.bounds.extents.y;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("WaterVolume")) {
			isInWater = false;
		}
	}

	private void ResetForces() {
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	private void InteractiveObject_OnDrop() {
		ResetForces();
	}

	private void InteractiveObject_OnPickUp() {
		ResetForces();
	}

}
