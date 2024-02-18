using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
	private Rigidbody rb;
	private bool isInWater;
	[SerializeField] private float buoyancyForce;

	private void Start() {
		rb = GetComponent<Rigidbody>();

	}

	private void FixedUpdate() {
		if (isInWater) {
			ApplyBuoyancy();
		}
	}

	private void ApplyBuoyancy() {
		rb.AddForce(Vector3.up * buoyancyForce * rb.mass);

		// Optionally, you can apply additional dampening forces
		// This helps stabilize the object and prevents excessive bobbing
		ApplyDrag();
	}
	private void ApplyDrag() {
		// Apply drag forces to stabilize the object
		rb.AddForce(-rb.velocity * rb.drag, ForceMode.Acceleration);
		rb.AddTorque(-rb.angularVelocity * rb.angularDrag, ForceMode.Acceleration);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("WaterVolume")) {
			isInWater = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("WaterVolume")) {
			isInWater = false;
		}
	}
}
