using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterSeekNeedState : MonsterState {
	public MonsterSeekNeedState(MonsterBehaviorController monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine) {

	}

	public override void EnterState() {
		base.EnterState();
		monsterBehavior.MonsterSeekNeedInstance.DoEnterLogic();
	}

	public override void ExitState() {
		base.ExitState();
		monsterBehavior.MonsterSeekNeedInstance.DoExitLogic();
	}

	public override void FrameUpdate() {
		base.FrameUpdate();
		monsterBehavior.MonsterSeekNeedInstance.DoFrameUpdateLogic();
	}

	public override void FrequentUpdate() {
		base.FrequentUpdate();
		monsterBehavior.MonsterSeekNeedInstance.DoFrequentUpdateLogic();

	}

	public override void TickUpdate() {
		base.TickUpdate();
		monsterBehavior.MonsterSeekNeedInstance.DoTickUpdateLogic();
	}



}