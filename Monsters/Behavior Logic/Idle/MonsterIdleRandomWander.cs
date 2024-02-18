using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This Idle logic makes the monster roam to a random point within
/// a certain bounds. Logic needs to be updated, or probably just scrapped.
/// </summary>
[CreateAssetMenu(fileName = "Idle-Random Wander", menuName = "Monster Logic/Idle Logic/Random Wander")]
public class MonsterIdleRandomWander : MonsterIdleSOBase {

	[SerializeField] private float minStandTime = 0f;
	[SerializeField] private float maxStandTime = 5f;
	[SerializeField] private float standingTimer;

	public override void DoEnterLogic() {
		base.DoEnterLogic();

		ResetStandingTimer();
	}

	public override void DoExitLogic() {
		base.DoExitLogic();
	}

	public override void DoFrameUpdateLogic() {
		base.DoFrameUpdateLogic();
		// Only decrement the timer if the monster is standing still
		if (!monster.movement.IsMoving()) {
			standingTimer -= Time.deltaTime;

			// If the standing timer has reached 0, find a new position and reset the timer
			if (standingTimer <= 0) {
				monster.movement.SetDestinationPosition(GetRandomPosition());
				ResetStandingTimer();
			}
		}

	}

	public override void DoFrequentUpdateLogic() {
		base.DoFrequentUpdateLogic();
	}

	public override void DoTickUpdateLogic() {
		base.DoTickUpdateLogic();
	}

	public override void Initialize(GameObject gameObject, Monster monster) {
		base.Initialize(gameObject, monster);
	}

	public override void ResetValues() {
		base.ResetValues();
	}

	private void ResetStandingTimer() {
		standingTimer = Random.Range(minStandTime, maxStandTime);
	}

	private Vector3 GetRandomPosition() {
		Vector3 newPosition = new Vector3(Random.Range(-10, 10), 0f, Random.Range(-10, 10));
		return newPosition;
	}

}
