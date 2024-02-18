using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterSaveDataList {
	public List<MonsterSaveData> monsters;

	public MonsterSaveDataList(List<Monster> monsters) {
		this.monsters = new List<MonsterSaveData>();
		foreach (Monster monster in monsters) {
			try {
				MonsterSaveData saveData = new MonsterSaveData(monster);
				this.monsters.Add(saveData);
			} catch (Exception e) {
				Debug.LogError($"Error creating MonsterSaveData for {monster.name}: {e.Message}");
			}
		}
	}
}