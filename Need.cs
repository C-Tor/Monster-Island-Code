using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A need for monsters. Each monster can have many needs.
[System.Serializable]
public class Need
{
	public NeedType type;
	public string name;

	// Current value of the need
	public float value;

	// Maximum value a need can reach
	public float maxValue;

	// How much a need changes with each tick
	public float changeRate;

	// if below this, pet dies. Set really really low if don't cause death.
	public float deathValue;

	// If need is below this value, it is considered needed and the monster
	// may seek ways to increase the need.
	public float neededValue;


	private Monster monster; // parent monster

	public Need(NeedType type, 
		float initialValue, 
		float maxValue, 
		float changeRate,
		float deathValue,
		float neededValue) {

		this.type = type;
		this.name = type.ToString();
		this.value = initialValue;
		this.maxValue = maxValue;
		this.changeRate = changeRate;
		this.deathValue = deathValue;
		this.neededValue = neededValue;


	}

	//What should happen to this Need with each tick
	public void TickBehavior() {
		ChangeNeedValue(changeRate);

	}

	public void ChangeNeedValue(float amount) {

		// Don't allow the need to fill past the max value
		if (value +	amount >= maxValue) {
			value = maxValue;
		} else{
			if (value < 0 && amount > 0) {
				//If increasing value and value negative, set to 0 then increase
				value = 0;
			}
			value += amount;
		}
	}

	public bool IsNeeded() { return value < neededValue; }

	/// <summary>
	/// This function calculates a priority value for this need, based on 
	/// the current value, change rate, deathValue and so on. 
	/// Used by monsterBehaviorController to know what need to currently focus.
	/// </summary>
	/// <returns> float priority </returns>
	public float CalculatePriority() {
		if (changeRate != 0) {
			return deathValue + (value / changeRate);
		} else {
			return deathValue;
		}
	}
}
