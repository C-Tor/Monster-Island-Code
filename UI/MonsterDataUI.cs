using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterDataUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI monsterNameText;
	[SerializeField] private TextMeshProUGUI monsterTypeText;
	[SerializeField] private TextMeshProUGUI monsterAgeText;
	[SerializeField] private TextMeshProUGUI monsterHeightText;

	[SerializeField] private Transform monsterNeedsContainer;
	[SerializeField] private Transform needTemplate;

	private void Awake() {
		needTemplate.gameObject.SetActive(false);
	}

	private void Start() {
		MonsterManager.Instance.OnMonsterSelected += MonsterManager_OnMonsterSelected;
		MonsterManager.Instance.OnMonsterDeselected += MonsterManager_OnMonsterDeselected;
		TimingManager.Instance.OnMinuteChanged += TimingManager_OnMinuteChanged;
		Hide();

	}

	private void OnEnable() {
	}

	private void MonsterManager_OnMonsterDeselected() {
		Hide();
	}
	private void MonsterManager_OnMonsterSelected() {
		UpdateData();
		Show();
	}

	private void TimingManager_OnMinuteChanged() {
		UpdateData();
	}


	private void UpdateData() {
		if (MonsterManager.Instance.selectedMonster != null) {
			monsterNameText.text = MonsterManager.Instance.selectedMonster.monsterName;
			monsterTypeText.text = MonsterManager.Instance.selectedMonster.monsterType.ToString();
			monsterAgeText.text = $"{MonsterManager.Instance.selectedMonster.ageDays:0.00} days old";
			monsterHeightText.text = $"{MonsterManager.Instance.selectedMonster.GetHeight():0.00} meters tall";

			UpdateNeeds();
		}
	}

	// Thanks Code Monkey
	private void UpdateNeeds() {
		foreach (Transform child in monsterNeedsContainer) {
			if (child == needTemplate) continue;
			Destroy(child.gameObject);
		}

		foreach(Need need in MonsterManager.Instance.selectedMonster.needsController.GetNeedList()) {
			Transform needTransform = Instantiate(needTemplate, monsterNeedsContainer);
			needTransform.gameObject.SetActive(true);
			needTransform.GetComponent<NeedSingleUI>().SetNeedUI(need);
		}
	}

	private void Show() {
		if (!MonsterManager.Instance.InAdoptionScreen) {
			gameObject.SetActive(true);
		}
	}

	private void Hide() {
		gameObject.SetActive(false);
	}

	private void OnDestroy() {
		MonsterManager.Instance.OnMonsterSelected -= MonsterManager_OnMonsterSelected;
		MonsterManager.Instance.OnMonsterDeselected -= MonsterManager_OnMonsterDeselected;
		TimingManager.Instance.OnMinuteChanged -= TimingManager_OnMinuteChanged;
	}

}
