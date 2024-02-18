using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private Button newGameButton;
	[SerializeField] private Button continueGameButton;
	[SerializeField] private Button quitGameButton;

	private const string saveFileName = "monsters.json";

	private void Start() {
		newGameButton.onClick.AddListener(NewGame);
		continueGameButton.onClick.AddListener(ContinueGame);
		quitGameButton.onClick.AddListener(QuitGame);
		if (IsFileEmpty(GetSaveFilePath())) {
			Debug.Log("File is empty: " + GetSaveFilePath());
			continueGameButton.interactable = false;
		}
	}



	private void NewGame() {
		File.Delete(GetSaveFilePath());
		Loader.Load(Loader.Scene.GameScene);
	}

	private void ContinueGame() {
		Loader.Load(Loader.Scene.GameScene);
	}

	private void QuitGame() {
		Application.Quit();
	}

	private bool IsFileEmpty(string filePath) {
		// Check if the file exists
		if (File.Exists(filePath)) {
			// Read the contents of the file
			string fileContent = File.ReadAllText(filePath);

			if (string.IsNullOrEmpty(fileContent)) {
				return true;
			}
			List<MonsterSaveData> savedMonsters = LoadMonsters();
			if (savedMonsters.Count == 0) {
				return true;
			}			
			return false;
		}

		// File doesn't exist, so it's considered empty
		return true;
	}

	private List<MonsterSaveData> LoadMonsters() {
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
