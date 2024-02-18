using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sleep-Default", menuName = "Monster Logic/Sleep/Default")]
public class MonsterSleepDefault : MonsterSleepingSOBase {
	private float[] originalChangeRates;
	[SerializeField] private float energyChangeRate;
	[SerializeField] private float otherNeedsChangeFactor;
	
	public override void DoEnterLogic() {
		base.DoEnterLogic();
		
		originalChangeRates = new float[monster.needsController.GetNeedCount()];

		for (int i = 0; i < monster.needsController.GetNeedCount(); i++) {
			Need currentNeed = monster.needsController.GetNeedByIndex(i);

			if (currentNeed != null) {
				originalChangeRates[i] = currentNeed.changeRate;
				currentNeed.changeRate *= otherNeedsChangeFactor;
			} else {
				Debug.LogError("Need at index " + i + " is null.");
			}
		}


		Need energyNeed = monster.needsController.GetNeedByType(NeedType.Energy);
		if (energyNeed != null) {
			energyNeed.changeRate = energyChangeRate;
		} else {
			Debug.LogError("Energy Need is null.");
		}
		Debug.Log("Sleep state entered");

		//Visual
		monster.GetComponentInChildren<MonsterVisual>().SetAnimBool(MonsterAnim.IsSleeping, true);
		monster.transform.position = bed.GetSleepPosition().position;
	}

	public override void DoExitLogic() {
		base.DoExitLogic();
		for (int i = 0; i < monster.needsController.GetNeedCount(); i++) {
			monster.needsController.GetNeedByIndex(i).changeRate = originalChangeRates[i];
		}
		Debug.Log("Sleep state exited");

		//Visual
		monster.GetComponentInChildren<MonsterVisual>().SetAnimBool(MonsterAnim.IsSleeping, false);

	}

	public override void DoFrameUpdateLogic() {
		base.DoFrameUpdateLogic();
		monster.transform.position = bed.GetSleepPosition().position;

		Need mostNeededNeed = monster.needsController.GetMostNeededNeed();
		if (mostNeededNeed != null) {
			// make this a dictionary or something at some point
			switch (mostNeededNeed.type) {
				case NeedType.Food:
					monsterBehavior.SeekFulfillment(ItemType.Food);
					break;
				default:
					break;
			}
		}
		if (monster.needsController.GetNeedByType(NeedType.Energy).value >= monster.needsController.GetNeedByType(NeedType.Energy).maxValue) {
			monsterBehavior.StateMachine.ChangeState(monsterBehavior.IdleState);
		}
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
