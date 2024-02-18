using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBounds : MonoBehaviour
{
	[SerializeField] private GameObject bounds;

	private Vector3 originalPosition;

	private void Start() {
		originalPosition = transform.position;
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject == bounds) {
			LeaveBoundBehavior();
		}
	}

	private void LeaveBoundBehavior() {
		InteractiveObject interactive = GetComponent<InteractiveObject>();
		if (interactive != null) {
			GameManager.Instance.DropObject(interactive);
		}
		transform.position = originalPosition;

	}

}
