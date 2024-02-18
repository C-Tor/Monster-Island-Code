using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterState
{


	public MonsterIdleState(MonsterBehaviorController monsterBehavior, MonsterStateMachine monsterStateMachine) : base(monsterBehavior, monsterStateMachine) {
	}

	public override void EnterState() {
		base.EnterState();
		monsterBehavior.MonsterIdleInstance.DoEnterLogic();
	}

	public override void ExitState() {
		base.ExitState();
		monsterBehavior.MonsterIdleInstance.DoExitLogic();
	}

	public override void FrameUpdate() {
		base.FrameUpdate();
		monsterBehavior.MonsterIdleInstance.DoFrameUpdateLogic();
	}

	public override void TickUpdate() {
		base.TickUpdate();
		monsterBehavior.MonsterIdleInstance.DoTickUpdateLogic();
	}

	public override void FrequentUpdate() {
		base.FrequentUpdate();
		monsterBehavior.MonsterIdleInstance.DoFrequentUpdateLogic();

	}


}
