using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
	public static SettingsManager Instance { get; private set; }

	private bool useEdgePan;
	public bool UseEdgePan {
		get { return useEdgePan; }
		set {
			useEdgePan = value;
			PlayerPrefs.SetInt("UseEdgeScroll", BoolTools.BoolToInt(useEdgePan));
			Debug.Log("Changing Edge Scroll to " + useEdgePan);
		}
	}

	private bool fullscreen;
	public bool Fullscreen {
		get { return fullscreen; }
		set {
			fullscreen = value;
			Screen.fullScreen = fullscreen;
			PlayerPrefs.SetInt("Fullscreen", BoolTools.BoolToInt(fullscreen));
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreen);
			Debug.Log("Changing Fullscreen to " + fullscreen);
		}
	}

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			//DontDestroyOnLoad(gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}

	private void Start() {
		LoadSettings();
	}

	public void LoadSettings() {
		// Load settings, including the monitor value
		Fullscreen = BoolTools.IntToBool(PlayerPrefs.GetInt("Fullscreen", 0));
		UseEdgePan = BoolTools.IntToBool(PlayerPrefs.GetInt("UseEdgeScroll", 0));

	}

}
