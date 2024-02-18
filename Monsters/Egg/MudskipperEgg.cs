using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudskipperEgg : Egg {

	private bool isInWater;

	private void Update() {
		if (isInWater) {
			hatchTimer -= Time.deltaTime;
		}
		if (hatchTimer <= 0) {
			HatchMonster();
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("WaterVolume")) {
			isInWater = true;
			isGrowing = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("WaterVolume")) {
			isInWater = false;
			isGrowing = false;
		}
	}

}

