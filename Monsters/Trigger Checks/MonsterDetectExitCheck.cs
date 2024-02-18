using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectExitCheck : MonoBehaviour
{
	private MonsterBehaviorController monsterBehavior;

	private void Awake() {
		monsterBehavior = GetComponentInParent<MonsterBehaviorController>();
	}

	private void OnTriggerExit(Collider other) {
		if (other.TryGetComponent<Interactable>(out var detectable)) {
			monsterBehavior.DetectedObjectExited(detectable);

			// If the object leaving trigger is currently being hunted by monster,
			// set hunted object to null, so it stops following object out of range.
			if (monsterBehavior.MonsterSeekNeedInstance.GetHuntedObject() == detectable) {
				monsterBehavior.MonsterSeekNeedInstance.SetHuntedObject(null);
			}
		}
	}
}
