using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	[SerializeField] private MonstersSO monsterList;
	public Transform MonsterSpawnPosition;
	public static MonsterManager Instance { get; private set; }
	public Monster selectedMonster;

	public event Action OnMonsterSelected;
	public event Action OnMonsterDeselected;
	public event Action<Monster> OnMonsterHatched;
	public bool InAdoptionScreen { get; set; }

	public List<Monster> monsters;
	private Dictionary<NeedType, ItemType> needTypeToItemTypeMapping;



	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		} else {
			Instance = this;
			monsters = new List<Monster>();
			InitializeTypeMapping();

			GameManager.Instance.OnContinueGame += GameManager_OnContinueGame;
			GameManager.Instance.OnNewGame += GameManager_OnNewGame;
		}

	}

	private void Start() {
		InAdoptionScreen = false;
	}

	private void GameManager_OnNewGame() {
		CreateEgg(monsterList.MudSkipperEgg);
	}

	private void GameManager_OnContinueGame() {
		SelectMonster(monsters[0]);
	}

	// Adds a monster to the monster list
	public void AddMonster(Monster monster) {
		if (monster != null) {
			monsters.Add(monster);
		} else {
			Debug.LogWarning("Attempted to add a null monster.");
		}
	}

	// Adds monster from saveData
	public void AddFromSaveData(MonsterSaveData saveData) {
		Debug.Log("Adding From Save Data");
		Monster monster = Instantiate(monsterList.MudSkipper, saveData.worldPosition, Quaternion.identity);
		monster.InitializeMonsterFromSaveData(saveData);
	}

	// Creates a new blank monster from template.
	public void CreateNewMonster(Monster monster) {
		Debug.Log("Creating New Monster");
		Monster newMonster = Instantiate(monster, MonsterSpawnPosition.position, Quaternion.identity);
		newMonster.InitializeMonsterFromTemplate();
		PrintMonsters();
	}

	// to call an event when an egg is hatched.
	public void EggHatched(Monster monster) {
		InAdoptionScreen = true;
		OnMonsterHatched?.Invoke(monster);
	}

	public void CreateEgg(Egg egg) {
		Egg newEgg = Instantiate(egg, MonsterSpawnPosition.position, Quaternion.identity);
	}

	private void Update() {
		//if (Input.GetKeyDown(KeyCode.C)) {
		//	CreateEgg(monsterList.MudSkipperEgg);
		//} if (Input.GetKeyDown(KeyCode.X)) {
		//	CreateNewMonster(monsterList.MudSkipper);
		//}
		//if (Input.GetKeyDown(KeyCode.V)) {
		//	PrintMonsters();
		//}
	}

	public void SelectMonster(Monster monster) {
		Debug.Log("Monter selected");
		OnMonsterSelected?.Invoke();
		selectedMonster = monster;
	}

	public void DeselectMonster() {
		OnMonsterDeselected?.Invoke();
		selectedMonster = null;
	}

	public void RemoveInteractableFromAllMonsters(Interactable objectToRemove) {
		foreach (Monster monster in monsters) {
			monster.monsterBehavior.RemoveFromDetected(objectToRemove);
		}
	}

	private void PrintMonsters() {
		Debug.Log("Monsters: ");
		if (monsters.Count == 0) {
			Debug.Log("No Monsters");
		} else {
			foreach (var monster in monsters) {
				Debug.Log("monster: " + monster);
			}
		}
		Debug.Log("====================================");
	}



	private void InitializeTypeMapping() {
		needTypeToItemTypeMapping = new Dictionary<NeedType, ItemType>
		{
			{ NeedType.Food, ItemType.Food },
			{ NeedType.Energy, ItemType.Bed },
			{ NeedType.Fun, ItemType.Toy }
            // Add more mappings as needed
        };
	}

	public ItemType GetItemTypeForNeed(NeedType needType) {
		// Use the dictionary to get the corresponding ItemType
		if (needTypeToItemTypeMapping.TryGetValue(needType, out ItemType itemType)) {
			return itemType;
		}

		// Return a default value if the mapping doesn't exist
		return ItemType.None;
	}
}
