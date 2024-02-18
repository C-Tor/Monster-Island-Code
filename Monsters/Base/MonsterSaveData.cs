using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSaveData 
{
	public Vector3 worldPosition;
	public string monsterName;
	public MonsterType monsterType;
	public float daysUntilMaxSize;
	public float ageDays;
	public MemoryData memoryData;

	public List<Need> needs;

	public MonsterSaveData(Monster monster) {
		worldPosition = monster.transform.position;
		monsterName = monster.monsterName;
		monsterType = monster.monsterType;
		daysUntilMaxSize = monster.daysUntilMaxSize;
		ageDays = monster.ageDays;

		memoryData = monster.memory.memoryData;

		needs = monster.needsController.GetNeedList();
	}
}
