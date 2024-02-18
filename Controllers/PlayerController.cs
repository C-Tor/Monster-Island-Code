using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public event Action OnObjectPickedUp;
	public event Action OnObjectDropped;

	private Camera mainCamera;
	private InteractiveObject heldObject;
	private PlacementIndicator placementIndicator;

	[SerializeField] private LayerMask pickupLayer;
	[SerializeField] private LayerMask placementLayer;
	[SerializeField] private float hoverOffset;

	private Vector3 lastValidPosition;

	private void Start() {
		mainCamera = Camera.main;
		placementIndicator = GetComponent<PlacementIndicator>();
		if (placementIndicator == null ) {
			Debug.LogError("PlacementIndicator not found");
		}

		GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
		GameManager.Instance.OnInteractiveObjectDrop += GameManager_OnInteractiveObjectDrop;
	}

	private void GameManager_OnInteractiveObjectDrop(InteractiveObject obj) {
		if (obj == heldObject) {
			DropObject();
		}
	}

	private void GameManager_OnGameResumed() {
		DropObject();
	}

	void Update()
    {
		if (GameManager.Instance.IsGamePaused) return;
		//Debug.Log("Held Object: " + heldObject);
		if (Input.GetMouseButton(0) && heldObject == null) {
			//left mouse button pressed

			PerformRaycast(pickupLayer, true);

		} if (Input.GetMouseButton(0) && heldObject != null) {
			//Left mouse button held and holding object
			UpdateHeldObjectPosition();

		} else if (Input.GetMouseButtonUp(0) && heldObject != null) {
			//left mouse button released and holding object 
			DropObject();

		}
    }

	private void PerformRaycast(LayerMask layerMask, bool isPickupRaycast) {

		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {

			InteractiveObject hitObject = hit.collider.GetComponent<InteractiveObject>();
			
			if (hitObject != null && isPickupRaycast) {
				if (hitObject.canPickUp) {
					PickUpObject(hitObject);
				}
			}
		}
	}

	private void UpdateHeldObjectPosition() {
		Vector3 newPosition = GetPlacementPosition(mainCamera.ScreenPointToRay(Input.mousePosition));

		// add offset to y-coordinate
		newPosition.y += hoverOffset;
		
		heldObject.UpdatePosition(newPosition);
	}

	// Get position for placement on the ground
	private Vector3 GetPlacementPosition(Ray ray) {
		RaycastHit hit;

		// Placement raycast
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer)) {
			lastValidPosition = hit.point;
			return hit.point;
		} else {
			return lastValidPosition;
		}
	}

	private void PickUpObject(InteractiveObject obj) {

		heldObject = obj;
		heldObject.PickUp();
		OnObjectPickedUp?.Invoke();
	}

	private void DropObject() {
		heldObject?.Drop();
		heldObject = null;
		OnObjectDropped?.Invoke();

	}

	public Vector3 GetPlacementPosition() {
		return lastValidPosition;
	}
}
