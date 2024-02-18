using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
	[SerializeField] private Button settingsButton;
	[SerializeField] private Button closeButton;
	[SerializeField] private Toggle enableEdgePanToggle;
	[SerializeField] private Toggle fullscreenToggle;


	private void Start() {
		enableEdgePanToggle.isOn = SettingsManager.Instance.UseEdgePan;
		enableEdgePanToggle.onValueChanged.AddListener((value) => {
			SettingsManager.Instance.UseEdgePan = value;
		});

		fullscreenToggle.isOn = SettingsManager.Instance.Fullscreen;
		fullscreenToggle.onValueChanged.AddListener((value) => {
			SettingsManager.Instance.Fullscreen = value;
		});
		

		closeButton.onClick.AddListener(() => {
			Hide();
		});

		settingsButton.onClick.AddListener(() => {
			gameObject.SetActive(!isActiveAndEnabled);
		});


		Hide();
	}

	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide() {
		gameObject.SetActive(false);
	}
}
