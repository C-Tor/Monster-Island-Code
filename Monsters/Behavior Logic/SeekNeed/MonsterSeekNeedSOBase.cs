using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSeekNeedSOBase : MonsterStateSOBase
{

	protected Interactable seekedObject;

	public override void Initialize(GameObject gameObject, Monster monster) {
		this.gameObject = gameObject;
		transform = gameObject.transform;
		this.monster = monster;
		monsterBehavior = monster.GetComponent<MonsterBehaviorController>();

	}

	public override void DoEnterLogic() { }

	public override void DoExitLogic() { ResetValues(); }

	public override void DoFrameUpdateLogic() {
		// If hunted object is null, go back to Idle state.
		// This can happen when the huntedObject leaves the monster's detection area
		if (seekedObject == null) {
			monsterBehavior.StateMachine.ChangeState(monsterBehavior.IdleState);
		}
	}

	public override void DoTickUpdateLogic() { }
	public override void DoFrequentUpdateLogic() {
		//Check if ran into desired object
		Vector3 mtp = monster.transform.position;
		Collider[] colliders = Physics.OverlapCapsule(mtp, new Vector3(mtp.x, mtp.y + monster.GetHeight() + monster.movement.GetBaseOffset(), mtp.z), monster.interactRadius);

		foreach (Collider collider in colliders) {
			Interactable interactable = collider.GetComponent<Interactable>();

			if (interactable != null && interactable.ItemType == seekedObject.ItemType) {

				interactable.Interact(monster.gameObject);
				monster.movement.SetDestinationObject(null);
				return;
			}
		}
	}



	public override void ResetValues() { }

	public virtual void SetHuntedObject(Interactable huntObject) {
		seekedObject = huntObject;
	}

	public Interactable GetHuntedObject() {
		return seekedObject;
	}
}
