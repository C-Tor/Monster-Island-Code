using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdoptionUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI notificationText;
	[SerializeField] private TMP_InputField monsterNameInput;
	[SerializeField] private Button submitButton;
	private Monster monster;

	private void Awake() {
		submitButton.onClick.AddListener(SubmitName);
	}

	private void Start() {
		Hide();
		MonsterManager.Instance.OnMonsterHatched += MonsterManager_OnMonsterHatched;
	}

	private void MonsterManager_OnMonsterHatched(Monster monster) {
		Show();
		this.monster = monster;
		notificationText.text = "a " + monster.monsterType.ToString() + " has hatched!";
	}

	private void SubmitName() {
		monster.monsterName = monsterNameInput.text;
		monster = null;
		MonsterManager.Instance.InAdoptionScreen = false;
		Hide();
	}

	private void Show() { gameObject.SetActive(true); }
	private void Hide() { gameObject.SetActive(false); }
}
