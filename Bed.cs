using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable{

	[SerializeField] private Transform sleepPosition;

	private void Awake() {
		ItemType = ItemType.Bed;
	}

	public override void Interact(GameObject interactingObject) {
		MonsterBehaviorController monsterBehavior = interactingObject.GetComponent<Monster>().monsterBehavior;
		monsterBehavior.MonsterSleepInstance.SetBed(this);
		monsterBehavior.StateMachine.ChangeState(monsterBehavior.SleepState);
		Debug.Log(interactingObject + " interacted with bed: " + this.gameObject);
	}

	public Transform GetSleepPosition() {
		return sleepPosition;
	}
}
