using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSleepState : MonsterState {
	public MonsterSleepState(MonsterBehaviorController monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine) {
	}

	public override void EnterState() {
		base.EnterState();
		monsterBehavior.MonsterSleepInstance.DoEnterLogic();
	}

	public override void ExitState() {
		base.ExitState();
		monsterBehavior.MonsterSleepInstance.DoExitLogic();

	}

	public override void FrameUpdate() {
		base.FrameUpdate();
		monsterBehavior.MonsterSleepInstance.DoFrameUpdateLogic();
	}

	public override void FrequentUpdate() {
		base.FrequentUpdate();
		monsterBehavior.MonsterSleepInstance.DoFrequentUpdateLogic();

	}

	public override void TickUpdate() {
		base.TickUpdate();
		monsterBehavior.MonsterSleepInstance.DoTickUpdateLogic();

	}
}
