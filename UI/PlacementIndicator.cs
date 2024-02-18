using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementIndicator : MonoBehaviour
{
	// display visual indicator at cursor hit in world position, when holding object.
	[SerializeField] private GameObject placementIndicatorPrefab;
	private GameObject placementIndicator;
	private PlayerController playerController;
	private bool holdingObject;

	private void Start() {
		holdingObject = false;
		

		placementIndicator = Instantiate(placementIndicatorPrefab, Vector3.zero, Quaternion.identity);

		if (placementIndicator == null ) {
			Debug.LogError("PlacementIndicator failed to instantiate");
		}

		playerController = GetComponent<PlayerController>();
		playerController.OnObjectPickedUp += PlayerController_OnObjectPickedUp;
		playerController.OnObjectDropped += PlayerController_OnObjectDropped;

		Hide();
	}

	private void Update() {
		if (holdingObject) {
			SetIndicatorPosition(playerController.GetPlacementPosition());
		}

	}

	private void PlayerController_OnObjectDropped() {
		holdingObject = false;
		Hide();
	}

	private void PlayerController_OnObjectPickedUp() {
		holdingObject = true;
		Show();
	}

	public void SetIndicatorPosition(Vector3 position) {
		placementIndicator.transform.position = position;
	}

	private void Show() {
		placementIndicator.SetActive(true);
	}

	private void Hide() {
		placementIndicator.SetActive(false);
	}
}
