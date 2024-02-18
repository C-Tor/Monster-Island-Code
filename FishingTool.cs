using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingTool : MonoBehaviour
{
	[Header("Hook Setings")]
	[SerializeField] private float minHookTime;
	[SerializeField] private float maxHookTime;
	[SerializeField] private float minOpportunityTime;
	[SerializeField] private float maxOpportunityTime;
	[SerializeField] private float bobAmount;
	[Header("Spawned Object")]
	[SerializeField] private GameObject objectToCatch;
	[Header("Catch Launch Settings")]
	[SerializeField] private float upwardForce = 10f;
	[SerializeField] private float horizontalForceMin = -5f;
	[SerializeField] private float horizontalForceMax = 5f;
	[SerializeField] private float torqueForceMin = -5f;
	[SerializeField] private float torqueForceMax = 5f;
	[SerializeField] private float launchPower;

	private float hookTimer;
	private float opportunityTime;
	private float waterHeight;
	private bool isInWater;
	private bool fishOnHook;
	private InteractiveObject interactiveObject;
	private FishingHook fishingHook;


	Rigidbody rb;

	private void Start() {
		fishingHook = GetComponentInChildren<FishingHook>();
		fishOnHook = false;
		rb = GetComponent<Rigidbody>();
		interactiveObject = GetComponent<InteractiveObject>();
		interactiveObject.OnPickUp += FishingTool_OnPickUp;
	}

	private void Update() {
		isInWater = fishingHook.IsInWater();
		if (interactiveObject.isPickedUp) {
			return;
		}
		if (isInWater) {
			hookTimer -= Time.deltaTime;
			if (hookTimer <= 0) {
				fishOnHook = true;
				if (hookTimer < -opportunityTime) {
					ResetHookTimer();
				}
			} else {
				fishOnHook = false;
			}
		} else {
			fishOnHook = false;
		}
	}

	private void FixedUpdate() {
		if (fishOnHook) {
			BobBobber();
		}
	}

	private void CatchFish() {
		fishOnHook = false;
		Debug.Log("Fish caught!");

		// Instantiate the fish object
		GameObject spawnedObject = Instantiate(objectToCatch, fishingHook.transform.position, Random.rotation);

		// Get the Rigidbody component of the spawned fish
		Rigidbody fishRigidbody = spawnedObject.GetComponent<Rigidbody>();

		if (fishRigidbody != null) {
			// Set the initial velocity for the fish using serialized fields
			float horizontalForce = Random.Range(horizontalForceMin, horizontalForceMax);
			Vector3 launchDirection = new Vector3(horizontalForce, upwardForce, horizontalForce).normalized * launchPower;
			fishRigidbody.velocity = launchDirection;

			// Add torque for a spinning effect using serialized fields
			float torqueForce = Random.Range(torqueForceMin, torqueForceMax);
			fishRigidbody.AddTorque(Vector3.up * torqueForce, ForceMode.Impulse);
		}
	}

	private void FishingTool_OnPickUp() {
		if (fishOnHook) {
			CatchFish();
		}
		ResetHookTimer();
	}

	public void ResetHookTimer() {
		hookTimer = Random.Range(minHookTime, maxHookTime);
		opportunityTime = Random.Range(minOpportunityTime, maxOpportunityTime);
		Debug.Log("now set, hookTimer: " + hookTimer + " opportunityTime: " +  opportunityTime);
	}

	private void BobBobber() {
		// Randomly change direction and force for bobbing
		Vector3 bobForce = new Vector3(0f, Random.Range(-1f, 1f), 0f).normalized * bobAmount;

		// Apply the force to the rigidbody for physics-based bobbing
		rb.AddForce(bobForce, ForceMode.Force);
	}


}
