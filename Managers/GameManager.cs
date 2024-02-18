using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;




	public bool IsGamePaused {  get; private set; }
	public event Action OnGamePaused;
	public event Action OnGameResumed;
	public event Action OnNewGame;
	public event Action OnContinueGame;
	public event Action<InteractiveObject> OnInteractiveObjectDrop;


	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(this);
		}
	}

	private void Start() {
		TimingManager.Instance.OnHourChanged += TimingManager_OnHourChanged;

		LoadFromDisk();
		bool newGame = MonsterManager.Instance.monsters.Count == 0;
		if (newGame) {
			NewGameInit();
		} else {
			ContinueGameInit();
		}
		ResumeGame();
	}

	private void TimingManager_OnHourChanged() {
		SaveToDisk();
	}

	private void NewGameInit() {
		Debug.Log("New game loaded");
		OnNewGame?.Invoke();
	}
	private void ContinueGameInit() {
		OnContinueGame?.Invoke();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (IsGamePaused) { ResumeGame(); }
			else { PauseGame(); }
		}
	}

	public void PauseGame() {
		Time.timeScale = 0;
		IsGamePaused = true;
		OnGamePaused?.Invoke();
	}

	public void ResumeGame() {
		Time.timeScale = 1;
		IsGamePaused = false;
		OnGameResumed?.Invoke();
	}

	public void QuitGame() {
		Debug.Log("Quitting game");
		Application.Quit();
	}

	#region Saving/Loading
	public void SaveToDisk() {
		if (MonsterSaveManager.Instance != null) {
			Debug.Log($"Saving {MonsterManager.Instance.monsters.Count} monsters to disk.");
			MonsterSaveManager.Instance.SaveMonsters(MonsterManager.Instance.monsters);
		}
	}

	public void LoadFromDisk() {
		Debug.Log("Loading Monsters");
		if (MonsterSaveManager.Instance != null) {
			List<MonsterSaveData> saveDataList = MonsterSaveManager.Instance.LoadMonsters();

			foreach (MonsterSaveData saveData in saveDataList) {
				// Instantiate monsters based on save data
				// You might want to modify this based on your instantiation logic
				MonsterManager.Instance.AddFromSaveData(saveData);
			}
		}
	}
	#endregion



	public void DropObject(InteractiveObject obj) {
		OnInteractiveObjectDrop?.Invoke(obj);
	}


}
