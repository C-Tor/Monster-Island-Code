using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectCheck : MonoBehaviour
{
	private MonsterBehaviorController monsterBehavior;

	private void Awake() {
		monsterBehavior = GetComponentInParent<MonsterBehaviorController>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.TryGetComponent<Interactable>(out var detectable)) {
			monsterBehavior.DetectedObjectEntered(detectable);
		}
	}

}
