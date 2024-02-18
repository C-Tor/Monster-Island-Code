using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLookForSOBase : MonsterStateSOBase
{

	protected ItemType seekedItem;
	protected Vector3 seekedPosition;

	public override void Initialize(GameObject gameObject, Monster monster) {
		this.gameObject = gameObject;
		transform = gameObject.transform;
		this.monster = monster;
		monsterBehavior = monster.GetComponent<MonsterBehaviorController>();

	}

	public override void DoEnterLogic() { }

	public override void DoExitLogic() { ResetValues(); }

	public override void DoFrameUpdateLogic() {
	}

	public override void DoTickUpdateLogic() { }
	public override void DoFrequentUpdateLogic() {
	}

	public void LookFor(ItemType itemType) {
		monster.movement.SetDestinationObject(null);
		seekedPosition = monster.memory.GetItemRememberedLocation(itemType);
		monster.movement.SetDestinationPosition(seekedPosition);
	}

	public override void ResetValues() { }

}
