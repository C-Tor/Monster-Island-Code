using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LookFor-Default", menuName = "Monster Logic/LookFor Logic/Default")]
public class MonsterLookForDefault : MonsterLookForSOBase {


	public override void DoEnterLogic() {
		base.DoEnterLogic();
		Debug.Log("LookFor state entered");
	}

	public override void DoExitLogic() {
		base.DoExitLogic();
	}

	public override void DoFrameUpdateLogic() {
		base.DoFrameUpdateLogic();
	}

	public override void DoFrequentUpdateLogic() {
		base.DoFrequentUpdateLogic();
		// If found item it was looking for, go into seek need state (via SeekFulfillment)
		Interactable foundItem = monsterBehavior.GetNearbyItem(seekedItem);
		float distanceToForget = 5f;
		if (foundItem != null) {
			monsterBehavior.SeekFulfillment(seekedItem);
		} else if (Vector3.Distance(transform.position, seekedPosition) < distanceToForget) {
			monster.memory.UnsetRememberedLocation(seekedItem);
			monsterBehavior.StateMachine.ChangeState(monsterBehavior.IdleState);
		}
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
}
