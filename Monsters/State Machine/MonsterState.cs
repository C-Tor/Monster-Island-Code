using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState
{
	protected MonsterBehaviorController monsterBehavior;
	protected MonsterStateMachine monsterStateMachine;

	public MonsterState(MonsterBehaviorController monster, MonsterStateMachine monsterStateMachine) {
		this.monsterBehavior = monster;
		this.monsterStateMachine = monsterStateMachine;
	}

	public virtual void EnterState() { }
	public virtual void ExitState() { }

	//Runs off Unity's Update()
	public virtual void FrameUpdate() { }

	//Runs off TimingManager's tick
	public virtual void TickUpdate() {}

	//Runs off Unity's FixedUpdate()
	public virtual void FrequentUpdate() { }

	//public virtual void AnimationTriggerEvent() { }

}
