using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This IdleLogic makes the monster choose a random spot within a sphere 
/// (of size roamRadius), and gets the closest navmesh surface from that point,  
/// then move to that point and wait for a random time (from minStandTime to MaxStandTime)
/// </summary>
[CreateAssetMenu(fileName = "Idle-Incremental Wander", menuName = "Monster Logic/Idle Logic/Incremental Wander")]
public class MonsterIdleIncrementalWander : MonsterIdleSOBase {

	[SerializeField] private float minStandTime = 0f;
	[SerializeField] private float maxStandTime = 5f;
	[SerializeField] private float standingTimer;
	[SerializeField] private float roamRadius;

	public override void DoEnterLogic() {
		base.DoEnterLogic();
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
				monster.movement.SetDestinationPosition(GetRandomInSphere(transform.position, roamRadius*monster.Scale, -1));
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

	public override void ResetValues() {
		base.ResetValues();
	}
	private void ResetStandingTimer() {
		standingTimer = Random.Range(minStandTime, maxStandTime);
	}

	private Vector3 GetRandomInSphere(Vector3 origin, float distance, int layermask) {
		Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

		randomDirection += origin;

		NavMeshHit navHit;

		NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

		return navHit.position;
	}
}
