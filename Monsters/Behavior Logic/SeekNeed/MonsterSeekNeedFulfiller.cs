using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeekNeed-Default", menuName = "Monster Logic/Seek")]
public class MonsterSeekNeedFulfiller : MonsterSeekNeedSOBase {



	public override void DoEnterLogic() {
		base.DoEnterLogic();
		monster.movement.SetDestinationObject(seekedObject.gameObject);
	}

	public override void DoExitLogic() {
		base.DoExitLogic();
		monster.movement.SetDestinationObject(null);
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

	public override void Initialize(GameObject gameObject, Monster monster) {
		base.Initialize(gameObject, monster);
	}

	public override void ResetValues() {
		base.ResetValues();
	}


}
