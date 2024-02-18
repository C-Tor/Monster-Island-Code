using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLookForState : MonsterState
{


	public MonsterLookForState(MonsterBehaviorController monsterBehavior, MonsterStateMachine monsterStateMachine) : base(monsterBehavior, monsterStateMachine) {
	}

	public override void EnterState() {
		base.EnterState();
		monsterBehavior.MonsterLookForInstance.DoEnterLogic();
	}

	public override void ExitState() {
		base.ExitState();
		monsterBehavior.MonsterLookForInstance.DoExitLogic();
	}

	public override void FrameUpdate() {
		base.FrameUpdate();
		monsterBehavior.MonsterLookForInstance.DoFrameUpdateLogic();
	}

	public override void TickUpdate() {
		base.TickUpdate();
		monsterBehavior.MonsterLookForInstance.DoTickUpdateLogic();
	}

	public override void FrequentUpdate() {
		base.FrequentUpdate();
		monsterBehavior.MonsterLookForInstance.DoFrequentUpdateLogic();

	}


}
