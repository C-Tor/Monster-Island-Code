using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSleepingSOBase : MonsterStateSOBase {

	protected Bed bed;

	public override void Initialize(GameObject gameObject, Monster monster) {
		this.gameObject = gameObject;
		transform = gameObject.transform;
		this.monster = monster;
		monsterBehavior = monster.GetComponent<MonsterBehaviorController>();
	}
	public override void DoEnterLogic() {
		base.DoEnterLogic();
	}

	public override void DoExitLogic() {
		base.DoExitLogic();
	}

	public override void DoFrameUpdateLogic() {
		base.DoFrameUpdateLogic();
	}

	public override void DoFrequentUpdateLogic() {
		base.DoFrequentUpdateLogic();
	}

	public override void DoTickUpdateLogic() {
		base.DoTickUpdateLogic();
	}

	public void SetBed(Bed bedSet) {
		bed = bedSet;
	}

}
