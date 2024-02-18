using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHook : MonoBehaviour
{
	private FishingTool fishingTool;
	private bool isInWater;

	private void Start() {
		fishingTool = GetComponentInParent<FishingTool>();
	}

	public bool IsInWater() {
		return isInWater;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("FishingVolume")) {
			isInWater = true;
			//code here to find the y value of top face of water volume
			fishingTool.ResetHookTimer();
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("FishingVolume")) {
			isInWater = false;
		}
	}
}
