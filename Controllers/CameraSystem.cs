using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;


	[SerializeField] private bool useDragPan;
	
	[SerializeField] private float followOffsetMin;
	[SerializeField] private float followOffsetMax;
	[SerializeField] private float pitchMin;
	[SerializeField] private float pitchMax;
	[SerializeField] private float followOffsetYMin;
	[SerializeField] private float followOffsetYMax;
	[SerializeField] private float followOffsetYMinZ;
	[SerializeField] private float followOffsetYMaxZ;
	[SerializeField] private float zoomSpeed;
	[SerializeField] private float zoomAmountY;
	[SerializeField] private float zoomAmountZ;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float moveSpeedBoostSet;
	[SerializeField] private float moveSpeedZoomFactor;
	[SerializeField] private float mouseOrbitFactor;
	[SerializeField] private float rotateSpeed;
	[SerializeField] private float jumpToTargetSpeed;
	[SerializeField] private float cameraLerpSpeed;
	[SerializeField] private Vector3 objectFollowOffset;

	[SerializeField] private Transform lookAtObject;
	
	[SerializeField] private LayerMask cameraFollowLayer;
	[SerializeField] private LayerMask collideLayer;

	[Header("Bounds")]
	[SerializeField] private float posX; 
	[SerializeField] private float negX;
	[SerializeField] private float posZ, negZ;

	private GameManager gameManager;
	private SettingsManager settingsManager;
	private Camera mainCamera;

	private float originalY;
	private bool followingObject;
	private GameObject followObject;
	private float moveSpeedBoost;
	private bool dragPanActive;
	private bool mouseOrbitActive = false;
	private Vector2 lastMousePosition;
	private Vector3 followOffset;

	private void Awake() {
		followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
		moveSpeedBoost = 1f;
	}

	private void Start() {
		gameManager = GameManager.Instance;
		settingsManager = SettingsManager.Instance;
		mainCamera = Camera.main;
		originalY = transform.position.y;
		followingObject = false;
		followObject = null;
		MonsterManager.Instance.OnMonsterHatched += MonsterManager_OnMonsterHatched;

		gameManager.OnNewGame += GameManager_OnNewGame;

		// Pass main camera (listener) to soundmanager for ambiance zones
		SoundManager.Instance.SetPlayer(mainCamera.transform);
	}

	private void GameManager_OnNewGame() {
		transform.position = MonsterManager.Instance.MonsterSpawnPosition.position;
		Debug.Log("weekn");
	}

	private void MonsterManager_OnMonsterHatched(Monster monster) {
		SetFollowObject(monster.gameObject);
	}

	private void Update() {
		HandleMoveSpeedBoost();

		HandleClickToFollow();
		
		HandleFollow();

		HandleCameraMovement();

		if (settingsManager.UseEdgePan && !followingObject) {
			HandleCameraMovementEdgeScroll();
		}
		if (useDragPan) {
			HandleCameraMovementDragPan();
		}

		HandleCameraRotation();

		HandleCameraZoom_MoveForward();
		//HandleCameraZoom();

	}

	private void HandleClickToFollow() {
		if (Input.GetMouseButtonDown(0)) {
			PerformFollowRaycast(cameraFollowLayer);
		}
	}
	
	private void HandleFollow() {
		if (followingObject) {
			if (followObject != null) {
				Monster monster = followObject.GetComponent<Monster>();
				if (monster != null) { 
					transform.position = followObject.transform.position + GetFollowOffset(monster);
				}else {
					transform.position = followObject.transform.position + objectFollowOffset;
				}
			} else {
				UnsetFollowObject();
			}
		}
	}

	private Vector3 GetFollowOffset(Monster monster) {
		return new Vector3(0, monster.GetHeight() / 2, 0);
	}

	private void HandleMoveSpeedBoost() {
		if (Input.GetKey(KeyCode.LeftShift)) {
			moveSpeedBoost = moveSpeedBoostSet;
		} else {
			moveSpeedBoost = 1;
		}
	}

	private void HandleCameraMovement() {

		Vector3 inputDir = new Vector3(0, 0, 0);

		if (Input.GetKey(KeyCode.W)) {
			inputDir.z = +1f;
		}
		if (Input.GetKey(KeyCode.S)) {
			inputDir.z = -1f;
		}
		if (Input.GetKey(KeyCode.A)) {
			inputDir.x = -1f;
		}
		if (Input.GetKey(KeyCode.D)) {
			inputDir.x = +1f;
		}

		Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
		if (moveDir != Vector3.zero) {
			UnsetFollowObject();
		}
		if (!followingObject) {
			UpdateCameraPosition(moveDir);
		}
	}


	private void UpdateCameraPosition(Vector3 moveDir) {
		float targetY = FindCameraYPos();
		float currentY = transform.position.y;

		// Use Mathf.Lerp for smoothing only on the Y-axis
		float newY = Mathf.Lerp(currentY, targetY, Time.deltaTime * cameraLerpSpeed);

		// Move the camera in the specified direction
		Vector3 newPosition = transform.position + moveDir * moveSpeed * moveSpeedBoost * Time.deltaTime;

		// Check if camera will be in bounds
		newPosition = AdjustForInBounds(newPosition);
		// Set the new position with the smoothed Y value and the original X and Z values
		transform.position = new Vector3(newPosition.x, newY, newPosition.z);


	}

	private Vector3 AdjustForInBounds(Vector3 desiredPosition) {
		Vector3 newPosition = desiredPosition;
		if (desiredPosition.x > posX) {
			newPosition.x = posX;
		} else if (desiredPosition.x < negX) {
			newPosition.x = negX;
		}

		if (desiredPosition.z > posZ) {
			newPosition.z = posZ;
		} else if (desiredPosition.z < negZ) {
			newPosition.z = negZ;
		}

		return newPosition;
		// You can add additional checks or modifications if needed
	}

	private void HandleCameraMovementEdgeScroll() {
		Vector3 inputDir = new Vector3(0, 0, 0);

		int edgeScrollSize = 20;
		if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
		if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
		if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
		if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1f;

		// Use world space directions for forward and right
		Vector3 forwardDir = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 rightDir = Vector3.Scale(transform.right, new Vector3(1, 0, 1)).normalized;

		// Calculate movement direction by combining forward and right
		Vector3 moveDir = forwardDir * inputDir.z + rightDir * inputDir.x;

		// Apply movement in world space
		UpdateCameraPosition(moveDir);
	}

	// Work on this to use with raycasts and actual world geometry
	private void HandleCameraMovementDragPan() {
		Vector3 inputDir = new Vector3(0, 0, 0);


		if (Input.GetMouseButtonDown(1)) {
			dragPanActive = true;
			lastMousePosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(1)) {
			dragPanActive = false;
		}

		if (dragPanActive) {
			Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

			float dragPanSpeed = 2f;

			inputDir.x = mouseMovementDelta.x * dragPanSpeed;
			inputDir.z = mouseMovementDelta.y * dragPanSpeed;

			lastMousePosition = Input.mousePosition;
		}

		Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

		transform.position += moveDir * moveSpeed * Time.deltaTime;
	}

	private void HandleCameraRotation() {
		float rotateDir = 0f;

		if (Input.GetMouseButtonDown(2)) {
			mouseOrbitActive = true;
			lastMousePosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(2)) {
			mouseOrbitActive = false;
		}

		if (mouseOrbitActive) {
			Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

			// Calculate the new rotation angles
			float newPitch = transform.eulerAngles.x - (mouseMovementDelta.y / Screen.height) * mouseOrbitFactor;
			float newYaw = transform.eulerAngles.y + (mouseMovementDelta.x / Screen.width) * mouseOrbitFactor;

			// Apply clamp to X rotation limits
			newPitch = Mathf.Clamp(newPitch, pitchMin, pitchMax);

			// Apply the new rotation angles separately
			transform.eulerAngles = new Vector3(newPitch, newYaw, 0);

			lastMousePosition = Input.mousePosition;
		} else {
			if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
			if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

			// Only modify the Yaw axis when not in mouse orbit mode
			transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
		}
	}

	private void HandleCameraZoom_MoveForward() {
		Vector3 zoomDir = followOffset.normalized;

		// Adjust the zoom speed based on the current followOffset magnitude
		float adjustedZoomSpeed = zoomSpeed * (followOffset.magnitude / followOffsetMax);

		if (Input.mouseScrollDelta.y > 0) {
			followOffset -= zoomDir * adjustedZoomSpeed;
		}
		if (Input.mouseScrollDelta.y < 0) {
			followOffset += zoomDir * adjustedZoomSpeed;
		}

		// Clamp the followOffset magnitude to the specified range
		float clampedMagnitude = Mathf.Clamp(followOffset.magnitude, followOffsetMin, followOffsetMax);
		followOffset = followOffset.normalized * clampedMagnitude;

		// Update the virtual camera's followOffset using Lerp for smooth transitions
		cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset
			= Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

		// Adjust moveSpeed based on the updated followOffset.magnitude for a zoom-dependent move speed
		moveSpeed = followOffset.magnitude * moveSpeedZoomFactor;
	}

	private void HandleCameraZoom_LowerY() {

		if (Input.mouseScrollDelta.y > 0) {
			followOffset.y -= zoomAmountY * followOffset.y;
			followOffset.z -= zoomAmountZ * followOffset.z;
		}
		if (Input.mouseScrollDelta.y < 0) {
			followOffset.y += zoomAmountY * followOffset.y;
			followOffset.z += zoomAmountZ * followOffset.z;
		}


		followOffset.y = Mathf.Clamp(followOffset.y, followOffsetYMin, followOffsetYMax);
		followOffset.z = Mathf.Clamp(followOffset.z, followOffsetYMaxZ, followOffsetYMinZ);

		cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset
			= Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

		moveSpeed = (followOffset.y + 1) * moveSpeedZoomFactor;


	}

	private void HandleCameraZoom() {
		// Use the mouse scroll delta to adjust zoom factors
		float zoomDelta = Input.mouseScrollDelta.y;

		// Calculate the multiplier based on the zoom delta and existing values
		float zoomMultiplier = 1f + zoomDelta * zoomAmountY;

		// Adjust the followOffset.y and followOffset.z based on the multiplier
		followOffset.y *= zoomMultiplier;
		followOffset.z *= zoomMultiplier;

		// Clamp the values to the specified range
		followOffset.y = Mathf.Clamp(followOffset.y, followOffsetYMin, followOffsetYMax);
		followOffset.z = Mathf.Clamp(followOffset.z, followOffsetYMaxZ, followOffsetYMinZ);

		// Update the virtual camera's followOffset using Lerp for smooth transitions
		cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset
			= Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

		// Adjust moveSpeed based on followOffset.y for a zoom-dependent move speed
		moveSpeed = (followOffset.y + 1) * moveSpeedZoomFactor;
	}

	private void PerformFollowRaycast(LayerMask layerMask) {

		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {

			GameObject hitObject = hit.collider.gameObject;

			if (hitObject != null) {

				SetFollowObject(hitObject);
			}
		}
	}

	private void SetFollowObject(GameObject followObject) {
		followingObject = true;
		this.followObject = followObject;
		Monster followMonster = followObject.GetComponent<Monster>();
		if (followMonster != null) {
			MonsterManager.Instance.SelectMonster(followMonster);
		}
	}

	private void UnsetFollowObject() {
		followingObject = false;
		this.followObject = null;
		MonsterManager.Instance.DeselectMonster();
	}

	private float FindCameraYPos() {
		RaycastHit hit;

		Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z);
		float sphereRadius = 1.5f;

		if (Physics.SphereCast(rayOrigin, sphereRadius, Vector3.down, out hit, Mathf.Infinity, collideLayer)) {
			return hit.point.y;
		}
		return originalY;
	}
}
