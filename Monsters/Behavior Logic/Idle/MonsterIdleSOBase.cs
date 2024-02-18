using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleSOBase : MonsterStateSOBase
{

	public override void Initialize(GameObject gameObject, Monster monster) { 
		this.gameObject = gameObject;
		transform = gameObject.transform;
		this.monster = monster;
		monsterBehavior = monster.GetComponent<MonsterBehaviorController>();
	}

	public override void DoEnterLogic() {

	}

	public override void DoExitLogic() { ResetValues(); }

	public override void DoFrameUpdateLogic() {
		
	}
	public override void DoTickUpdateLogic() {
		
	}

	public override void DoFrequentUpdateLogic() {
		List<Need> sortedNeeds = monster.needsController.GetNeedsSortedByPriority();

		foreach (Need need in sortedNeeds) {
			if (!need.IsNeeded()) {
				continue;
			}
			ItemType neededItemType = MonsterManager.Instance.GetItemTypeForNeed(need.type);
			monsterBehavior.SeekFulfillment(neededItemType);
		}
	}


	public override void ResetValues() { }

}
