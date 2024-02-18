using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NeedsController : MonoBehaviour
{
	[SerializeField] private List<Need> needs = new List<Need>();

	private void Start() {

	}

	public void AddNeed(Need need) {
		Need newNeed = new Need(need.type, need.value, need.maxValue, need.changeRate, need.deathValue, need.neededValue);
		needs.Add(newNeed);
	}

	//Called after each tick (tick time in TimingManager, Called in Monster)
	public void Tick() {
		foreach (Need need in needs) {
			need.TickBehavior();
		}
	}

	/// <summary>
	/// Returns need by NeedType
	/// </summary>
	/// <param name="type">The type of need to retrieve</param>
	/// <returns>Need with given Type; null if not present</returns>
	public Need GetNeedByType(NeedType type) {
		foreach (Need need in needs) {
			if (need.type == type) {
				return need;
			}
		}
		return null;
	}

	public Need GetNeedByIndex(int index) {
		if (index >= 0 && index < GetNeedCount()) {
			return needs[index];
		}
		return null;
	}


	public int GetNeedCount() { return needs.Count;}

	/// <summary>
	/// Returns the need which requires the most immediate attention
	/// </summary>
	/// <returns>Most Needed Need; Null if no needed need</returns>
	public Need GetMostNeededNeed() {
		Need mostNeededNeed = null;
		float highestPriority = float.MinValue;

		foreach (Need need in needs) {
			if (need.IsNeeded()) {
				float priority = need.CalculatePriority();

				if (priority > highestPriority) {
					highestPriority = priority;
					mostNeededNeed = need;
				}
			}
		}
		return mostNeededNeed;
	}
	
	/// <summary>
	/// Checks if a need is low enough to kill monster
	/// </summary>
	/// <param name="monster">Monster to check needs on</param>
	/// <returns>Need that killed monster, or null if none kill monster</returns>
	public Need CheckDeath(Monster monster) {
		foreach (Need need in needs) {
			if (need.value <= need.deathValue) {
				return need;
			}
		}
		return null;
	}

	public List<Need> GetNeedList() {
		return needs;
	}

	/// <summary>
	/// Returns the list of needs sorted by priority in descending order.
	/// </summary>
	/// <returns>List of needs sorted by priority.</returns>
	public List<Need> GetNeedsSortedByPriority() {
		return needs.OrderByDescending(need => need.CalculatePriority()).ToList();
	}

	public void InitNeedsFromSaveData(MonsterSaveData saveData) {
		needs.Clear();
		foreach(Need need in saveData.needs) {
			needs.Add(need);
		}
	}

}
