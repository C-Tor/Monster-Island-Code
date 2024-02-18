using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientSoundPlayer : MonoBehaviour
{

	[SerializeField] private float beachMaxVolLocation;
	[SerializeField] private float landMaxVolLocation;
	[SerializeField] private float skyMaxVolLocation;
	[SerializeField] private float maxHeight;
	[SerializeField] private float transitionSmoothTime;

	private Transform playerTransform;
	private SoundManager sm;

	private void Start() {
		sm = SoundManager.Instance;
	}


	private void Update() {
		HandleAudioZones();
	}

	#region AudioZones

	private float beachVolume; //private backing field
	private float BeachVolume {
		get => beachVolume;
		set {
			float setValue = Mathf.Clamp(value, 0.0001f, 1f);
			sm.Mixer.SetFloat("Beach", Mathf.Log10(setValue) * 20);
			beachVolume = setValue;
		}
	}

	private float landVolume; //private backing field
	private float LandVolume {
		get => landVolume;
		set {
			float setValue = Mathf.Clamp(value, 0.0001f, 1f);
			sm.Mixer.SetFloat("Land", Mathf.Log10(setValue) * 20);
			landVolume = setValue;
		}
	}

	private float skyVolume; //private backing field
	private float SkyVolume {
		get => skyVolume;
		set {
			float setValue = Mathf.Clamp(value, 0.0001f, 1f);
			sm.Mixer.SetFloat("Sky", Mathf.Log10(setValue) * 20);
			skyVolume = setValue;
		}
	}



	#endregion

	private void HandleAudioZones() {
		float playerHeight = sm.GetPlayer().position.y;

		float normalizedHeight = Mathf.Clamp01(playerHeight / maxHeight);
		//Debug.Log("normalizedHeight: " + normalizedHeight);


		float targetBeachVolume = CalculateParabolaVolume(normalizedHeight, beachMaxVolLocation);
		//Debug.Log("BeachVolume: " + beachVolume);
		float targetLandVolume = CalculateParabolaVolume(normalizedHeight, landMaxVolLocation);
		//Debug.Log("LandVolume: " + LandVolume);
		float targetSkyVolume = CalculateParabolaVolume(normalizedHeight, skyMaxVolLocation);
		//Debug.Log("SkyVolume: " + SkyVolume);


		BeachVolume = Mathf.Lerp(BeachVolume, targetBeachVolume, Time.deltaTime * transitionSmoothTime);
		LandVolume = Mathf.Lerp(LandVolume, targetLandVolume, Time.deltaTime * transitionSmoothTime);
		SkyVolume = Mathf.Lerp(SkyVolume, targetSkyVolume, Time.deltaTime * transitionSmoothTime);

	}

	[SerializeField] private float parabolaFactor;
	[SerializeField] private float parabolaAdd;

	private float CalculateParabolaVolume(float t, float apex) {
		// Quadratic function for a parabola: f(t) = a * (t - h)^2 + k
		// Where (h, k) is the vertex (apex) of the parabola
		// We set a = 4 to ensure the parabola opens upwards and reaches 1 at the apex

		float a = parabolaFactor;
		float k = parabolaAdd;
		float height = a * Mathf.Pow(t - apex, 2) + k;

		return Mathf.Clamp01(height);
	}


}
