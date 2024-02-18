using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
	[SerializeField] private Button resumeButton;
	[SerializeField] private Slider masterVolumeSlider;
	[SerializeField] private Slider ambientVolumeSlider;
	[SerializeField] private Slider sfxVolumeSlider;
	[SerializeField] private Button mainMenuButton;
	[SerializeField] private Button quitButton;

	private void Start() {
		GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;

		resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);
		mainMenuButton.onClick.AddListener(MainMenu);
		quitButton.onClick.AddListener(QuitGame);
	


		masterVolumeSlider.onValueChanged.AddListener((value) => {
			SoundManager.Instance.MasterVolume = value;
		});
		ambientVolumeSlider.onValueChanged.AddListener((value) => {
			SoundManager.Instance.AmbientVolume = value;
		});
		sfxVolumeSlider.onValueChanged.AddListener((value) => {
			SoundManager.Instance.SFXVolume = value;
		});

		masterVolumeSlider.value = SoundManager.Instance.MasterVolume;
		ambientVolumeSlider.value = SoundManager.Instance.AmbientVolume;
		sfxVolumeSlider.value = SoundManager.Instance.SFXVolume;
		Hide();
	}

	private void MainMenu() {
		GameManager.Instance.SaveToDisk();
		Loader.Load(Loader.Scene.MainMenu);
	}

	private void QuitGame() {
		GameManager.Instance.SaveToDisk();
		GameManager.Instance.QuitGame();
	}

	private void GameManager_OnGameResumed() {
		Hide();
	}

	private void GameManager_OnGamePaused() {
		Show();
	}


	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide() {
		gameObject.SetActive(false);
	}
}
