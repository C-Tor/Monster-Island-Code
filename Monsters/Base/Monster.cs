using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
//Base monster class that other monster classes can inherit from
public class Monster : MonoBehaviour {
	[SerializeField] protected MonsterDataSO monsterTemplate;
	[System.NonSerialized] public MonsterMovement movement;
	[System.NonSerialized] public NeedsController needsController;
	[System.NonSerialized] public MonsterBehaviorController monsterBehavior;
	[System.NonSerialized] public MonsterMemory memory;
	[System.NonSerialized] public MonsterAudio monsterAudio;

	public float daysUntilMaxSize = 1f;
	public float Scale { get; private set; }
	[SerializeField] private float initialScaleFactor;
	[SerializeField] private Transform heightPoint;


	#region Monster Data
	public string monsterName;
	public MonsterType monsterType;
	//public float happiness; //put happiness in Monster rather than NeedsController, as it can be dependant on the needs
	public float interactRadius;
	public float initInteractRadius;
	public float ageDays;

	#endregion

	private void Awake() {

	}

	protected virtual void Start() {
		TimingManager.Instance.OnTick += TimingManager_OnTick;
		TimingManager.Instance.OnHourChanged += TimingManager_OnHourChanged;
		TimingManager.Instance.OnMinuteChanged += Instance_OnMinuteChanged;
		

		transform.localScale = new Vector3(initialScaleFactor, initialScaleFactor, initialScaleFactor);

	}

	private void InitializeMonster(MonsterDataSO dataToUse) {
		needsController = GetComponent<NeedsController>();
		movement = GetComponent<MonsterMovement>();
		monsterBehavior = GetComponent<MonsterBehaviorController>();
		memory = GetComponent<MonsterMemory>();
		monsterAudio = GetComponentInChildren<MonsterAudio>();

		monsterName = dataToUse.monsterType.ToString();
		monsterType = dataToUse.monsterType;
		daysUntilMaxSize = dataToUse.daysUntilMaxSize;

		ageDays = 0;
		initInteractRadius = dataToUse.interactRadius;

		foreach (Need need in dataToUse.Needs) {
			needsController.AddNeed(need);
		}

		MonsterManager.Instance.AddMonster(this);
	}


	public void InitializeMonsterFromTemplate() {
		InitializeMonster(monsterTemplate);
	}

	public void InitializeMonsterFromSaveData(MonsterSaveData saveData) {
		InitializeMonster(monsterTemplate);

		//override values with saved data
		ApplySaveData(saveData);
	}

	private void ApplySaveData(MonsterSaveData saveData) {
		monsterName = saveData.monsterName;
		monsterType = saveData.monsterType;
		daysUntilMaxSize = saveData.daysUntilMaxSize;
		ageDays = saveData.ageDays;

		needsController.InitNeedsFromSaveData(saveData);

		memory.memoryData = saveData.memoryData;
	}

	private void Update() {
	}

	protected virtual void Tick() {
		needsController.Tick();
		CheckDeathFromNeeds();
	}

	private void CheckDeathFromNeeds() {
		Need need = needsController.CheckDeath(this);
		if (need != null) {
			Debug.Log(monsterName + " died from lack of " + need.name);
			Die();
		}
	}

	private void TimingManager_OnTick() {
		Tick();
	}

	private void TimingManager_OnHourChanged() {
		//ageDays += (1f / 24f);
	}

	private void Instance_OnMinuteChanged() {
		ageDays += (1f / 1440f);
		UpdateSize();
	}



	private void Die() {
		TimingManager.Instance.OnTick -= TimingManager_OnTick;
		TimingManager.Instance.OnHourChanged -= TimingManager_OnHourChanged;
		TimingManager.Instance.OnMinuteChanged -= Instance_OnMinuteChanged;
		Destroy(this.gameObject);
	}

	private void UpdateSize() {
		float growthFactor = Mathf.Clamp01(ageDays / daysUntilMaxSize);

		Scale = initialScaleFactor + (1 - initialScaleFactor) * growthFactor;

		// Calculate the new size using the growth factor
		Vector3 newSize = new Vector3(Scale, Scale, Scale);

		// Update the monster's scale
		transform.localScale = newSize;
		interactRadius = initInteractRadius * Scale;

		// Update monster's audio
		monsterAudio.SetScaleInfluence(Scale);

	}

	public float GetHeight() {
		return heightPoint.position.y - transform.position.y;
	}


}
