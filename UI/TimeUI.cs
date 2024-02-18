using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI clockText;

	private void Start() {
		TimingManager.Instance.OnMinuteChanged += TimingManager_OnMinuteChanged;
		TimingManager.Instance.OnHourChanged += TimingManager_OnHourChanged;
		UpdateTime();
	}

	private void TimingManager_OnHourChanged() {
		UpdateTime();
	}

	private void TimingManager_OnMinuteChanged() {
		UpdateTime();
	}

	private void OnDisable() {
		TimingManager.Instance.OnMinuteChanged -= TimingManager_OnMinuteChanged;
		TimingManager.Instance.OnHourChanged -= TimingManager_OnHourChanged;
	}

	private void UpdateTime() {
		clockText.text = $"{TimingManager.Hour:00}:{TimingManager.Minute:00}";
	}
}
