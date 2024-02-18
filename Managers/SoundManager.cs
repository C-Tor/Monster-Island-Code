using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance { get; private set; }
	public AudioMixer Mixer;
	private Transform playerTransform;

	#region Volume Fields
	private float masterVolume; //private backing field
	public float MasterVolume { get => masterVolume;
		set {
			float setValue = Mathf.Clamp(value, 0.0001f, 1f);
			masterVolume = setValue;
			Mixer.SetFloat("MasterVolume", Mathf.Log10(setValue) * 20);
			PlayerPrefs.SetFloat("MasterVolume", setValue);
		} 
	}

	private float ambientVolume; //private backing field
	public float AmbientVolume {
		get => ambientVolume;
		set {
			float setValue = Mathf.Clamp(value, 0.0001f, 1f);
			ambientVolume = setValue;
			Mixer.SetFloat("AmbientVolume", Mathf.Log10(setValue) * 20);
			PlayerPrefs.SetFloat("AmbientVolume", setValue);
		}
	}

	private float sfxVolume; //private backing field
	public float SFXVolume {
		get => sfxVolume;
		set {
			float setValue = Mathf.Clamp(value, 0.0001f, 1f);
			sfxVolume = setValue;
			Mixer.SetFloat("SFXVolume", Mathf.Log10(setValue) * 20);
			PlayerPrefs.SetFloat("SFXVolume", setValue);
		}
	}
	#endregion

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			//DontDestroyOnLoad(gameObject);
			// Get saved volume
			MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
			AmbientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1);
			SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
			// make sure it's good value
			MasterVolume = Mathf.Clamp01(MasterVolume);
		} else {
			Destroy(this.gameObject);
		}
	}

	public void SetPlayer(Transform playerTransform) {
		this.playerTransform = playerTransform;
	}

	public Transform GetPlayer() { return playerTransform; }

}
