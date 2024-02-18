using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCamer : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] private float rotateSpeed;

	private Camera mainCamera;

	private void Update() {

		HandleCameraRotation();

	}



	private void HandleCameraRotation() {
		Vector3 rotate = new Vector3(0f, 1f, 0f) * rotateSpeed;
		transform.Rotate(rotate);
	}

}
