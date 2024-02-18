using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterSaveManager : MonoBehaviour
{
	public static MonsterSaveManager Instance { get; private set; }
	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}
	private const string saveFileName = "monsters.json";

	public void SaveMonsters(List<Monster> monsters) {
		try {
			MonsterSaveDataList saveDataList = new MonsterSaveDataList(monsters);
			string json = JsonUtility.ToJson(saveDataList);
			File.WriteAllText(GetSaveFilePath(), json);
			Debug.Log("Saved Data: " + json);
			Debug.Log("Saved monsters to: " + GetSaveFilePath());
		} catch (Exception e) {
			Debug.LogError("Error saving monsters: " + e.Message);
		}
	}

	public List<MonsterSaveData> LoadMonsters() {
		string filePath = GetSaveFilePath();

		if (File.Exists(filePath)) {
			string json = File.ReadAllText(filePath);

			Debug.Log("Loaded JSON: " + json); // Add this line for debugging

			// Parse the JSON using a wrapper class
			MonsterSaveDataList saveDataListWrapper = JsonUtility.FromJson<MonsterSaveDataList>(json);

			// Check if the wrapper is not null and contains the monsters list
			if (saveDataListWrapper != null && saveDataListWrapper.monsters != null) {
				return saveDataListWrapper.monsters;
			} else {
				Debug.LogError("Failed to parse monsters from JSON: " + json);
				return new List<MonsterSaveData>();
			}
		} else {
			Debug.LogWarning("Save file not found: " + filePath);
			return new List<MonsterSaveData>();
		}
	}


	private string GetSaveFilePath() {
		return Path.Combine(Application.persistentDataPath, saveFileName);
	}
}
