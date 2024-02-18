using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public MonsterState CurrentMonsterState { get; set; }

	public void Initialize(MonsterState startingState) {
		CurrentMonsterState = startingState;
		CurrentMonsterState.EnterState();
	}

	public void ChangeState(MonsterState newState) {
		CurrentMonsterState.ExitState();
		CurrentMonsterState = newState;
		CurrentMonsterState.EnterState();
	}
}
